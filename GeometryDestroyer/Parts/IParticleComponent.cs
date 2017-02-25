using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    /// <summary>
    /// Defines a system for creating particle effects.
    /// </summary>
    public interface IParticleComponent : IGameComponent
    {
        int Count { get; }

        /// <summary>
        /// Creates an explosion type particle effect.
        /// </summary>
        /// <param name="description">A description for the particles to create.</param>
        /// <param name="position">The position to create the effect from.</param>
        /// <param name="color">The color to apply to the particles.</param>
        /// <param name="size">The size of the explosion.</param>
        void AddExplosion(EmitterDescription description, Vector3 position, Color color, ExplosionSize size);
    }
}