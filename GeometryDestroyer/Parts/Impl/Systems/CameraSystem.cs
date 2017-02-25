using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    public class CameraSystem : ICameraSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraSystem" /> class.
        /// </summary>
        /// <param name="graphicsManager">The graphics manager to use.</param>
        /// <param name="boundarySize">The boundary size for the viewport.</param>
        public CameraSystem(GraphicsDeviceManager graphicsManager, int boundarySize)
        {
            this.Width = graphicsManager.PreferredBackBufferWidth;
            this.Height = graphicsManager.PreferredBackBufferHeight;
            this.View = Matrix.CreateLookAt(new Vector3(0, 0, boundarySize), Vector3.Zero, Vector3.Up);
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), this.Width / this.Height, 0.01f, 100.0f);
            this.Boundary = new Rectangle(-boundarySize, -boundarySize, boundarySize * 2, boundarySize * 2);
        }

        /// <inheritdoc />
        public int Width { get; }

        /// <inheritdoc />
        public int Height { get; }

        /// <inheritdoc />
        public Rectangle Boundary { get; }

        /// <inheritdoc />
        public Matrix Projection { get; }

        /// <inheritdoc />
        public Matrix View { get; }
    }
}