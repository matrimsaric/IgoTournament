using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Models
{
    public class RoundDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public DateTime? RoundDate { get; set; }
        public string? Notes { get; set; }
    }
}
