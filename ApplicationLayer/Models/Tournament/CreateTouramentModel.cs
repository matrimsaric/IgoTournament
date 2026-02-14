using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.Tournament
{
    public sealed class CreateTournamentModel
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public DateOnly? StartDate { get; init; }
        public DateOnly? EndDate { get; init; }
        public string? Location { get; init; }
    }
}
