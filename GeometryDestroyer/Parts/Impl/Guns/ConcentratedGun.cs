using System;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Guns
{
    /// <summary>
    /// Defines a gun that concentrates fire over a small range.
    /// </summary>
    public class ConcentratedGun : Gun
    {
        private const int NumProjectiles = 5;
        private const float SpeedFactor = 2.2f;
        private const float SpreadAngle = 0.349066f; // 20 degrees.

        private readonly TimeSpan fireRate = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcentratedGun" /> class.
        /// </summary>
        /// <param name="owner">The owner of the gun.</param>
        public ConcentratedGun(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override TimeSpan FireRate => this.fireRate;

        /// <inheritdoc />
        public override void Shoot(Vector2 position, Vector2 direction)
        {
            if (this.CanShoot)
            {
                var angle = this.MathSystem.AngleOf(direction.X, direction.Y) - (SpreadAngle / 2);
                var increment = SpreadAngle / NumProjectiles;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    var shootDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                    shootDirection.Normalize();
                    this.ProjectileComponent.AddProjectile(this.Owner, new Vector3(position.X, position.Y, 0), shootDirection * SpeedFactor);
                    angle += increment;
                }

                this.LastShot.Restart();
            }
        }
    }
}