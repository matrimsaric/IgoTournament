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
    // ARCHIVE STATUS
    // ============================================================
    [Table("archive_status")]
    [HasModifiedDate(true)]
    public class ArchiveStatus : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("match_id"), FieldType(SqlDbType.UniqueIdentifier), FieldIsNullable(true)]
        public Guid? MatchId { get; set; }

        [FieldName("sgf_id"), FieldType(SqlDbType.UniqueIdentifier), FieldIsNullable(true)]
        public Guid? SgfId { get; set; }

        [FieldName("extracted"), FieldType(SqlDbType.Bit)]
        public bool Extracted { get; set; }

        [FieldName("formatted"), FieldType(SqlDbType.Bit)]
        public bool Formatted { get; set; }

        [FieldName("posted_to_blog"), FieldType(SqlDbType.Bit)]
        public bool PostedToBlog { get; set; }

        [FieldName("blog_url"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string BlogUrl { get; set; } = String.Empty;

        [FieldName("last_updated"), FieldType(SqlDbType.DateTime), FieldIsNullable(true)]
        public DateTime? LastUpdated { get; set; }

        public override IModelEntry Clone()
        {
            return new ArchiveStatus
            {
                Id = this.Id,
                Name = this.Name,
                MatchId = this.MatchId,
                SgfId = this.SgfId,
                Extracted = this.Extracted,
                Formatted = this.Formatted,
                PostedToBlog = this.PostedToBlog,
                BlogUrl = this.BlogUrl,
                LastUpdated = this.LastUpdated
            };
        }
    }
}