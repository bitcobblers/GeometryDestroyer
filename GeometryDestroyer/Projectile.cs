using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust
{
    /// <summary>
    /// Defines a projectile shot by a player.
    /// </summary>
    public class Projectile : BoundedObject, ICanUpdate, ICanDie
    {
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
            : base(model, 1.0f)
        {
            this.owner = owner;
            this.direction = direction;
            this.power = power;

            this.Position = position;
        }

        /// <inheritdoc />
        public bool IsAlive { get; private set; } = true;

        /// <inheritdoc />
        public void Update(IGameEngine engine, GameTime gameTime)
        {
            // Check for collisions with enemies.
            foreach (var enemy in engine.Enemies)
            {
                if (this.IntersectsWith(enemy))
                {
                    this.IsAlive = false;
                    this.owner.Score += enemy.Damage(this.power);
                }
            }

            // Boundary check.
            if (this.IsWithinBounds(engine.Bounds) == false)
            {
                this.IsAlive = false;
            }
        }

        /// <inheritdoc />
        public override void Move(IGameEngine engine, GameTime gameTime)
        {
            this.Position += new Vector3(this.direction.X, this.direction.Y, 0);
            this.World = Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0);
            this.RecalculateSpheres();
        }

        /// <inheritdoc />
        public void Die(IGameEngine engine)
        {
            engine.AddExplosion(this.Position, Color.Yellow, ExplosionSize.Small);
        }
    }
}