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
    public class Player : BoundedObject, ICanBeKilled, IDisposable
    {
        private readonly IMathSystem mathSystem;
        private readonly IGunSystem gunSystem;
        private readonly IDirectorComponent directorComponent;
        private readonly IParticleComponent particleComponent;
        private readonly IEnemyComponent enemyComponent;
        private readonly GameController controller;
        private readonly Random rnd = new Random();
        private Gun currentGun;

        /// <summary>
        /// Triggered when the player has been eliminated from the game.
        /// </summary>
        public event EventHandler PlayerEliminated = delegate { };

        /// <summary>
        /// Triggered whenever the player dies.
        /// </summary>
        public event EventHandler PlayerKilled = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="controller">The controller to bind to the player.</param>
        /// <param name="model">The model of the player to render.</param>
        public Player(GameController controller, Model model)
            : base(model)
        {
            this.controller = controller;
            this.mathSystem = ServiceLocator.Get<IMathSystem>();
            this.gunSystem = ServiceLocator.Get<IGunSystem>();
            this.directorComponent = ServiceLocator.Get<IDirectorComponent>();
            this.particleComponent = ServiceLocator.Get<IParticleComponent>();
            this.enemyComponent = ServiceLocator.Get<IEnemyComponent>();

            this.currentGun = this.gunSystem.GetGun(this);
            this.directorComponent.LevelIncreased += DirectorSystem_LevelIncreased;
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
        public PlayerIndex Id => this.controller.Id;

        /// <summary>
        /// Gets or sets score of the player.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the number of lives remaining for the player.
        /// </summary>
        public int LivesRemaining { get; set; } = 3;

        /// <summary>
        /// Gets the number of bumbs the player has.
        /// </summary>
        public int Bombs { get; private set; } = 3;

        /// <summary>
        /// Gets or sets a value indicating whether the player is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets the time when the player was last killed.
        /// </summary>
        public DateTime KillTime { get; private set; }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var state = this.controller.State;
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

            if (this.controller.IsTriggerPressed() && this.Bombs > 0)
            {
                this.enemyComponent.KillAll(this);
                this.Bombs--;
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
            this.KillTime = DateTime.Now;
            this.IsActive = false;
            this.LivesRemaining--;
            this.particleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.Red, ExplosionSize.Large);
            this.particleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.SteelBlue, ExplosionSize.Large);
            this.particleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.White, ExplosionSize.Huge);

            if (this.LivesRemaining < 0)
            {
                this.PlayerEliminated(this, EventArgs.Empty);
            }

            this.PlayerKilled(this, EventArgs.Empty);
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
                this.directorComponent.LevelIncreased -= this.DirectorSystem_LevelIncreased;
            }
        }

        private void DirectorSystem_LevelIncreased(object sender, EventArgs e)
        {
            this.currentGun = this.gunSystem.GetGun(this);
        }
    }
}