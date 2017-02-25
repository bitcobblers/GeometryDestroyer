using Microsoft.Xna.Framework;

namespace GeometryDestroyer
{
    /// <summary>
    /// Defines an object that can be drawn.
    /// </summary>
    public interface ICanDraw
    {
        /// <summary>
        /// Draws the object.
        /// </summary>
        /// <param name="gameSystem">The game system to use.</param>
        /// <param name="gameTime">The time in the game.</param>
        void Draw(GameTime gameTime);
    }
}