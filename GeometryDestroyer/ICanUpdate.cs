using Microsoft.Xna.Framework;

namespace GeometryHolocaust
{
    /// <summary>
    /// Defines an object that needs to be updated each frame.
    /// </summary>
    public interface ICanUpdate
    {
        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="engine">The engine for the game.</param>
        /// <param name="gameTime">The time in the game.</param>
        void Update(IGameEngine engine, GameTime gameTime);
    }
}