using Application.Models.Tournament;
using CompetitionDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.Workflows
{
    public interface ITournamentWorkflow
    {
        Task<Tournament> CreateTournamentAsync(CreateTournamentModel model);
        Task<Tournament?> GetTournamentAsync(Guid id);
        Task<IEnumerable<Tournament>> GetAllTournamentsAsync();
        Task<Tournament> UpdateTournamentAsync(Guid id, UpdateTournamentModel model);
        Task DeleteTournamentAsync(Guid id);
    }

}
