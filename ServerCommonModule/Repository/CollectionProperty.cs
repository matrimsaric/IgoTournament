using ServerCommonModule.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerCommonModule.Repository
{
    public class CollectionProperty
    {
        public bool IsNullable { get; }
        public bool IsPrimaryKey { get; private set; }
        public SqlDbType FieldType { get; }
        public bool Uppercase { get; }
        public bool Identity { get; }
        public bool IsJson { get; }
        public string FieldName { get; private set; }
        public PropertyInfo Info { get; }
        public string DisplayName { get; private set; }

        public bool IsPublicNameProperty { get; set; }

        public CollectionProperty(PropertyInfo property, string fieldName)
        {
            FieldName = fieldName;

            Info = property;

            if (property.GetCustomAttribute<FieldTypeAttribute>() is FieldTypeAttribute fieldTypeAttribute)
                FieldType = fieldTypeAttribute.FieldType;
            else
                FieldType = SqlDbType.NVarChar;

            if (property.GetCustomAttribute<FieldIsNullableAttribute>() is FieldIsNullableAttribute isNullableAttribute)
                IsNullable = isNullableAttribute.IsNullable;

            if (property.GetCustomAttribute<FieldIsPrimaryKeyAttribute>() is FieldIsPrimaryKeyAttribute isPrimaryKeyAttribute)
                IsPrimaryKey = isPrimaryKeyAttribute.IsPrimaryKey;

            if (property.GetCustomAttribute<FieldIdentityAttribute>() is FieldIdentityAttribute identityAttribute)
                Identity = identityAttribute.IsIdentity;

            if (property.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttribute)
                DisplayName = displayNameAttribute.DisplayName;
            else
                DisplayName = property.Name;

            IsPublicNameProperty = string.Equals(DisplayName, "Name", System.StringComparison.InvariantCultureIgnoreCase);

        }

        public void DisablePrimaryKey()
        {
            IsPrimaryKey = false;
        }
    }
}
