using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PlayerDomain.Model
{
    [Table("teams")]
    [HasModifiedDate(true)]
    public class Team : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("name_jp"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string NameJp { get; set; } = String.Empty;

        [FieldName("colour_primary"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string ColourPrimary { get; set; } = String.Empty;

        [FieldName("colour_secondary"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string ColourSecondary { get; set; } = String.Empty;

        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = String.Empty;

        public override IModelEntry Clone()
        {
            return new Team
            {
                Id = this.Id,
                Name = this.Name,
                NameJp = this.NameJp,
                ColourPrimary = this.ColourPrimary,
                ColourSecondary = this.ColourSecondary,
                Notes = this.Notes
            };
        }
    }
}