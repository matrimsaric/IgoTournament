using CompetitionDomain.Model;
using StoneLedger.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Services.Api.Interfaces
{
    public interface ITournamentService
    {
        Task<List<Tournament>> GetAllTournamentsAsync();
    }

}
