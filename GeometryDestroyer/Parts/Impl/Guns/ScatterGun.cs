using System;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Guns
{
    /// <summary>
    /// Defines a scatter gun that fires single projectiles in different directions.
    /// </summary>
    public class ScatterGun : Gun
    {
        private const int MinInterval = -2;
        private const int AngleIntervals = 5;
        private const float SpeedFactor = 2.2f;
        private const float SpreadAngle = 0.523599f; // 30 degrees.
        private const float AngleIncrements = SpreadAngle / AngleIntervals;

        private readonly Random rnd = new Random();
        private readonly TimeSpan fireRate = TimeSpan.FromMilliseconds(100);
        private long shotsFired = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterGun" /> class.
        /// </summary>
        public ScatterGun(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override TimeSpan FireRate => this.fireRate;

        public override void Shoot(Vector2 position, Vector2 direction)
        {
            if (this.CanShoot)
            {
                var modulus = (this.shotsFired++) % AngleIntervals;
                var offset = AngleIncrements * (MinInterval + modulus);
                var angle = this.MathSystem.AngleOf(direction.X, direction.Y) + offset;
                var shootDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                shootDirection.Normalize();

                this.ProjectileComponent.AddProjectile(this.Owner, new Vector3(position.X, position.Y, 0), shootDirection * SpeedFactor);
                this.LastShot.Restart();
            }
        }
    }
}