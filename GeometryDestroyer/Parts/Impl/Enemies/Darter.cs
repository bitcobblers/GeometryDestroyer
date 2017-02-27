using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Enemies
{
    public class Darter : Enemy
    {
        private const float MovementSpeed = 45;
        private const float MinScale = 0.75f;
        private const float MaxScale = 1.25f;
        private const float DeltaScale = 0.005f;

        private static readonly Random rnd = new Random();

        private Vector3 direction;
        private int scaleDirection = 1;
        private float currentScale = 1.0f;
        private float tumble = 0.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Darter"/> class.
        /// </summary>
        /// <param name="model">The model to render the object with.</param>
        public Darter(Model model, Vector3 position)
            : base(model)
        {
            var angle = MathHelper.ToRadians(rnd.Next(0, 360));

            this.direction = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0);
            this.Position = position;
            this.Rotation = angle;
        }

        /// <inheritdoc />
        public override int Value => 500;

        /// <inheritdoc />
        public override int Health { get; set; } = 1;

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            this.currentScale += (this.scaleDirection * DeltaScale);

            if (this.currentScale < MinScale)
            {
                this.currentScale = MinScale;
                this.scaleDirection = -this.scaleDirection;
            }
            else if (this.currentScale > MaxScale)
            {
                this.currentScale = MaxScale;
                this.scaleDirection = -this.scaleDirection;
            }

            this.direction = this.ReverseMovement(this.CameraSystem.Boundary, this.direction);
            this.Rotation = this.MathSystem.AngleOf(this.direction.X, this.direction.Y);

            this.Scale = new Vector3(this.currentScale, this.currentScale, 2);
            this.Position += this.direction * (float)(gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed);

            // Update collision detection.
            base.Update(gameTime);
        }

        /// <inheritdoc />
        public override void Kill()
        {
            base.Kill();
            this.ParticleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.Yellow, ExplosionSize.Medium);
        }
    }
}