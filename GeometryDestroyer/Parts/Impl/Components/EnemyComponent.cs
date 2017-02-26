using System.Collections.Generic;
using GeometryDestroyer.Parts.Impl.Enemies;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Components
{
    public class EnemyComponent : BaseDrawableGameComponent, IEnemyComponent
    {
        private readonly LinkedList<Enemy> enemies = new LinkedList<Enemy>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyComponent"/> class.
        /// </summary>
        /// <param name="game">The game object.</param>
        public EnemyComponent(Game game) : base(game)
        {
            ServiceLocator.Register<IEnemyComponent>(this);
        }

        /// <summary>
        /// Gets the list system to use
        /// </summary>
        public IListSystem ListSystem { get; private set; }

        /// <summary>
        /// Gets the game system to use.
        /// </summary>
        public IGameSystem GameSystem { get; private set; }

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();
            this.ListSystem = ServiceLocator.Get<IListSystem>();
            this.GameSystem = ServiceLocator.Get<IGameSystem>();

            this.GameSystem.GameReset += (s, e) => this.enemies.Clear();
            this.GameSystem.StateChanged += (s, e) =>
            {
                if (e == GameState.GameOver)
                {
                    foreach (var enemy in this.enemies)
                    {
                        enemy.Damage(int.MaxValue);
                    }
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

            this.ListSystem.UpdateCollection(gameTime, this.enemies);
            this.ListSystem.RemoveDeadCollection(this.enemies);
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            if(this.GameSystem.State != GameState.Running)
            {
                return;
            }

            this.ListSystem.DrawCollection(gameTime, this.enemies);
        }

        /// <inheritdoc />
        public void AddEnemy(Enemy enemy) => this.enemies.AddFirst(enemy);

        /// <inheritdoc />
        public IEnumerable<Enemy> Enemies => this.enemies;
    }
}