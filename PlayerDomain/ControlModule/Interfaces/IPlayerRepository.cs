using ImageDomain.Model;
using PlayerDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerDomain.ControlModule.Interfaces
{
        public interface IPlayerRepository
        {
            Task<PlayerCollection> GetAllPlayers(bool reload = true);
            Task<Player?> GetPlayerById(Guid id, bool reload = true);

            Task<string> CreatePlayer(Player newPlayer, bool reload = true);
            Task<string> UpdatePlayer(Player updatedPlayer, bool reload = true);
            Task<string> DeletePlayer(Player deletePlayer, bool reload = true);

            Task<Image?> GetPrimaryImageForPlayer(Guid playerId, bool reload = true);

    }
}
