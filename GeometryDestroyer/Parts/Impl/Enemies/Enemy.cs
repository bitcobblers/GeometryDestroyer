﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Enemies
{

    public abstract class Enemy : BoundedObject, ICanDie
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy" /> class.
        /// </summary>
        /// <param name="model">The model for the enemy.</param>
        public Enemy(Model model)
            : base(model)
        {
            this.PlayerComponent = ServiceLocator.Get<IPlayerComponent>();
            this.ParticleComponent = ServiceLocator.Get<IParticleComponent>();
        }

        /// <summary>
        /// Gets the player component tou se.
        /// </summary>
        public IPlayerComponent PlayerComponent { get; }

        /// <summary>
        /// Gets the particle component to use.
        /// </summary>
        public IParticleComponent ParticleComponent { get; }

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

        /// <summary>
        /// Applies damage to the enemy and returns the 'value' of the hit.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        /// <returns>The value of the hit.</returns>
        public virtual int Damage(int damage)
        {
            this.Health -= damage;

            if (this.Health <= 0)
            {
                this.OnDestroyed();
                return this.Value;
            }
            else
            {
                return 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update collision detection.
            base.Update(gameTime);

            // Check for collisions with players.
            foreach (var player in this.PlayerComponent.Players)
            {
                if (this.IntersectsWith(player))
                {
                    player.Kill();
                    this.Damage(int.MaxValue);
                }
            }
        }

        /// <summary>
        /// Triggered whenever an enemy is killed.
        /// </summary>
        protected virtual void OnDestroyed()
        {
        }
    }
}