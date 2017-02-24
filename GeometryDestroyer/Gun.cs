using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace GeometryHolocaust
{
    /// <summary>
    /// Defines a gun that a player uses to fire projectiles from.
    /// </summary>
    public abstract class Gun
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Gun" /> class.
        /// </summary>
        /// <param name="owner">The owner of the gun.</param>
        protected Gun(Player owner)
        {
            this.Owner = owner;
            this.LastShot = new Stopwatch();
            this.LastShot.Start();
        }

        /// <summary>
        /// Gets the fire rate for the projectile.
        /// </summary>
        protected abstract TimeSpan FireRate { get; }

        /// <summary>
        /// Gets the player that owns the gun.
        /// </summary>
        protected Player Owner { get; }

        /// <summary>
        /// Gets the stopwatch used to throttle shots.
        /// </summary>
        protected Stopwatch LastShot { get; }

        /// <summary>
        /// Gets a value indicating whether a new shot can be fired.
        /// </summary>
        protected bool CanShoot => this.LastShot.Elapsed > this.FireRate;

        /// <summary>
        /// Fires one or more projectiles from the gun.
        /// </summary>
        /// <param name="engine">The engine to register the projectile with.</param>
        /// <param name="position">The position the projectile originates from.</param>
        /// <param name="direction">The direction to fire the gun in.</param>
        public abstract void Shoot(IGameEngine engine, Vector2 position, Vector2 direction);

        /// <summary>
        /// Defines a gun that concentrates fire over a small range.
        /// </summary>
        public class Concentrated : Gun
        {
            private const int NumProjectiles = 5;
            private const float SpeedFactor = 2.2f;
            private const float SpreadAngle = 0.349066f; // 20 degrees.

            private readonly TimeSpan fireRate = TimeSpan.FromMilliseconds(100);

            /// <summary>
            /// Initializes a new instance of the <see cref="Concentrated" /> class.
            /// </summary>
            /// <param name="owner">The owner of the gun.</param>
            public Concentrated(Player owner)
                : base(owner)
            {
            }

            /// <inheritdoc />
            protected override TimeSpan FireRate => this.fireRate;

            /// <inheritdoc />
            public override void Shoot(IGameEngine engine, Vector2 position, Vector2 direction)
            {
                if (this.CanShoot)
                {
                    var angle = Game.AngleOf(direction.X, direction.Y) - (SpreadAngle / 2);
                    var increment = SpreadAngle / NumProjectiles;

                    for (int i = 0; i < NumProjectiles; i++)
                    {
                        var shootDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                        shootDirection.Normalize();
                        engine.AddProjectile(this.Owner, new Vector3(position.X, position.Y, 0), shootDirection * SpeedFactor);
                        angle += increment;
                    }

                    this.LastShot.Restart();
                }
            }
        }

        /// <summary>
        /// Defines a scatter gun that fires single projectiles in different directions.
        /// </summary>
        public class Scattered : Gun
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
            /// Initializes a new instance of the <see cref="Scattered" /> class.
            /// </summary>
            public Scattered(Player owner)
                : base(owner)
            {
            }

            /// <inheritdoc />
            protected override TimeSpan FireRate => this.fireRate;

            public override void Shoot(IGameEngine engine, Vector2 position, Vector2 direction)
            {
                if (this.CanShoot)
                {
                    var modulus = (this.shotsFired++) % AngleIntervals;
                    var offset = AngleIncrements * (MinInterval + modulus);
                    var angle = Game.AngleOf(direction.X, direction.Y) + offset;
                    var shootDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                    shootDirection.Normalize();
                    engine.AddProjectile(this.Owner, new Vector3(position.X, position.Y, 0), shootDirection * SpeedFactor);

                    this.LastShot.Restart();
                }
            }
        }
    }
}