using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    public interface ICameraSystem
    {
        /// <summary>
        /// Gets the width of the backbuffer.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the backbuffer.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the view matrix for the camera.
        /// </summary>
        Matrix View { get; }

        /// <summary>
        /// Gets the projection matrix for the camera.
        /// </summary>
        Matrix Projection { get; }

        /// <summary>
        /// Gets the boundary for the view port.
        /// </summary>
        Rectangle Boundary { get; }
    }
}