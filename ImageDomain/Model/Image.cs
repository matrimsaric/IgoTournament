using ServerCommonModule.Attributes;
using ServerCommonModule.Model;
using ServerCommonModule.Model.Interfaces;
using System;
using System.Data;

namespace ImageDomain.Model
{
    // ============================================================
    // IMAGES
    // ============================================================
    [Table("images")]
    [HasModifiedDate(true)]
    public class Image : ModelEntry, IComparable<Image>
    {
        [FieldName("object_id"), FieldType(SqlDbType.UniqueIdentifier)]
        public Guid ObjectId { get; set; }

        [FieldName("object_type"), FieldType(SqlDbType.Int)]
        public int ObjectType { get; set; }

        [FieldName("image_url"), FieldType(SqlDbType.NVarChar)]
        public string ImageUrl { get; set; } = string.Empty;

        [FieldName("size_type"), FieldType(SqlDbType.Int)]
        public int SizeType { get; set; }

        [FieldName("sort_order"), FieldType(SqlDbType.Int)]
        public int SortOrder { get; set; }

        [FieldName("notes"), FieldType(SqlDbType.NVarChar), FieldIsNullable(true)]
        public string Notes { get; set; } = string.Empty;

        public override IModelEntry Clone()
        {
            return new Image
            {
                Id = this.Id,
                ObjectId = this.ObjectId,
                ObjectType = this.ObjectType,
                ImageUrl = this.ImageUrl,
                SizeType = this.SizeType,
                SortOrder = this.SortOrder,
                Notes = this.Notes
            };
        }

        // Sorting is NOT meaningful for images, so we just use SortOrder
        public int CompareTo(Image? other)
        {
            if (other == null) return 1;
            return SortOrder.CompareTo(other.SortOrder);
        }
    }
}
