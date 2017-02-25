using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    public interface IProjectileComponent : IGameComponent
    {
        /// <summary>
        /// Adds a projectile to the system.
        /// </summary>
        /// <param name="owner">The owner of the projectile.</param>
        /// <param name="position">The position of the projectile.</param>
        /// <param name="direction">The direction of the projectile.</param>
        void AddProjectile(Player owner, Vector3 position, Vector2 direction);
    }
}