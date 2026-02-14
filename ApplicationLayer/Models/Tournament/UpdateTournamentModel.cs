namespace Application.Models.Tournament
{
    public sealed class UpdateTournamentModel
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public DateOnly? StartDate { get; init; }
        public DateOnly? EndDate { get; init; }
        public string? Location { get; init; }
    }
}
