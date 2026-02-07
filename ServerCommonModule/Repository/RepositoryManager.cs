using ServerCommonModule.Attributes;
using ServerCommonModule.Repository.Interfaces;
using ServerCommonModule.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ServerCommonModule.Model;
using ServerCommonModule.Support;

namespace ServerCommonModule.Repository
{
    public partial class RepositoryManager<T> : IRepositoryManager<T>
    {
        protected DataCollection<T>? _collection = null;
        private bool _hasModifiedDate = true;
        private readonly Dictionary<string, CollectionProperty> _collectionProperties = [];
        private const string MODIFIED_FIELD = "modified_date";

        private readonly IDbUtility _dbUtility;
        private readonly IEnvironmentalParameters _environmentalParameters;

        private enum FieldsGetter
        {
            PrimaryKeys,
            NotPrimaryKeys,
            /// <summary> Similar to "NotPrimaryKeys" but without identity fields. Because if an UPDATE is created using NotPrimaryKeys it will fail on identity fields. </summary>
            NotPrimaryKeysUpdeatable,
            All,
            /// <summary> Similar to "All" but without identity fields. Because if an INSERT is created using all it will fail on identity fields. </summary>
            AllInsertable
        }

        public RepositoryManager(IDbUtilityFactory dbUtilityFactory, IEnvironmentalParameters environmentParameters)
        {
            _dbUtility = dbUtilityFactory.Get();
            _environmentalParameters = environmentParameters;

        }



        public void SetCollection(DataCollection<T> collection)
        {
            _collection = collection;
            GetTableProperties();
            GetCollectionProperties();
        }



        #region Private methods

        private async Task Load(IDbConnection connection, IDbTransaction transaction, string query, List<string> whereConditions, List<IDataParameter> whereParameters)
        {
            if (whereConditions.Count > 0)
            {
                query += "\nWHERE\n\t" + string.Join("\tAND ", whereConditions);

                if (transaction != null)
                    using (IDataReader dataReader = await _dbUtility.ExecuteReader(transaction, query, [.. whereParameters]))
                        GetItems(dataReader);
                else
                    using (IDataReader dataReader = await _dbUtility.ExecuteReader(connection, query, [.. whereParameters]))
                        GetItems(dataReader);
            }
            else
            {
                if (transaction != null)
                    using (IDataReader dataReader = await _dbUtility.ExecuteReader(transaction, query))
                        GetItems(dataReader);
                else
                    using (IDataReader dataReader = await _dbUtility.ExecuteReader(connection, query))
                        GetItems(dataReader);
            }


        }



        /// <summary>
        /// Adds where condition on additional properties to filter the loading collection
        /// </summary>
        private void GetAdditionalWhereCondition(List<string> whereConditions, List<IDataParameter> whereParameters, string tablePrefix = "")
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            IEnumerable<KeyValuePair<string, CollectionProperty>> fields = _collectionProperties.Where(x => x.Value.IsPrimaryKey);
            List<string> keysWhereCondition = [];

            //int parameterIndex = 1;
            T defaultItem = _collection.CreateItem();
            bool whereExists = false;

            StringBuilder builder = new();
            for (int i = 0; i < keysWhereCondition.Count; i++)
            {
                whereExists = true;
                if (i == 0 && keysWhereCondition.Count > 1)
                    builder.Append('(');

                builder.Append(keysWhereCondition[i]);

                if (i < keysWhereCondition.Count - 1)
                    builder.Append(" OR ");
                else if (i == keysWhereCondition.Count - 1 && keysWhereCondition.Count > 1)
                    builder.Append(')');
            }
            if (whereExists == true) whereConditions.Add(builder.ToString());
        }





        private IDataParameter CreateSqlParameter(string parameterName, SqlDbType dbType, object value)
        {
            if (dbType == SqlDbType.UniqueIdentifier)
            {
                Guid guidValue = value is Guid guid ? guid : Guid.Parse((string)value);
                return _dbUtility.CreateSqlParameter(parameterName, dbType, guidValue);
            }
            else
            {
                return _dbUtility.CreateSqlParameter(parameterName, dbType, value);
            }
        }

