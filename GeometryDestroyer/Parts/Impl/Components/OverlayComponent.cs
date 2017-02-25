﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Components
{
    public class OverlayComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont titleFont;
        private SpriteFont overlayFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayComponent"/> class.
        /// </summary>
        /// <param name="game">The game object.</param>
        public OverlayComponent(Game game) : base(game)
        {
        }

        /// <summary>
        /// Gets or sets the game system to use.
        /// </summary>
        public IGameSystem GameSystem { get; private set; }

        /// <summary>
        /// Gets or sets the camera system to use.
        /// </summary>
        public ICameraSystem CameraSystem { get; private set; }

        /// <summary>
        /// Gets or sets the player system to use.
        /// </summary>
        public IPlayerComponent PlayerSystem { get; private set; }

        private IParticleComponent particles;

        /// <inheritdoc />
        public override void Initialize()
        {
            this.GameSystem = ServiceLocator.Get<IGameSystem>();
            this.CameraSystem = ServiceLocator.Get<ICameraSystem>();
            this.PlayerSystem = ServiceLocator.Get<IPlayerComponent>();
            this.particles = ServiceLocator.Get<IParticleComponent>();

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.titleFont = this.Game.Content.Load<SpriteFont>("Fonts/Title");
            this.overlayFont = this.Game.Content.Load<SpriteFont>("Fonts/Overlay");
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            switch (this.GameSystem.State)
            {
                case GameState.NotStarted:
                    this.DrawCenteredText(this.titleFont, "Geometry Destroyer", 0);
                    this.DrawCenteredText(this.overlayFont, "Press Start to Begin", 50);
                    break;
                case GameState.Running:
                case GameState.Paused:
                    {
                        float x = 10;
                        float y = 10;

                        // Render the players and their scores.
                        foreach (var player in this.PlayerSystem.Players)
                        {
                            var left = player.Controller.State.ThumbSticks.Left;
                            var right = player.Controller.State.ThumbSticks.Right;

                            this.spriteBatch.DrawString(this.overlayFont, $"Player {player.Id}: {player.Score:N0}", new Vector2(x, y), Color.White);
                            y += 20;
                        }

                        this.spriteBatch.DrawString(this.overlayFont, $"Particles: {this.particles.Count:N0}", new Vector2(x, y), Color.White);
                    }

                    if (this.GameSystem.State == GameState.Paused)
                    {
                        this.DrawCenteredText(this.overlayFont, "Paused", 0);
                    }

                    break;
                case GameState.GameOver:
                    this.DrawCenteredText(this.titleFont, "GAME OVER", 0);
                    this.DrawCenteredText(this.overlayFont, "Press Start to Play Again", 50);
                    break;
            }

            this.spriteBatch.End();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            // Load the dependencies.
            this.GameSystem = ServiceLocator.Get<IGameSystem>();
            this.CameraSystem = ServiceLocator.Get<ICameraSystem>();
            this.PlayerSystem = ServiceLocator.Get<IPlayerComponent>();
        }

        /// <summary>
        /// Draws centered text to the screen.
        /// </summary>
        /// <param name="font">The font of the text to draw.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="yOffset">The Y offset on the screen to draw the text.</param>
        private void DrawCenteredText(SpriteFont font, string text, float yOffset)
        {
            float width = this.CameraSystem.Width / 2.0f;
            float height = this.CameraSystem.Height / 2.0f;

            var size = font.MeasureString(text);
            var position = new Vector2(width - (size.X / 2), height - (size.Y / 2) + yOffset);

            this.spriteBatch.DrawString(font, text, position, Color.SteelBlue);
        }
    }
}