using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust.Enemies
{
    /// <summary>
    /// Defines a pin-wheel enemy.
    /// </summary>
    /// <remarks>
    /// Pin-wheels are dumb enemies that float around aimlessly.
    /// </remarks>
    public class Pinwheel : Enemy
    {
        private const float RotationSpeed = 15.0f / 60.0f; // RPMs.
        private const float MovementSpeed = 5;
        private const float TwoPie = (float)Math.PI * 2.0f;

        private static Random rnd = new Random();

        private Vector3 direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pinwheel" /> class.
        /// </summary>
        public Pinwheel(Model model, Vector3 position)
            : base(model, 10.0f)
        {
            var angle = MathHelper.ToRadians(rnd.Next(0, 360));

            this.direction = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0);
            this.Position = position;
            this.Scale = new Vector3(2, 2, 2);
        }

        /// <inheritdoc />
        public override int Value => 100;

        /// <inheritdoc />
        public override int Health { get; set; } = 1;

        /// <inheritdoc />
        public override void Move(IGameEngine engine, GameTime gameTime)
        {
            this.Rotation += TwoPie * (RotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (this.Rotation >= TwoPie)
            {
                this.Rotation -= TwoPie;
            }

            this.direction = this.BounceMovement(engine.Bounds, this.direction);
            this.Position += this.direction * (float)(gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed);

            base.Move(engine, gameTime);
        }

        /// <inheritdoc />
        public override void Die(IGameEngine engine)
        {
            engine.AddExplosion(this.Position, Color.Purple, ExplosionSize.Large);
        }
    }
}