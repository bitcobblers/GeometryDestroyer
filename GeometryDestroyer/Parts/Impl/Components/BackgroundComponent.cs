using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Components
{
    /// <summary>
    /// Defines a component that is responsible for background rendering.
    /// </summary>
    public class BackgroundComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D background;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundComponent"/> class.
        /// </summary>
        /// <param name="game">The game object.</param>
        public BackgroundComponent(Game game) : base(game)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this.background = this.Game.Content.Load<Texture2D>("Images/background2");
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.background, this.GraphicsDevice.Viewport.Bounds, Color.White);
            this.spriteBatch.End();
        }
    }
}