using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust.Enemies
{

    public abstract class Enemy : BoundedObject, ICanUpdate, ICanDie
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy" /> class.
        /// </summary>
        /// <param name="model">The model for the enemy.</param>
        public Enemy(Model model, float mass)
            : base(model, mass)
        {
        }

        /// <inheritdoc />
        public virtual bool IsAlive => this.Health > 0;

        /// <summary>
        /// Gets the value of the enemy;
        /// </summary>
        public abstract int Value { get; }

        /// <summary>
        /// Gets or sets the health of the enemy;
        /// </summary>
        public abstract int Health { get; set; }

        /// <inheritdoc />
        public abstract void Die(IGameEngine engine);

        /// <inheritdoc />
        public virtual void Update(IGameEngine engine, GameTime gameTime)
        {
        }

        /// <summary>
        /// Applies damage to the enemy and returns the 'value' of the hit.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        /// <returns>The value of the hit.</returns>
        public virtual int Damage(int damage)
        {
            this.Health -= damage;

            if(this.Health <= 0)
            {
                return this.Value;
            }
            else
            {
                return 0;
            }
        }
    }
}