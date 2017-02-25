using System;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    /// <summary>
    /// Defines the director system which coordinates when enemies get spawned.
    /// </summary>
    public interface IDirectorComponent : IGameComponent
    {
        /// <summary>
        /// Triggered whenever the level is incremented.
        /// </summary>
        event EventHandler LevelIncreased;
    }
}