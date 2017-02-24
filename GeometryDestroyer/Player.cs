using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GeometryHolocaust
{
    /// <summary>
    /// Defines a game player.
    /// </summary>
    public class Player : BoundedObject, ICanUpdate, ICanDie
    {
        private readonly Random rnd = new Random();
        private readonly Gun[] allGuns;
        private Gun currentGun;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="controller">The controller to bind to the player.</param>
        /// <param name="model">The model of the player to render.</param>
        public Player(GameController controller, Model model)
            : base(model, 10.0f)
        {
            this.Controller = controller;
            this.currentGun = new Gun.Scattered(this);

            this.allGuns = new Gun[]
            {
                new Gun.Concentrated(this),
                new Gun.Scattered(this)
            };
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
        public bool IsAlive => true;

        /// <summary>
        /// Randomly changes the gun to one of the pre-loaded guns.
        /// </summary>
        public void ChangeGun()
        {
            this.currentGun = this.allGuns[(this.rnd.Next(0, this.allGuns.Length))];
        }

        /// <inheritdoc />
        public void Update(IGameEngine engine, GameTime gameTime)
        {
            var state = this.Controller.State;
            var shootX = state.ThumbSticks.Right.X;
            var shootY = state.ThumbSticks.Right.Y;

            // Create any new projectils.
            if (shootX != 0 || shootY != 0)
            {
                this.currentGun.Shoot(engine, new Vector2(this.Position.X, this.Position.Y), new Vector2(shootX, shootY));
            }
        }

        /// <inheritdoc />
        public override void Move(IGameEngine engine, GameTime gameTime)
        {
            var state = this.Controller.State;
            var rotateX = state.ThumbSticks.Left.X;
            var rotateY = state.ThumbSticks.Left.Y;

            // Move the player.
            this.Position += this.CalibrateMovement(engine.Bounds, new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, 0));

            // Rotate the player.
            if (rotateX != 0 || rotateY != 0)
            {
                this.Rotation = Game.AngleOf(rotateX, rotateY);
            }

            this.World = Matrix.CreateRotationZ(this.Rotation) * Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0.0f);
            this.RecalculateSpheres();
        }

        /// <inheritdoc />
        public void Die(IGameEngine engine)
        {
            engine.AddExplosion(this.Position, Color.WhiteSmoke, ExplosionSize.Large);
        }
    }
}