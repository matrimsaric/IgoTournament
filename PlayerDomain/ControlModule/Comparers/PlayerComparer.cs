using PlayerDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerDomain.ControlModule.Comparers
{
    public sealed class PlayerComparer : IComparer<Player>
    {
        public int Compare(Player? x, Player? y)
        {
            // Handle nulls safely
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            // Primary sort: English name
            int nameCompare = string.Compare(
                x.Name ?? string.Empty,
                y.Name ?? string.Empty,
                StringComparison.OrdinalIgnoreCase
            );

            if (nameCompare != 0)
                return nameCompare;

            // Secondary sort: Japanese name
            int jpCompare = string.Compare(
                x.NameJp ?? string.Empty,
                y.NameJp ?? string.Empty,
                StringComparison.Ordinal
            );

            if (jpCompare != 0)
                return jpCompare;

            // Tertiary sort: Rank (as text)
            int rankCompare = string.Compare(
                x.Rank ?? string.Empty,
                y.Rank ?? string.Empty,
                StringComparison.Ordinal
            );

            if (rankCompare != 0)
                return rankCompare;

            // Final fallback: GUID (guarantees stable ordering)
            return x.Id.CompareTo(y.Id);
        }
    }
}