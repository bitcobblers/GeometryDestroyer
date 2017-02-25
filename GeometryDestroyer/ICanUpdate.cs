using Microsoft.Xna.Framework;
using GeometryDestroyer.Parts;

namespace GeometryDestroyer
{
    /// <summary>
    /// Defines an object that needs to be updated each frame.
    /// </summary>
    public interface ICanUpdate
    {
        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="gameTime">The time in the game.</param>
        void Update(GameTime gameTime);
    }
}