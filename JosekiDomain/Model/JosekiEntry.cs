using CommonModule.Enums;
using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using TableAttribute = ServerCommonModule.Attributes.TableAttribute;

namespace JosekiDomain.Model
{
    // ============================================================
    // JOSEKI ENTRIES
    // ============================================================
    [Table("joseki_entries")]
    [HasModifiedDate(true)]
    public class JosekiEntry : ModelEntry
    {
        [FieldName("name"), FieldType(SqlDbType.NVarChar)]
        public new string Name { get; set; } = string.Empty;

        [FieldName("category"), FieldType(SqlDbType.Int)]
        public int Category { get; set; }

        [FieldName("sub_category"), FieldType(SqlDbType.Int), FieldIsNullable(true)]
        public int SubCategory { get; set; }

        [FieldName("is_sente"), FieldType(SqlDbType.Bit)]
        public bool IsSente { get; set; }

        [FieldName("book_id"), FieldType(SqlDbType.UniqueIdentifier), FieldIsNullable(true)]
        public Guid? BookId { get; set; }

        [FieldName("description"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Description { get; set; } = string.Empty;

        // NEW: full move sequence (SGF, JSON, or token format)
        [FieldName("moves"), FieldType(SqlDbType.NVarChar)]
        public string Moves { get; set; } = string.Empty;

        [FieldName("variation_change_index"), FieldType(SqlDbType.Int), FieldIsNullable(true)]
        public int VariationChangeIndex { get; set; }

        [FieldName("variation_change_coord"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string VariationChangeCoord { get; set; } = string.Empty;

        [FieldName("parent_id"), FieldType(SqlDbType.UniqueIdentifier), FieldIsNullable(true)]
        public Guid? ParentId { get; set; }

        [FieldName("is_even_result"), FieldType(SqlDbType.Bit)]
        public bool IsEvenResult { get; set; }

        [FieldName("joseki_intent"), FieldType(SqlDbType.Int)]
        public int Intent { get; set; }

        public override IModelEntry Clone()
        {
            return new JosekiEntry
            {
                Id = this.Id,
                Name = this.Name,
                Category = this.Category,
                SubCategory = this.SubCategory,
                IsSente = this.IsSente,
                BookId = this.BookId,
                Description = this.Description,
                Moves = this.Moves,
                VariationChangeIndex = this.VariationChangeIndex,
                VariationChangeCoord = this.VariationChangeCoord,
                ParentId = this.ParentId,
                IsEvenResult = this.IsEvenResult,
                Intent = this.Intent,
            };
        }

    }
}
