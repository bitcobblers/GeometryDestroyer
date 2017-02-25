using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Guns
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

            this.MathSystem = ServiceLocator.Get<IMathSystem>();
            this.ProjectileComponent = ServiceLocator.Get<IProjectileComponent>();
        }

        /// <summary>
        /// Gets the math system to use.
        /// </summary>
        protected IMathSystem MathSystem { get; }

        /// <summary>
        /// Gets the projectile component to use.
        /// </summary>
        protected IProjectileComponent ProjectileComponent { get; }

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
        /// <param name="position">The position the projectile originates from.</param>
        /// <param name="direction">The direction to fire the gun in.</param>
        public abstract void Shoot(Vector2 position, Vector2 direction);
    }
}