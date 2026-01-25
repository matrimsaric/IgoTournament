using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System.Data;

namespace CompetitionDomain.Model
{
    // ============================================================
    // TOURNAMENTS
    // ============================================================
    [Table("tournaments")]
    [HasModifiedDate(true)]
    public class Tournament : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("name_jp"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string NameJp { get; set; } = String.Empty;

        [FieldName("season"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Season { get; set; } = String.Empty;

        [FieldName("organiser"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Organiser { get; set; } = String.Empty;

        [FieldName("ruleset"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Ruleset { get; set; } = String.Empty;

        [FieldName("komi"), FieldType(SqlDbType.Decimal), FieldIsNullable(true)]
        public decimal? Komi { get; set; }

        [FieldName("time_settings"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string TimeSettings { get; set; } = String.Empty;

        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = String.Empty;

        public override IModelEntry Clone()
        {
            return new Tournament
            {
                Id = this.Id,
                Name = this.Name,
                NameJp = this.NameJp,
                Season = this.Season,
                Organiser = this.Organiser,
                Ruleset = this.Ruleset,
                Komi = this.Komi,
                TimeSettings = this.TimeSettings,
                Notes = this.Notes
            };
        }
    }
}