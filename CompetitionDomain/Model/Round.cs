using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CompetitionDomain.Model
{
    // ============================================================
    // ROUNDS
    // ============================================================
    [Table("rounds")]
    [HasModifiedDate(true)]
    public class Round : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("tournament_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid TournamentId { get; set; }

        [FieldName("round_number"), FieldType(SqlDbType.Int)]
        public int RoundNumber { get; set; }

        [FieldName("round_date"), FieldType(SqlDbType.Date), FieldIsNullable(true)]
        public DateTime? RoundDate { get; set; }

        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = String.Empty;

        public override IModelEntry Clone()
        {
            return new Round
            {
                Id = this.Id,
                Name = this.Name,
                TournamentId = this.TournamentId,
                RoundNumber = this.RoundNumber,
                RoundDate = this.RoundDate,
                Notes = this.Notes
            };
        }
    }
}