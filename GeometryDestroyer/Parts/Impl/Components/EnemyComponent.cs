using System;
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
        /// Gets the player component to use.
        /// </summary>
        public IPlayerComponent PlayerComponent { get; private set; }

        /// <inheritdoc />
        public IEnumerable<Enemy> Enemies => this.enemies;

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();
            this.ListSystem = ServiceLocator.Get<IListSystem>();
            this.PlayerComponent = ServiceLocator.Get<IPlayerComponent>();

            this.GameSystem.GameReset += (s, e) => this.enemies.Clear();
            this.GameSystem.StateChanged += (s, e) =>
            {
                if (e == GameState.GameOver)
                {
                    foreach (var enemy in this.enemies)
                    {
                        enemy.Kill();
                    }

                    foreach(var player in this.PlayerComponent.Players)
                    {
                        player.PlayerKilled -= this.PlayerKilled;
                    }
                }
                else if(e == GameState.Starting)
                {
                    foreach(var player in this.PlayerComponent.Players)
                    {
                        player.PlayerKilled += this.PlayerKilled;
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
            if (this.GameSystem.State != GameState.Running && this.GameSystem.State != GameState.Paused)
            {
                return;
            }

            this.ListSystem.DrawCollection(gameTime, this.enemies);
        }

        /// <inheritdoc />
        public void AddEnemy(Enemy enemy) => this.enemies.AddFirst(enemy);

        /// <inheritdoc />
        public void KillAll(Player player)
        {
            foreach(var enemy in this.enemies)
            {
                player.Score += enemy.Damage(int.MaxValue);
            }
        }

        private void PlayerKilled(object sender, EventArgs e)
        {
            foreach (var enemy in this.enemies)
            {
                enemy.Kill();
            }
        }
    }
}