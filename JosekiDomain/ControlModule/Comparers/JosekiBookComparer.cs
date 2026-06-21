using JosekiDomain.Model;
using System.Collections.Generic;

namespace JosekiDomain.ControlModule.Comparers
{
    public class JosekiBookComparer : IComparer<JosekiBook>
    {
        public int Compare(JosekiBook? x, JosekiBook? y)
        {
            if (x == null || y == null)
                return 0;

            return string.Compare(x.Title, y.Title, true);
        }
    }
}
