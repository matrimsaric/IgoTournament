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
    // MATCHES
    // ============================================================
    [Table("matches")]
    [HasModifiedDate(true)]
    public class Match : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("round_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid RoundId { get; set; }

        [FieldName("board_number"), FieldType(SqlDbType.Int), FieldIsNullable(true)]
        public int BoardNumber { get; set; }

        [FieldName("black_player_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid BlackPlayerId { get; set; }

        [FieldName("white_player_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid WhitePlayerId { get; set; }

        [FieldName("result"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Result { get; set; } = String.Empty;

        [FieldName("winner_id"), FieldType(SqlDbType.UniqueIdentifier), FieldIsNullable(true)]
        public Guid? WinnerId { get; set; }

        [FieldName("sgf_id"), FieldType(SqlDbType.UniqueIdentifier), FieldIsNullable(true)]
        public Guid? SgfId { get; set; }

        [FieldName("game_date"), FieldType(SqlDbType.Date), FieldIsNullable(true)]
        public DateTime? GameDate { get; set; }

        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = String.Empty;

        public override IModelEntry Clone()
        {
            return new Match
            {
                Id = this.Id,
                Name = this.Name,
                RoundId = this.RoundId,
                BoardNumber = this.BoardNumber,
                BlackPlayerId = this.BlackPlayerId,
                WhitePlayerId = this.WhitePlayerId,
                Result = this.Result,
                WinnerId = this.WinnerId,
                SgfId = this.SgfId,
                GameDate = this.GameDate,
                Notes = this.Notes
            };
        }
    }
}