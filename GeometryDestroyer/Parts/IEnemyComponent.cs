using System.Collections.Generic;
using GeometryDestroyer.Parts.Impl.Enemies;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    public interface IEnemyComponent : IGameComponent
    {
        /// <summary>
        /// Gets a collection of enemies in the system.
        /// </summary>
        IEnumerable<Enemy> Enemies { get; }

        /// <summary>
        /// Adds an enemy to the system.
        /// </summary>
        /// <param name="enemy">The enemy to add to the system.</param>
        void AddEnemy(Enemy enemy);
    }
}