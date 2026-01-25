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
    // SGF RECORDS
    // ============================================================
    [Table("sgf_records")]
    [HasModifiedDate(true)]
    public class SgfRecord : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("match_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid MatchId { get; set; }

        [FieldName("source_url"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string SourceUrl { get; set; } = String.Empty;

        [FieldName("raw_sgf"), FieldType(SqlDbType.NVarChar)]
        public string RawSgf { get; set; } = String.Empty;

        [FieldName("parsed_moves"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string ParsedMovesJson { get; set; } = String.Empty;

        [FieldName("published_at"), FieldType(SqlDbType.DateTime), FieldIsNullable(true)]
        public DateTime? PublishedAt { get; set; }

        [FieldName("retrieved_at"), FieldType(SqlDbType.DateTime), FieldIsNullable(true)]
        public DateTime? RetrievedAt { get; set; }

        public override IModelEntry Clone()
        {
            return new SgfRecord
            {
                Id = this.Id,
                Name = this.Name,
                MatchId = this.MatchId,
                SourceUrl = this.SourceUrl,
                RawSgf = this.RawSgf,
                ParsedMovesJson = this.ParsedMovesJson,
                PublishedAt = this.PublishedAt,
                RetrievedAt = this.RetrievedAt
            };
        }
    }
}