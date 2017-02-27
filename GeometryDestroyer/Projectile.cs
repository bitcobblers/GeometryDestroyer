using System;
using GeometryDestroyer.Parts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer
{
    /// <summary>
    /// Defines a projectile shot by a player.
    /// </summary>
    public class Projectile : BoundedObject, ICanUpdate, ICanDie, ICanBeKilled
    {
        private readonly IEnemyComponent enemyComponent;
        private readonly IParticleComponent particleComponent;
        private readonly Player owner;
        private readonly Vector2 direction;
        private readonly int power;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projectile" /> class.
        /// </summary>
        /// <param name="owner">The player that owns the projectile.</param>
        /// <param name="model">The model of the projectile to render.</param>
        /// <param name="position">The starting position of the projectile.</param>
        /// <param name="direction">The direction the particle is traveling.</param>
        /// <param name="power">The power of the projectile (i.e the amount of health to subtract from each enemy on hit).</param>
        public Projectile(Player owner, Model model, Vector3 position, Vector2 direction, int power)
            : base(model)
        {
            this.owner = owner;
            this.direction = direction;
            this.power = power;
            this.enemyComponent = ServiceLocator.Get<IEnemyComponent>();
            this.particleComponent = ServiceLocator.Get<IParticleComponent>();
            this.Position = position;
        }

        /// <inheritdoc />
        public bool IsAlive { get; private set; } = true;

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            this.Position += new Vector3(this.direction.X, this.direction.Y, 0);
            this.World = Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0);
            this.RecalculateSpheres();

            // Check for collisions with enemies.
            foreach (var enemy in this.enemyComponent.Enemies)
            {
                if (this.IntersectsWith(enemy))
                {
                    this.IsAlive = false;
                    this.owner.Score += enemy.Damage(this.power);
                }
            }

            // Boundary check.
            if (this.IsWithinBounds(this.CameraSystem.Boundary) == false)
            {
                this.IsAlive = false;
            }

            if (this.IsAlive == false)
            {
                this.Kill();
            }
        }

        /// <inheritdoc />
        public void Kill()
        {
            this.particleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.Yellow, ExplosionSize.Small);
        }
    }
}