using System.Collections.Generic;

namespace GeometryDestroyer.Parts
{
    /// <summary>
    /// Defines the sub-system that enumerates connected controllers.
    /// </summary>
    public interface IControllerSystem
    {
        /// <summary>
        /// Gets a collection of connected controllers.
        /// </summary>
        /// <returns>A collection of connected controllers.</returns>
        IEnumerable<GameController> GetControllers();

        /// <summary>
        /// Gets a collection of connected controllers.
        /// </summary>
        /// <param name="forceUpdate">True to force an update on any found controllers.</param>
        /// <returns>A collection of connected controllers.</returns>
        IEnumerable<GameController> GetControllers(bool forceUpdate);
    }
}