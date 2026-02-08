using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System;
using System.Data;

namespace CompetitionDomain.Model
{
    [Table("teams")]
    [HasModifiedDate(true)]
    public class Team : ModelEntry, IComparable<Team>
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("name_jp"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string NameJp { get; set; } = string.Empty;

        [FieldName("colour_primary"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string ColourPrimary { get; set; } = string.Empty;

        [FieldName("colour_secondary"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string ColourSecondary { get; set; } = string.Empty;

        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = string.Empty;

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

        public int CompareTo(Team? other)
        {
            if (other == null) return 1;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}
