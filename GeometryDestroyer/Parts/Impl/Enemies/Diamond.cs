using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Enemies
{
    public class Diamond : Enemy
    {
        private const float MovementSpeed = 20;
        private const float MinScale = 0.75f;
        private const float MaxScale = 1.75f;
        private const float DeltaScale = 0.01f;

        private int scaleDirection = 1;
        private float currentScale = 1.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Diamond"/> class.
        /// </summary>
        /// <param name="model">The model to use for the enemy.</param>
        public Diamond(Model model, Vector3 position) : base(model)
        {
            this.Position = position;
        }

        /// <inheritdoc />
        public override int Health { get; set; } = 1;

        /// <inheritdoc />
        public override int Value => 200;

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            // Update the position.
            var targetVector = this.ClosestPoint(this.PlayerComponent.Players);

            if(targetVector != Vector3.Zero)
            {
                targetVector.Normalize();
                this.Position += targetVector * ((float)(gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed));
            }

            // Adjust scaling.
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

            this.Scale = new Vector3(1.0f, this.currentScale, 1.0f);

            // Update collision detection.
            base.Update(gameTime);
        }

        /// <inheritdoc />
        public override void Kill()
        {
            base.Kill();
            this.ParticleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.Blue, ExplosionSize.Medium);
        }

        /// <summary>
        /// Enumerates the players and calculates a vector between the current position and the closest player.
        /// </summary>
        /// <param name="players">The players to find the distance between.</param>
        /// <returns>A vector to the closest player.</returns>
        private Vector3 ClosestPoint(IEnumerable<Player> players)
        {
            float maxDistance = float.MaxValue;
            Vector3 result = Vector3.Zero;

            foreach (var player in players.Where(p => p.IsActive))
            {
                var distance = Vector3.Distance(this.Position, player.Position);

                if(distance<maxDistance)
                {
                    maxDistance = distance;
                    result = player.Position - this.Position;
                }
            }

            return result;
        }
    }
}