using System.Collections.Generic;
using System.Linq;
using GeometryHolocaust.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust
{
    public class GameEngine : IGameEngine
    {
        private const int BoundarySize = 50;

        private readonly GraphicsDeviceManager graphicsManager;
        private readonly GraphicsDevice graphicsDevice;
        private readonly ContentManager content;
        private readonly DirectorManager directorManager;

        private readonly LinkedList<Enemy> enemies = new LinkedList<Enemy>();
        private readonly LinkedList<Projectile> projectiles = new LinkedList<Projectile>();
        private readonly LinkedList<Particle> particles = new LinkedList<Particle>();
        private readonly LinkedList<Player> players = new LinkedList<Player>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine" /> class.
        /// </summary>
        /// <param name="graphicsManager">The graphics manager for the game.</param>
        /// <param name="content">The content manager to use.</param>
        public GameEngine(GraphicsDeviceManager graphicsManager, ContentManager content)
        {
            var width = graphicsManager.PreferredBackBufferWidth;
            var height = graphicsManager.PreferredBackBufferHeight;

            this.graphicsManager = graphicsManager;
            this.graphicsDevice = graphicsManager.GraphicsDevice;
            this.content = content;

            this.View = Matrix.CreateLookAt(new Vector3(0, 0, BoundarySize), Vector3.Zero, Vector3.Up);
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), width / height, 0.01f, 100.0f);
            this.Bounds = new Rectangle(-BoundarySize, -BoundarySize, BoundarySize * 2, BoundarySize * 2);

            // Start the director.
            this.directorManager = new DirectorManager(this, content);
        }

        /// <inheritdoc />
        public GameState State { get; private set; } = GameState.NotStarted;

        /// <inheritdoc />
        public Rectangle Bounds { get; }

        /// <inheritdoc />
        public Matrix View { get; }

        /// <inheritdoc />
        public Matrix Projection { get; }

        /// <inheritdoc />
        public IEnumerable<GameController> Controllers => this.players.Select(p => p.Controller);

        /// <inheritdoc />
        public IEnumerable<Player> Players => this.players;

        /// <inheritdoc />
        public IEnumerable<Enemy> Enemies => this.enemies;

        /// <inheritdoc />
        public IEnumerable<Projectile> Projectiles => this.projectiles;

        /// <summary>
        /// Updates the engine.
        /// </summary>
        /// <param name="gameTime">The time in the game.</param>
        public void Update(GameTime gameTime)
        {
            // Always update the controllers.
            foreach (var controller in this.Controllers)
            {
                controller.Update();
            }

            if (this.State == GameState.Running)
            {
                // Update the director.
                this.directorManager.Update();

                // Move all of the objects in the game.
                this.MoveCollections(gameTime, this.players, this.enemies, this.projectiles, this.particles);

                // Update each of the collections.
                this.UpdateCollections(gameTime, this.players, this.enemies, this.projectiles, this.particles);

                // Killing collections must be done individually since LinkedList<T> is not covariant.
                this.KillCollection(this.players);
                this.KillCollection(this.enemies);
                this.KillCollection(this.projectiles);
                this.KillCollection(this.particles);
            }
        }

        /// <summary>
        /// Drawss a new frame.
        /// </summary>
        /// <param name="gameTime">The time in the game.</param>
        public void Draw(GameTime gameTime)
        {
            this.DrawCollections(gameTime, this.players, this.enemies, this.projectiles, this.particles);
        }

        #region State Methods

        /// <inheritdoc />
        public void AddExplosion(Vector3 position, Color color, ExplosionSize size)
        {
            var particleSizes = new Dictionary<ExplosionSize, int>
            {
                [ExplosionSize.Small] = 10,
                [ExplosionSize.Medium] = 50,
                [ExplosionSize.Large] = 250,
                [ExplosionSize.Huge] = 1000
            };

            var emitter = new ParticleEmitter(EmitterDescription.Explosion, color, this.content, position, particleSizes[size]);

            foreach (var particle in emitter.Particles)
            {
                this.particles.AddFirst(particle);
            }
        }

        /// <inheritdoc />
        public void AddProjectile(Player owner, Vector3 position, Vector2 direction)
        {
            this.projectiles.AddFirst(new Projectile(owner, this.content.Load<Model>(@"Models\projectile"), position, direction, 10));
        }

        /// <inheritdoc />
        public void AddEnemy(Enemy enemy) => this.enemies.AddFirst(enemy);

        #endregion

        #region Maintenance

        /// <summary>
        /// Resets the game engine.
        /// </summary>
        public void Reset(IEnumerable<GameController> controllers)
        {
            this.directorManager.Reset();

            // Clear the game state.
            this.enemies.Clear();
            this.projectiles.Clear();
            this.particles.Clear();
            this.players.Clear();

            foreach (var controller in controllers)
            {
                var player = new Player(controller, this.content.Load<Model>("Models/player"));

                this.directorManager.LevelIncreased += () => player.ChangeGun();
                this.players.AddLast(player);

                controller.Update();
            }

            this.State = GameState.Running;
        }

        /// <summary>
        /// Toggles the state between paused and running.
        /// </summary>
        public void TogglePaused()
        {
            if (this.State == GameState.Paused)
            {
                this.State = GameState.Running;
            }
            else if (this.State == GameState.Running)
            {
                this.State = GameState.Paused;
            }
        }

        /// <summary>
        /// Draws a series of collections.
        /// </summary>
        /// <param name="gameTime">The time in the game.</param>
        /// <param name="collections">The collections to draw.</param>
        private void DrawCollections(GameTime gameTime, params IEnumerable<ICanDraw>[] collections)
        {
            foreach (var collection in collections)
            {
                foreach (var item in collection)
                {
                    item.Draw(this, gameTime);
                }
            }
        }

        /// <summary>
        /// Moves a series of collections.
        /// </summary>
        /// <param name="gameTime">The time in the game.</param>
        /// <param name="collections">The collections to move.</param>
        private void MoveCollections(GameTime gameTime, params IEnumerable<ICanMove>[] collections)
        {
            foreach (var collection in collections)
            {
                foreach (var item in collection)
                {
                    item.Move(this, gameTime);
                }
            }
        }


        /// <summary>
        /// Updates a series of collections.
        /// </summary>
        /// <param name="gameTime">The time in the game.</param>
        /// <param name="collections">The collections to update.</param>
        private void UpdateCollections(GameTime gameTime, params IEnumerable<ICanUpdate>[] collections)
        {
            foreach (var collection in collections)
            {
                foreach (var item in collection)
                {
                    item.Update(this, gameTime);
                }
            }
        }

        /// <summary>
        /// Processes a collection and eliminates any elements that are no longer 'alive'.
        /// </summary>
        /// <typeparam name="T">The type of collection to update.</typeparam>
        /// <param name="collection">The collection to process.</param>
        private void KillCollection<T>(LinkedList<T> collection)
            where T : ICanDie
        {
            var node = collection.First;

            while (node != null)
            {
                var nextNode = node.Next;

                if (node.Value.IsAlive == false)
                {
                    node.Value.Die(this);
                    collection.Remove(node);
                }

                node = nextNode;
            }
        }

        #endregion
    }
}