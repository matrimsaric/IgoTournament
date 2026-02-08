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
    // TEAM MEMBERSHIP
    // ============================================================
    [Table("team_membership")]
    [HasModifiedDate(true)]
    public class TeamMembership : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("player_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid PlayerId { get; set; }

        [FieldName("team_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid TeamId { get; set; }

        [FieldName("season"), FieldType(SqlDbType.NVarChar)]
        public string Season { get; set; } = String.Empty;

        [FieldName("role"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Role { get; set; } = String.Empty;

        public override IModelEntry Clone()
        {
            return new TeamMembership
            {
                Id = this.Id,
                Name = this.Name,
                PlayerId = this.PlayerId,
                TeamId = this.TeamId,
                Season = this.Season,
                Role = this.Role
            };
        }
    }
}