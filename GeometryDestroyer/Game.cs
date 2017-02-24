using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GeometryHolocaust
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly Dictionary<PlayerIndex, GameController> connectedControllers = new Dictionary<PlayerIndex, GameController>();

        private GraphicsDeviceManager graphics;
        private RenderTarget2D screenTarget;
        private SpriteBatch spriteBatch;
        private Texture2D background;
        private Effect pauseBackgroundEffect;
        private GameEngine engine;

        private SpriteFont titleFont;
        private SpriteFont overlayFont;


        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            //this.graphics.PreferredBackBufferWidth = this.GraphicsDevice.DisplayMode.Width;
            //this.graphics.PreferredBackBufferHeight = this.GraphicsDevice.DisplayMode.Height;
            //this.graphics.ApplyChanges();
            //this.graphics.ToggleFullScreen();

            base.Initialize();
            this.engine = new GameEngine(this.graphics, this.Content);
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.screenTarget = new RenderTarget2D(this.GraphicsDevice, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight, false, this.graphics.PreferredBackBufferFormat, DepthFormat.Depth24);
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.background = this.Content.Load<Texture2D>("Images/background2");
            this.pauseBackgroundEffect = this.Content.Load<Effect>("Effects/PauseBackground");

            // Load the fonts.
            this.titleFont = this.Content.Load<SpriteFont>("Fonts/Title");
            this.overlayFont = this.Content.Load<SpriteFont>("Fonts/Overlay");
        }

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            switch (this.engine.State)
            {
                case GameState.NotStarted:
                case GameState.GameOver:
                    this.UpdateControllerList();

                    if (this.connectedControllers.Values.Any(c => c.State.IsButtonDown(Buttons.Start)))
                    {
                        this.engine.Reset(this.connectedControllers.Values);
                    }

                    break;
                case GameState.Running:
                case GameState.Paused:
                    if (this.engine.Controllers.Any(c => c.IsKeyPressed(Buttons.Start)))
                    {
                        this.engine.TogglePaused();
                    }

                    this.engine.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            // Draw the screen to the target first.
            this.GraphicsDevice.SetRenderTarget(this.screenTarget);
            this.GraphicsDevice.Clear(Color.White);

            // Draw the sprites.
            this.spriteBatch.Begin();
            this.DrawBackground();
            this.spriteBatch.End();

            // Reset these after rendering the sprite batch.
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.engine.Draw(gameTime);

            // Reset the target.
            this.GraphicsDevice.SetRenderTarget(null);

            // Render the target onto the back buffer.
            if (this.engine.State == GameState.Paused)
            {
                this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, this.pauseBackgroundEffect);
            }
            else
            {
                this.spriteBatch.Begin();
            }

            this.spriteBatch.Draw(this.screenTarget, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), Color.White);
            this.spriteBatch.End();

            // Draw the overlay directy to the backbuffer.
            this.spriteBatch.Begin();
            this.DrawOverlay();
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws the background for the display.
        /// </summary>
        private void DrawBackground()
        {
            this.spriteBatch.Draw(this.background, this.GraphicsDevice.Viewport.Bounds, Color.White);
        }

        /// <summary>
        /// Draws the overlay for the screen.
        /// </summary>
        private void DrawOverlay()
        {
            switch (this.engine.State)
            {
                case GameState.NotStarted:
                    this.DrawCenteredText(this.titleFont, "Geometry Holocaust", 0);
                    this.DrawCenteredText(this.overlayFont, "Press Start to Begin", 50);
                    break;
                case GameState.Running:
                case GameState.Paused:
                    {
                        float x = 10;
                        float y = 10;

                        // Render the players and their scores.
                        foreach (var player in this.engine.Players)
                        {
                            var left = player.Controller.State.ThumbSticks.Left;
                            var right = player.Controller.State.ThumbSticks.Right;

                            this.spriteBatch.DrawString(this.overlayFont, $"Player {player.Id}: {player.Score:N0}", new Vector2(x, y), Color.White);
                            y += 20;
                        }
                    }

                    if (this.engine.State == GameState.Paused)
                    {
                        this.DrawCenteredText(this.overlayFont, "Paused", 0);
                    }

                    break;
                case GameState.GameOver:
                    this.DrawCenteredText(this.titleFont, "GAME OVER", 0);
                    this.DrawCenteredText(this.overlayFont, "Press Start to Play Again", 50);
                    break;
            }
        }

        /// <summary>
        /// Draws centered text to the screen.
        /// </summary>
        /// <param name="font">The font of the text to draw.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="yOffset">The Y offset on the screen to draw the text.</param>
        private void DrawCenteredText(SpriteFont font, string text, float yOffset)
        {
            float width = this.graphics.PreferredBackBufferWidth / 2;
            float height = this.graphics.PreferredBackBufferHeight / 2;

            var size = font.MeasureString(text);
            var position = new Vector2(width - (size.X / 2), height - (size.Y / 2) + yOffset);

            this.spriteBatch.DrawString(font, text, position, Color.SteelBlue);
        }

        /// <summary>
        /// Updates the controller list; adding or removing controllers dynamically.
        /// </summary>
        private void UpdateControllerList()
        {
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                var capabilities = GamePad.GetState(i);

                if (capabilities.IsConnected)
                {
                    if (this.connectedControllers.ContainsKey(i) == false)
                    {
                        this.connectedControllers.Add(i, new GameController(i));
                    }
                }
                else
                {
                    if (this.connectedControllers.ContainsKey(i))
                    {
                        this.connectedControllers.Remove(i);
                    }
                }
            }

            foreach (var controller in this.connectedControllers.Values)
            {
                controller.Update();
            }
        }

        /// <summary>
        /// Calculates the angle of a normalized vector.
        /// </summary>
        /// <param name="x">The x coordinate of the vector.</param>
        /// <param name="y">The y coordinate of the vector.</param>
        /// <returns>The angle of the vector.</returns>
        public static float AngleOf(float x, float y)
        {
            if (x == 1 && y == 0)
            {
                return 0.0f;
            }
            else if (x == 0 && y == 1)
            {
                return (float)(Math.PI / 2);
            }
            else if (x == -1 && y == 0)
            {
                return (float)Math.PI;
            }
            else if (x == 0 && y == -1)
            {
                return (float)(3 * Math.PI / 2);
            }
            else
            {
                var tan = y / x;

                if (x < 0)
                {
                    return (float)(Math.Atan(tan) + Math.PI);
                }
                else
                {
                    return (float)Math.Atan(tan);
                }
            }
        }
    }
}