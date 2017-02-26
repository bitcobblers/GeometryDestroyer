using System.Linq;
using GeometryDestroyer.Parts;
using GeometryDestroyer.Parts.Impl.Components;
using GeometryDestroyer.Parts.Impl.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GeometryDestroyer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private RenderTarget2D screenTarget;
        private SpriteBatch spriteBatch;
        private Effect pauseBackgroundEffect;

        private IGameSystem gameSystem;
        private IControllerSystem controllerSystem;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = this.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = this.GraphicsDevice.DisplayMode.Height;
            this.graphics.ApplyChanges();
            this.graphics.ToggleFullScreen();

            // Register components.
            this.Components.Add(new BackgroundComponent(this));
            this.Components.Add(new OverlayComponent(this));
            this.Components.Add(new PlayerComponent(this));
            this.Components.Add(new EnemyComponent(this));
            this.Components.Add(new ParticleComponent(this));
            this.Components.Add(new DirectorComponent(this));
            this.Components.Add(new ProjectileComponent(this));

            // Register services.
            ServiceLocator.Register<IGameSystem>(new GameSystem());
            ServiceLocator.Register<IMathSystem>(new MathSystem());
            ServiceLocator.Register<IListSystem>(new ListSystem());
            ServiceLocator.Register<ISpawnSystem>(new SpawnSystem(this.Content));
            ServiceLocator.Register<ICameraSystem>(new CameraSystem(this.graphics, 50));
            ServiceLocator.Register<IGunSystem>(new GunSystem());
            ServiceLocator.Register<IControllerSystem>(new ControllerSystem());

            base.Initialize();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            this.gameSystem = ServiceLocator.Get<IGameSystem>();
            this.controllerSystem = ServiceLocator.Get<IControllerSystem>();

            // Create a new SpriteBatch, which can be used to draw textures.
            this.screenTarget = new RenderTarget2D(this.GraphicsDevice, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight, false, this.graphics.PreferredBackBufferFormat, DepthFormat.Depth24);
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.pauseBackgroundEffect = this.Content.Load<Effect>("Effects/PauseBackground");
        }

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            switch (this.gameSystem.State)
            {
                case GameState.NotStarted:
                case GameState.GameOver:
                    if (this.controllerSystem.GetControllers(forceUpdate: true).Any(c => c.State.IsButtonDown(Buttons.Start)))
                    {
                        this.gameSystem.Reset();
                    }
                    break;
                case GameState.Running:
                case GameState.Paused:
                    if (this.controllerSystem.GetControllers(forceUpdate: true).Any(c => c.IsKeyPressed(Buttons.Start)))
                    {
                        this.gameSystem.TogglePaused();
                    }

                    break;
            }

            base.Update(gameTime);
        }

        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.screenTarget);

            // Let the sub-systems draw.
            base.Draw(gameTime);

            // Reset the target.
            this.GraphicsDevice.SetRenderTarget(null);

            // Render the target onto the back buffer.
            if (this.gameSystem.State == GameState.Paused)
            {
                this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, this.pauseBackgroundEffect);
            }
            else
            {
                this.spriteBatch.Begin();
            }

            this.spriteBatch.Draw(this.screenTarget, new Rectangle(0, 0, this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight), Color.White);
            this.spriteBatch.End();
        }
    }
}