using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Components
{
    public class ProjectileComponent : BaseDrawableGameComponent, IProjectileComponent
    {
        private readonly LinkedList<Projectile> projectiles = new LinkedList<Projectile>();
        private Model model;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectileComponent"/> class.
        /// </summary>
        /// <param name="game">The game object.</param>
        public ProjectileComponent(Game game) : base(game)
        {
            ServiceLocator.Register<IProjectileComponent>(this);
        }

        public IListSystem ListSystem { get; private set; }

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();
            this.model = this.Game.Content.Load<Model>("Models/projectile");
            this.ListSystem = ServiceLocator.Get<IListSystem>();
            this.GameSystem.GameReset += (s, e) => this.projectiles.Clear();
            this.GameSystem.StateChanged += (s, e) =>
            {
                if (e == GameState.GameOver)
                {
                    foreach (var projectile in this.projectiles)
                    {
                        projectile.Kill();
                    }

                    this.projectiles.Clear();
                }
            };
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            if (this.SuppressUpdate)
            {
                return;
            }

            this.ListSystem.UpdateCollection(gameTime, this.projectiles);
            this.ListSystem.RemoveDeadCollection(this.projectiles);
        }

        public override void Draw(GameTime gameTime)
        {
            this.ListSystem.DrawCollection(gameTime, this.projectiles);
        }

        /// <inheritdoc />
        public void AddProjectile(Player owner, Vector3 position, Vector2 direction)
        {
            this.projectiles.AddFirst(new Projectile(owner, this.model, position, direction, 10));
        }
    }
}