using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerDomain.ControlModule.Comparers
{
    public sealed class NameComparer<T> : IComparer<T> where T : ModelEntry
    {
        public int Compare(T? x, T? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            int nameCompare = string.Compare(
                x.Name ?? string.Empty,
                y.Name ?? string.Empty,
                StringComparison.OrdinalIgnoreCase
            );

            if (nameCompare != 0)
                return nameCompare;

            return x.Id.CompareTo(y.Id);
        }
    }
}