using ServerCommonModule.Model;

namespace ImageDomain.Model
{
    public class ImageCollection : ModelEntryCollection<Image>
    {
        // Images are NOT sorted by default
        public ImageCollection() : base(false, null) { }

        public override Image CreateItem() => new();
    }
}
