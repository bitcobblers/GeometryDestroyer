using Microsoft.Xna.Framework;

namespace GeometryHolocaust
{
    public interface ICanMove
    {
        /// <summary>
        /// Moves the object.
        /// </summary>
        /// <param name="engine">The game engine.</param>
        /// <param name="gameTime">The time in the game.</param>
        void Move(IGameEngine engine, GameTime gameTime);
    }
}