using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    public interface IPlayerComponent : IGameComponent
    {
        /// <summary>
        /// Gets the players in the system.
        /// </summary>
        IEnumerable<Player> Players { get; }
    }
}