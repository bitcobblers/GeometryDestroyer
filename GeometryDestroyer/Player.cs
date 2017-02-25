using System;
using GeometryDestroyer.Parts;
using GeometryDestroyer.Parts.Impl.Guns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer
{
    /// <summary>
    /// Defines a game player.
    /// </summary>
    public class Player : BoundedObject, ICanDie, ICanBeKilled, IDisposable
    {
        private readonly IMathSystem mathSystem;
        private readonly IGunSystem gunSystem;
        private readonly IDirectorComponent directorSystem;
        private readonly IParticleComponent particleComponent;
        private readonly Random rnd = new Random();
        private Gun currentGun;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="controller">The controller to bind to the player.</param>
        /// <param name="model">The model of the player to render.</param>
        public Player(GameController controller, Model model)
            : base(model)
        {
            this.Controller = controller;
            this.mathSystem = ServiceLocator.Get<IMathSystem>();
            this.gunSystem = ServiceLocator.Get<IGunSystem>();
            this.directorSystem = ServiceLocator.Get<IDirectorComponent>();
            this.particleComponent = ServiceLocator.Get<IParticleComponent>();

            this.currentGun = this.gunSystem.GetGun(this);
            this.directorSystem.LevelIncreased += DirectorSystem_LevelIncreased;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Player"/> class.
        /// </summary>
        ~Player()
        {
            this.Dispose(disposing: false);
        }

        /// <summary>
        /// Gets the id of the player.
        /// </summary>
        public PlayerIndex Id => this.Controller.Id;

        /// <summary>
        /// Gets the controller bound to the player.
        /// </summary>
        public GameController Controller { get; }

        /// <summary>
        /// Gets or sets score of the player.
        /// </summary>
        public int Score { get; set; }

        /// <inheritdoc />
        public bool IsAlive { get; private set; } = true;

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var state = this.Controller.State;
            var shootX = state.ThumbSticks.Right.X;
            var shootY = state.ThumbSticks.Right.Y;
            var rotateX = state.ThumbSticks.Left.X;
            var rotateY = state.ThumbSticks.Left.Y;

            // Move the player.
            this.Position += this.CalibrateMovement(this.CameraSystem.Boundary, new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, 0));

            // Rotate the player.
            if (rotateX != 0 || rotateY != 0)
            {
                this.Rotation = this.mathSystem.AngleOf(rotateX, rotateY);
            }

            // Create any new projectils.
            if (shootX != 0 || shootY != 0)
            {
                this.currentGun.Shoot(new Vector2(this.Position.X, this.Position.Y), new Vector2(shootX, shootY));
            }

            // Update collision detection.
            this.World = Matrix.CreateRotationZ(this.Rotation) * Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0.0f);
            this.RecalculateSpheres();
        }

        /// <inheritdoc />
        public void Kill()
        {
            this.IsAlive = false;
            this.particleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.White, ExplosionSize.Large);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes resources held by the instance.
        /// </summary>
        /// <param name="disposing">True if the dispose method was called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.directorSystem.LevelIncreased -= this.DirectorSystem_LevelIncreased;
            }
        }

        private void DirectorSystem_LevelIncreased(object sender, EventArgs e)
        {
            this.currentGun = this.gunSystem.GetGun(this);
        }
    }
}