        private void GetItems(IDataReader dataReader)
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            while (dataReader.Read())
            {
                T genericTObj = _collection.CreateItem();
                Debug.Assert(genericTObj != null);

                int i = 0;
                foreach (CollectionProperty field in _collectionProperties.Values)
                {
                    if (dataReader.IsDBNull(i))
                    {
                        i++;
                        continue;
                    }

                    PropertyInfo propertyInfo = field.Info;
                    bool uppercase = field.Uppercase;

                    if (propertyInfo.PropertyType == typeof(Version))
                    {
                        string? versionString = dataReader.GetString(i);
                        if (!string.IsNullOrEmpty(versionString) && Version.TryParse(versionString, out Version? version))
                            propertyInfo.SetValue(genericTObj, version);
                    }
                    else if (propertyInfo.PropertyType.IsEnum)
                    {
                        Type fieldType = dataReader.GetFieldType(i);
                        if (fieldType == typeof(string))
                        {
                            string? enumString = dataReader.GetString(i);
                            if (enumString != null)
                                propertyInfo.SetValue(genericTObj, Enum.ToObject(propertyInfo.PropertyType, Enum.Parse(propertyInfo.PropertyType, enumString, true)), null);
                        }
                        else if (fieldType == typeof(int))
                            propertyInfo.SetValue(genericTObj, Enum.ToObject(propertyInfo.PropertyType, dataReader.GetInt32(i)), null);
                        else
                            throw new ApplicationException();
                    }
                    else
                    {
                        switch (field.FieldType)
                        {
                            case SqlDbType.TinyInt:
                                propertyInfo.SetValue(genericTObj, dataReader.GetByte(i));
                                break;
                            case SqlDbType.SmallInt:
                                propertyInfo.SetValue(genericTObj, dataReader.GetInt16(i));
                                break;
                            case SqlDbType.Int:
                                propertyInfo.SetValue(genericTObj, dataReader.GetInt32(i));
                                break;
                            case SqlDbType.BigInt:
                                propertyInfo.SetValue(genericTObj, dataReader.GetInt64(i));
                                break;
                            case SqlDbType.Decimal:
                                propertyInfo.SetValue(genericTObj, dataReader.GetDecimal(i));
                                break;
                            case SqlDbType.VarChar:
                            case SqlDbType.NVarChar:
                                if (uppercase)
                                    propertyInfo.SetValue(genericTObj, dataReader.GetString(i).ToUpper());
                                else
                                    propertyInfo.SetValue(genericTObj, dataReader.GetString(i));
                                break;
                            case SqlDbType.Bit:
                                propertyInfo.SetValue(genericTObj, dataReader.GetBoolean(i));
                                break;
                            case SqlDbType.DateTime:
                            case SqlDbType.Date:
                                propertyInfo.SetValue(genericTObj, dataReader.GetDateTime(i));
                                break;
                            case SqlDbType.UniqueIdentifier:
                                propertyInfo.SetValue(genericTObj, dataReader.GetGuid(i));
                                break;
                            case SqlDbType.VarBinary:
                                long len = dataReader.GetBytes(i, 0, null, 0, 0);
                                Byte[] buffer = new Byte[len];
                                dataReader.GetBytes(i, 0, buffer, 0, (int)len);
                                propertyInfo.SetValue(genericTObj, buffer);
                                break;
                        }
                    }
                    i++;
                }

                _collection.Add(genericTObj);
            }
        }

        private string GetNotMatchedSearchConditionSingleItem(string query, bool isDDTable, bool hasFilterParameters, List<IDataParameter> mergeParameters)
        {
            if (string.IsNullOrEmpty(query) == false)
                query += " AND ";

            if (isDDTable)
            {
                query += "feature_key = @FeatureKey";
            }

            if (string.IsNullOrEmpty(query) == false && hasFilterParameters)
                query += " AND ";

            if (hasFilterParameters)
            {
                List<String> whereConditions = [];
                List<IDataParameter> whereParameters = [];
                GetAdditionalWhereCondition(whereConditions, whereParameters);
                mergeParameters.AddRange(whereParameters);
                query += string.Join(" AND ", whereConditions);
            }

            return query;
        }

        private string GetNotMatchedSearchCondition(string partialQuery, bool isDDTable, bool hasFilterParameters, List<IDataParameter> mergeParameters)
        {
            string query = string.IsNullOrEmpty(partialQuery) ? string.Empty : "AND" + partialQuery;

            if (hasFilterParameters)
            {
                List<string> whereConditions = [];
                List<IDataParameter> whereParameters = [];
                GetAdditionalWhereCondition(whereConditions, whereParameters, "target.");
                mergeParameters.AddRange(whereParameters);
                query += " AND " + string.Join(" AND ", whereConditions);
            }

            return query;
        }

        private async Task Save(IDbConnection connection, IDbTransaction? transaction, string partialQuery = "", List<IDataParameter>? partialQueryParameters = null)
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            List<IDataParameter> safePartialQueryParameters = partialQueryParameters ?? [];

            GetStatements(
                partialQuery,
                safePartialQueryParameters,
                out string tempTablePrefix,
                out List<IDataParameter> mergeParameters,
                out DataTable tableToSave,
                out List<string> allFieldList,
                out string temporaryTableCreationQuery,
                out string preMergeDelete,
                out StringBuilder mergeStatement
            );

            string tempTableName = tempTablePrefix + _collection.TableName;

            if (transaction != null)
            {
                // 1. Create temp table
                await _dbUtility.ExecuteNonQuery(transaction, temporaryTableCreationQuery);

                // ⭐ 2. Insert DataTable rows into temp table
                await InsertRowsIntoTempTable(connection, transaction, tempTableName, tableToSave);

                // 3. Pre-merge delete
                if (!string.IsNullOrEmpty(preMergeDelete))
                    await _dbUtility.ExecuteNonQuery(transaction, preMergeDelete, mergeParameters.ToArray());

                // 4. MERGE
                await _dbUtility.ExecuteNonQuery(transaction, mergeStatement.ToString(), mergeParameters.ToArray());
            }
            else
            {
                // 1. Create temp table
                await _dbUtility.ExecuteNonQuery(connection, temporaryTableCreationQuery);

                // ⭐ 2. Insert DataTable rows into temp table
                await InsertRowsIntoTempTable(connection, transaction, tempTableName, tableToSave);

                // 3. Pre-merge delete
                if (!string.IsNullOrEmpty(preMergeDelete))
                    await _dbUtility.ExecuteNonQuery(connection, preMergeDelete, mergeParameters.ToArray());

                // 4. MERGE
                await _dbUtility.ExecuteNonQuery(connection, mergeStatement.ToString(), mergeParameters.ToArray());
            }
        }


        private void GetStatements(string partialQuery, List<IDataParameter> partialQueryParameters, out string tempTablePrefix, out List<IDataParameter> mergeParameters, out DataTable tableToSave, out List<string> allFieldList, out string temporaryTableCreationQuery, out string preMergeDelete, out StringBuilder mergeStatement)
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            mergeParameters = [];
            tableToSave = GetCollectionDataTable();
            allFieldList = GetFields(FieldsGetter.AllInsertable, true);
            List<string> primaryKeyList = GetFields(FieldsGetter.PrimaryKeys, true);
            List<string> updateFieldList = GetFields(FieldsGetter.NotPrimaryKeysUpdeatable, true);

            List<string> temporaryTableFieldList = GetFields(FieldsGetter.All, true);

            preMergeDelete = string.Empty;
            mergeStatement = new StringBuilder();

            switch (_environmentalParameters.Database)
            {
                case ConnectionType.POSTGRESS:
                    {
                        tempTablePrefix = "tt_";

                        temporaryTableCreationQuery = $@"
create temporary table if not exists  {tempTablePrefix}{_collection.TableName} as
SELECT {string.Join(", ", temporaryTableFieldList)} FROM  {_collection.TableNameWithSchema} WHERE 1=0;
";

                        string checkOnPrimaryKeys = string.Join(" AND ", primaryKeyList.Select(x => "target." + x + " = source." + x));
                        string updateFields = string.Join(", ", updateFieldList.Select(x => x + " = source." + x));

                        string sourceTableName = tempTablePrefix + _collection.TableName;

                        string getNotMatchedSearchCondition = GetNotMatchedSearchCondition(partialQuery, false, false, mergeParameters);
                        if (string.IsNullOrEmpty(getNotMatchedSearchCondition) == false)
                        {
                            bool startsWithAnd = getNotMatchedSearchCondition.TrimStart().StartsWith("AND");
                            if (startsWithAnd == true) getNotMatchedSearchCondition += Environment.NewLine;
                            else getNotMatchedSearchCondition = " AND " + getNotMatchedSearchCondition + Environment.NewLine;
                        }

                        preMergeDelete = $"DELETE FROM {_collection.TableNameWithSchema} AS target" + Environment.NewLine +
                            $"WHERE NOT EXISTS(" + Environment.NewLine +
                            $"	SELECT NULL" + Environment.NewLine +
                            $"  FROM {sourceTableName} source " + Environment.NewLine +
                            $"  WHERE ({checkOnPrimaryKeys})" + Environment.NewLine +
                            getNotMatchedSearchCondition +
                            $")";

                        mergeStatement.Append($"MERGE INTO {_collection.TableNameWithSchema} AS target" + Environment.NewLine +
                            $"USING {tempTablePrefix}{_collection.TableName} AS source" + Environment.NewLine +
                            $"  ON ({checkOnPrimaryKeys})");

                        if (string.IsNullOrEmpty(updateFields) == false)
                        {
                            mergeStatement.AppendLine($"WHEN MATCHED THEN" + Environment.NewLine +
                            $"  UPDATE SET {updateFields}");
                        }

                        mergeStatement.AppendLine($"WHEN NOT MATCHED THEN" + Environment.NewLine +
                            $"  INSERT" + Environment.NewLine +
                            $"      ({string.Join(", ", allFieldList)})" + Environment.NewLine +
                            $"  VALUES" + Environment.NewLine +
                            $"      (source.{string.Join(", source.", allFieldList)})" + Environment.NewLine +
                            $";");

                        break;
                    }
                case ConnectionType.MS_SQL:
                default:
                    {
                        tempTablePrefix = (string.IsNullOrEmpty(_collection.Schema) ? "#" : _collection.Schema + ".#");

                        temporaryTableCreationQuery = $"IF (OBJECT_ID('tempdb..{tempTablePrefix}{_collection.TableName}') IS NULL) BEGIN SELECT {string.Join(", ", temporaryTableFieldList)} INTO {tempTablePrefix}{_collection.TableName} FROM {_collection.TableNameWithSchema} WHERE 1=0 END";
                        string checkOnPrimaryKeys = string.Join(" AND ", primaryKeyList.Select(x => "target." + x + " = source." + x));
                        string updateFields = string.Join(", ", updateFieldList.Select(x => "target." + x + " = source." + x));

                        string sourceTableName = tempTablePrefix + _collection.TableName;

                        mergeStatement.Append($"MERGE {_collection.TableNameWithSchema} AS target" + Environment.NewLine +
                            $"USING {tempTablePrefix}{_collection.TableName} AS source" + Environment.NewLine +
                            $"  ON({checkOnPrimaryKeys})");

                        if (string.IsNullOrEmpty(updateFields) == false)
                        {
                            mergeStatement.AppendLine($"WHEN MATCHED THEN" + Environment.NewLine +
                            $"  UPDATE SET {updateFields}");
                        }

                        mergeStatement.AppendLine($"WHEN NOT MATCHED BY target THEN" + Environment.NewLine +
                            $"  INSERT" + Environment.NewLine +
                            $"      ({string.Join(", ", allFieldList)})" + Environment.NewLine +
                            $"  VALUES" + Environment.NewLine +
                            $"      (source.{string.Join(", source.", allFieldList)})" + Environment.NewLine +
                            $"WHEN NOT MATCHED BY source" + Environment.NewLine +
                            GetNotMatchedSearchCondition(partialQuery, false, false, mergeParameters) +
                            $"  THEN DELETE" + Environment.NewLine +
                            $";");

                        break;
                    }
            }

            if (partialQueryParameters != null)
                mergeParameters.AddRange(partialQueryParameters);
        }

        private async Task ExecuteSingleItemAction(IDbConnection connection, IDbTransaction transaction, T item, bool insert, bool update, bool delete, string partialQuery = "", List<IDataParameter>? partialQueryParameters = null)
        {

            Dictionary<string, IDataParameter> itemParameters = GetSingleItemAsParameters(item);
            List<IDataParameter> allParameters = [];

            // Fix CS8604: Ensure partialQueryParameters is not null

            Console.WriteLine("=== ITEM PARAMETERS ==="); foreach (var kv in itemParameters) { Console.WriteLine($"{kv.Key} -> {kv.Value.ParameterName}, TYPE: {kv.Value.DbType}"); }


            string statement = ExecuteSingleActionPrepareQueries(insert, update, delete, partialQuery, partialQueryParameters ?? [], itemParameters, allParameters);

            Console.WriteLine("=== ALL PARAMETERS AFTER MERGE ==="); foreach (DbParameter p in allParameters) { Console.WriteLine($"{p.ParameterName}, TYPE: {p.DbType}, VALUE: {p.Value}"); }

            if (transaction != null)
            {
                await _dbUtility.ExecuteNonQuery(transaction, statement, [.. allParameters]);
            }
            else
            {
                await _dbUtility.ExecuteNonQuery(connection, statement, [.. allParameters]);
            }
        }

        private string ExecuteSingleActionPrepareQueries(bool insert, bool update, bool delete, string partialQuery, List<IDataParameter> partialQueryParameters, Dictionary<string, IDataParameter> itemParameters, List<IDataParameter> allParameters)
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            List<string> allFieldList = GetFields(FieldsGetter.AllInsertable, true);
            List<string> primaryKeyList = GetFields(FieldsGetter.PrimaryKeys, true);
            List<string> updateFieldList = GetFields(FieldsGetter.NotPrimaryKeysUpdeatable, true);


            string whereCondition = string.Join(" AND ", primaryKeyList.Select(x => x + " = " + itemParameters[x].ParameterName));
            string tempPartial = GetNotMatchedSearchConditionSingleItem(partialQuery, false, false, allParameters);
            if (string.IsNullOrEmpty(tempPartial) == false)
            {
                if (string.IsNullOrEmpty(whereCondition) == false)
                    whereCondition += " AND " + tempPartial;
                else
                    whereCondition = tempPartial;
            }
            AddParametersWithReplace(allParameters, partialQueryParameters);
            AddParametersWithReplace(allParameters, itemParameters.Values);

            string statement = string.Empty;
            if (insert)
            {

                statement = @"
    INSERT INTO " + _collection.TableNameWithSchema + @"
    (" + string.Join(", ", allFieldList) + @")
    VALUES
    (" + string.Join(", ", allFieldList.Select(x => itemParameters[x].ParameterName)) + @")
    ";
            }
            else if (update)
            {
                statement = @"
    UPDATE " + _collection.TableNameWithSchema + @" SET
    " + string.Join(", ", updateFieldList.Select(x => x + " = " + itemParameters[x].ParameterName)) + @"
    WHERE
    " + whereCondition + @"
    ";
            }
            else if (delete)
            {
                statement = @"
    DELETE FROM " + _collection.TableNameWithSchema + @" 
    WHERE
    " + whereCondition + @"
    ";
            }

            return statement;
        }

        private static void AddParametersWithReplace(List<IDataParameter> masterParameters, IEnumerable<IDataParameter> additionalParameters)
        {
            if (additionalParameters == null)
                return;

            foreach (DbParameter incoming in additionalParameters.Cast<DbParameter>())
            {
                // If incoming has no explicit type, skip it
                if (incoming.DbType == DbType.String && incoming.Value is DateTime)
                    continue;

                var existing = masterParameters
                    .FirstOrDefault(x => string.Equals(x.ParameterName, incoming.ParameterName, StringComparison.InvariantCultureIgnoreCase));

                if (existing != null)
                {
                    // If existing is typed and incoming is not, skip replacement
                    if (existing.DbType != DbType.String && incoming.DbType == DbType.String)
                        continue;

                    masterParameters.Remove(existing);
                }

                masterParameters.Add(incoming);
            }
        }



        private void GetTableProperties()
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            foreach (var custAtt in typeof(T).GetCustomAttributes())
            {
                if (custAtt is TableAttribute nameFld && nameFld != null && !string.IsNullOrEmpty(nameFld.Name))
                {
                    _collection.TableName = nameFld.Name;
                }
            }
        }



        private void GetCollectionProperties()
        {
            HasModifiedDateAttribute? hasModifiedDateAttribute = typeof(T).GetCustomAttribute<HasModifiedDateAttribute>();
            if (hasModifiedDateAttribute != null)
                _hasModifiedDate = hasModifiedDateAttribute.HasModifiedDate;

            foreach (PropertyInfo property in typeof(T).GetProperties(DataCollection<T>.BINDING_FLAGS))
            {
                if (property.GetCustomAttributes(true).Any())
                {
                    FieldNameAttribute? fieldNameAttribute = property.GetCustomAttribute<FieldNameAttribute>();
                    string fieldName = string.Empty;
                    if (fieldNameAttribute != null)
                        fieldName = fieldNameAttribute.FieldName;

                    if (string.IsNullOrEmpty(fieldName) == false)
                        _collectionProperties.Add(fieldName, new CollectionProperty(property, fieldName));
                }
            }

            Console.WriteLine("=== COLLECTION PROPERTIES ==="); foreach (var kv in _collectionProperties) { Console.WriteLine($"{kv.Key} -> {kv.Value.Info.Name}"); }
            Console.WriteLine("=============================");
        }

        private string GenerateSelectQuery(bool useReadUncommitted)
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            string sql = $"SELECT \n\t{string.Join(",\n\t", GetFields(FieldsGetter.All, true, false))} \nFROM \n\t{_collection.TableNameWithSchema}";
            sql += useReadUncommitted ? " WITH (READUNCOMMITTED)" : string.Empty;

            return sql;
        }

        private List<string> GetFields(FieldsGetter fieldsGetter, bool addBrackets, bool forInsert = true)
        {
            List<string> fields = [];

            fields = fieldsGetter switch
            {
                FieldsGetter.PrimaryKeys => [.. _collectionProperties.Where(x => x.Value.IsPrimaryKey).Select(x => x.Key)],
                FieldsGetter.NotPrimaryKeys => [.. _collectionProperties.Where(x => x.Value.IsPrimaryKey == false).Select(y => y.Key)],
                FieldsGetter.NotPrimaryKeysUpdeatable => [.. _collectionProperties.Where(x => x.Value.IsPrimaryKey == false).Select(y => y.Key)],
                FieldsGetter.All => [.. _collectionProperties.Where(x => x.Value.IsPrimaryKey == false || x.Value.IsPrimaryKey == true).Select(y => y.Key)],
                FieldsGetter.AllInsertable => [.. _collectionProperties.Where(x => x.Value.Identity == false).Select(y => y.Key)],
                _ => [],
            };
            if (fieldsGetter != FieldsGetter.PrimaryKeys && _hasModifiedDate && !fields.Contains(MODIFIED_FIELD)) { fields.Add(MODIFIED_FIELD); }

            return fields ?? [];
        }

        private DataTable GetCollectionDataTable()
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            DataTable collectionDataTable = new(_collection.TableName);



            foreach (KeyValuePair<string, CollectionProperty> field in _collectionProperties)
            {

                collectionDataTable.Columns.Add(field.Key, Utility.ToType(field.Value.FieldType));
            }



            DataRow row;
            foreach (T item in _collection)
            {
                row = collectionDataTable.NewRow();

                int i = 0;
                foreach (KeyValuePair<string, CollectionProperty> field in _collectionProperties)
                {
                    PropertyInfo propertyInfo = field.Value.Info;
                    bool uppercase = field.Value.Uppercase;


                    object? value = propertyInfo.GetValue(item);

                    if (value == null)
                        row[field.Key] = DBNull.Value;
                    else
                    {
                        if (uppercase)
                        {
                            // Ensure value is string before calling ToUpper()
                            row[field.Key] = value is string strValue ? strValue.ToUpper() : DBNull.Value;
                        }
                        else
                        {
                            row[field.Key] = value;
                        }
                    }


                    i++;
                }


                collectionDataTable.Rows.Add(row);
            }

            return collectionDataTable;
        }

        private async Task InsertRowsIntoTempTable(
    IDbConnection connection,
    IDbTransaction? transaction,
    string tempTableName,
    DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                var columnNames = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
                var paramNames = columnNames.Select(c => "@" + c).ToList();

                string insertSql = $@"
            INSERT INTO {tempTableName} ({string.Join(", ", columnNames)})
            VALUES ({string.Join(", ", paramNames)});
        ";

                var parameters = new List<IDataParameter>();

                foreach (DataColumn col in table.Columns)
                {
                    object value = row[col] == DBNull.Value ? DBNull.Value : row[col];
                    SqlDbType sqlType = Utility.ToSqlDbType(col.DataType);

                    parameters.Add(
                        _dbUtility.CreateSqlParameter("@" + col.ColumnName, sqlType, value)
                    );
                }

                if (transaction != null)
                    await _dbUtility.ExecuteNonQuery(transaction, insertSql, parameters.ToArray());
                else
                    await _dbUtility.ExecuteNonQuery(connection, insertSql, parameters.ToArray());
            }
        }


        private Dictionary<string, IDataParameter> GetSingleItemAsParameters(T item)
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            Dictionary<string, IDataParameter> parameters = [];

            foreach (KeyValuePair<string, CollectionProperty> field in _collectionProperties)
            {
                PropertyInfo propertyInfo = field.Value.Info;
                bool uppercase = field.Value.Uppercase;

                string fieldName = field.Value.FieldName;
                string paramName = "@" + field.Value.FieldName.Replace(" ", string.Empty);
                SqlDbType dbType = field.Value.FieldType;

                object? rawValue = propertyInfo.GetValue(item);

                // ⭐ Normalize DateTime for PostgreSQL timestamp without time zone
                if (rawValue is DateTime dt)
                {
                    if (dt.Kind == DateTimeKind.Utc)
                        rawValue = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
                }

                // Handle archive table switching
                if (fieldName.Equals("IsArchiveRecord") && rawValue is bool isArchiveRecord && isArchiveRecord)
                {
                    _collection.TableName = _collection.ArchiveTableName;
                }

                // Apply uppercase rule
                if (uppercase && rawValue is string s)
                {
                    rawValue = s.ToUpperInvariant();
                }

                // Create parameter
                IDataParameter parameter = _dbUtility.CreateSqlParameter(
                    paramName,
                    dbType,
                    rawValue ?? DBNull.Value
                );

                parameters.Add(fieldName, parameter);
            }

            return parameters;
        }


        private async Task InsertSingleItem(IDbConnection connection, IDbTransaction transaction, T item, string partialQuery = "", List<IDataParameter>? partialQueryParameters = null)
        {
            await ExecuteSingleItemAction(connection, transaction, item, true, false, false, partialQuery, partialQueryParameters);
        }
        private async Task UpdateSingleItem(IDbConnection connection, IDbTransaction transaction, T item, string partialQuery = "", List<IDataParameter>? partialQueryParameters = null)
        {
            await ExecuteSingleItemAction(connection, transaction, item, false, true, false, partialQuery, partialQueryParameters);
        }
        private async Task DeleteSingleItem(IDbConnection connection, IDbTransaction transaction, T item, string partialQuery = "", List<IDataParameter>? partialQueryParameters = null)
        {
            await ExecuteSingleItemAction(connection, transaction, item, false, false, true, partialQuery, partialQueryParameters);
        }

        #endregion

        #region Public

        public async Task LoadCollection()
        {
            Debug.Assert(_collection != null);

            using IDbConnection dbConnection = await _dbUtility.GetConnection();
            await LoadCollection(dbConnection, null!);
        }

        public async Task LoadCollection(IDbConnection connection, IDbTransaction transaction, bool useReadUncommitted = false)
        {
            string query = GenerateSelectQuery(useReadUncommitted);

            List<String> whereConditions = [];
            List<IDataParameter> whereParameters = [];
            GetAdditionalWhereCondition(whereConditions, whereParameters);

            await Load(connection, transaction, query, whereConditions, whereParameters);
        }

        public async Task SaveCollection()
        {
            Debug.Assert(_collection != null);

            using IDbConnection dbConnection = await _dbUtility.GetConnection();
            await Save(dbConnection, null);
        }

        public async Task SaveCollection(IDbConnection dbConnection, DbTransaction sqlTransaction)
        {
            Debug.Assert(dbConnection != null);
            Debug.Assert(sqlTransaction != null);

            await Save(dbConnection, sqlTransaction);
        }

        public async Task InsertSingleItem(T item)
        {
            Debug.Assert(_collection != null);

            using IDbConnection dbConnection = await _dbUtility.GetConnection();
            await InsertSingleItem(dbConnection, null, item);
        }

        public async Task InsertSingleItem(IDbConnection dbConnection, IDbTransaction? transaction, T item)
        {
            Debug.Assert(_collection != null);

            // Pass 'transaction ?? null' to satisfy the non-nullable parameter requirement
            await InsertSingleItem(dbConnection, transaction ?? null!, item, string.Empty, null);
        }

        public async Task UpdateSingleItem(T item)
        {
            Debug.Assert(_collection != null);

            using IDbConnection dbConnection = await _dbUtility.GetConnection();
            await UpdateSingleItem(dbConnection, null, item);
        }

        public async Task UpdateSingleItem(IDbConnection dbConnection, IDbTransaction? transaction, T item)
        {
            bool createdTransaction = false;

            if (transaction == null)
            {
                transaction = dbConnection.BeginTransaction();
                createdTransaction = true;
            }

            await ExecuteUpdate(dbConnection, transaction, item, string.Empty, null);

            if (createdTransaction)
                transaction.Commit();
        }

        private async Task ExecuteUpdate(
            IDbConnection dbConnection,
            IDbTransaction transaction,
            T item,
            string partialQuery,
            List<IDataParameter>? partialParams)
        {
            // ⭐ Call the internal update logic, NOT the public wrapper
            await UpdateSingleItemInternal(dbConnection, transaction, item, partialQuery, partialParams);
        }

        // This is your existing internal update method (renamed)
        private async Task UpdateSingleItemInternal(
            IDbConnection dbConnection,
            IDbTransaction transaction,
            T item,
            string partialQuery,
            List<IDataParameter>? partialParams)
        {
            // Your existing SQL-building + ExecuteSingleItemAction logic
            await UpdateSingleItem(dbConnection, transaction, item, partialQuery, partialParams);
        }

        public async Task DeleteSingleItem(T item)
        {
            Debug.Assert(_collection != null);

            using IDbConnection dbConnection = await _dbUtility.GetConnection();
            await DeleteSingleItem(dbConnection, null!, item);
        }

        public async Task DeleteSingleItem(IDbConnection dbConnection, IDbTransaction transaction, T item)
        {
            Debug.Assert(_collection != null);

            await DeleteSingleItem(dbConnection, transaction, item, string.Empty, null);
        }

        public void Clear()
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            _collectionProperties.Clear();
            _collection.Clear();
        }

        public DataCollection<T> GetCollection()
        {
            if (_collection == null)
                throw new InvalidOperationException("_collection is not set.");

            return this._collection;
        }



        #endregion
    }
}
