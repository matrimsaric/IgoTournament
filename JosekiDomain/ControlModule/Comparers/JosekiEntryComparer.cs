using JosekiDomain.Model;
using System.Collections.Generic;

namespace JosekiDomain.ControlModule.Comparers
{
    public class JosekiEntryComparer : IComparer<JosekiEntry>
    {
        public int Compare(JosekiEntry? x, JosekiEntry? y)
        {
            if (x == null || y == null)
                return 0;

            // Sort by category, then subcategory, then name
            int c = x.Category.CompareTo(y.Category);
            if (c != 0) return c;

            c = (x.SubCategory ?? 0).CompareTo(y.SubCategory ?? 0);
            if (c != 0) return c;

            return string.Compare(x.Name, y.Name, true);
        }
    }
}
