using Microsoft.Xna.Framework;
using GeometryDestroyer.Parts.Impl.Enemies;

namespace GeometryDestroyer.Parts
{
    /// <summary>
    /// Defines a system for spawning enemies.
    /// </summary>
    public interface ISpawnSystem
    {
        /// <summary>
        /// Spawns an enemy.
        /// </summary>
        /// <param name="type">The type of enemy to spawn.</param>
        /// <param name="position">The position to spawn the enemy in.</param>
        void Spawn(EnemyType type, Vector3 position);

        /// <summary>
        /// Spawns a random enemy.
        /// </summary>
        /// <param name="filter">A filter of enemy types to spawn.</param>
        /// <param name="position">The position to spawn the enemy in.</param>
        void Random(EnemyType filter, Vector3 position);
    }
}