using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TableAttribute = ServerCommonModule.Attributes.TableAttribute;

namespace PlayerDomain.Model
{
    // ============================================================
    // PLAYERS
    // ============================================================
    [Table("players")]
    [HasModifiedDate(true)]
    public class Player : ModelEntry, IComparable<Player>
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("name_jp"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string NameJp { get; set; } = String.Empty;

        [FieldName("rank"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Rank { get; set; } = String.Empty;


        [FieldName("birth_year"), FieldType(SqlDbType.Int), FieldIsNullable(true)]
        public int? BirthYear { get; set; }

        [FieldName("affiliation"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Affiliation { get; set; } = String.Empty;


        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = String.Empty;

        public override IModelEntry Clone()
        {
            return new Player
            {
                Id = this.Id,
                Name = this.Name,
                NameJp = this.NameJp,
                Rank = this.Rank,
                BirthYear = this.BirthYear,
                Affiliation = this.Affiliation,
                Notes = this.Notes
            };
        }

        public int CompareTo(Player? other)
        {
            if (other == null) return 1;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

    }
}