using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System;
using System.Data;

namespace JosekiDomain.Model
{
    // ============================================================
    // JOSEKI BOOKS
    // ============================================================
    [Table("joseki_books")]
    [HasModifiedDate(true)]
    public class JosekiBook : ModelEntry
    {
        [FieldName("title"), FieldType(SqlDbType.NVarChar)]
        public string Title { get; set; } = string.Empty;

        [FieldName("author"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Author { get; set; } = string.Empty;

        [FieldName("publisher"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Publisher { get; set; } = string.Empty;

        [FieldName("year"), FieldType(SqlDbType.Int), FieldIsNullable(true)]
        public int? Year { get; set; }

        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = string.Empty;

        public override IModelEntry Clone()
        {
            return new JosekiBook
            {
                Id = this.Id,
                Title = this.Title,
                Author = this.Author,
                Publisher = this.Publisher,
                Year = this.Year,
                Notes = this.Notes
            };
        }
    }
}
