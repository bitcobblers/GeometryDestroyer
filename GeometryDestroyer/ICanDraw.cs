using Microsoft.Xna.Framework;

namespace GeometryHolocaust
{
    /// <summary>
    /// Defines an object that can be drawn.
    /// </summary>
    public interface ICanDraw
    {
        /// <summary>
        /// Draws the object.
        /// </summary>
        void Draw(IGameEngine engine, GameTime gameTime);
    }
}