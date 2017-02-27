using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    public interface IPlayerComponent : IGameComponent
    {
        /// <summary>
        /// Gets the total number of available players (i.e. Players that have not been eliminated from the game completely).
        /// </summary>
        int AvailablePlayers { get; }

        /// <summary>
        /// Gets the players in the system.
        /// </summary>
        IEnumerable<Player> Players { get; }

        /// <summary>
        /// Gets a collection of players that are active.
        /// </summary>
        IEnumerable<Player> ActivePlayers { get; }
    }
}