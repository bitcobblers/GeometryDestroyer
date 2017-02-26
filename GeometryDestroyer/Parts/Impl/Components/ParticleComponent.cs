using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Components
{
    public class ParticleComponent : BaseDrawableGameComponent, IParticleComponent
    {
        private readonly LinkedList<Particle> particles = new LinkedList<Particle>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleComponent"/> class.
        /// </summary>
        /// <param name="game">The game object.</param>
        public ParticleComponent(Game game) : base(game)
        {
            ServiceLocator.Register<IParticleComponent>(this);
        }

        /// <summary>
        /// Gets or sets the list system to use.
        /// </summary>
        public IListSystem ListSystem { get; private set; }

        /// <summary>
        /// Gets or sets the camera system to use.
        /// </summary>
        public ICameraSystem CameraSystem { get; private set; }

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();
            this.ListSystem = ServiceLocator.Get<IListSystem>();
            this.CameraSystem = ServiceLocator.Get<ICameraSystem>();
            this.GameSystem.GameReset += (s, e) => this.particles.Clear();
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            this.ListSystem.UpdateCollection(gameTime, this.particles);
            this.ListSystem.RemoveDeadCollection(this.particles);
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.ListSystem.DrawCollection(gameTime, this.particles);
        }

        /// <inheritdoc />
        public void AddExplosion(EmitterDescription description, Vector3 position, Color color, ExplosionSize size)
        {
            var model = this.Game.Content.Load<Model>(description.ModelName);
            var particleSizes = new Dictionary<ExplosionSize, int>
            {
                [ExplosionSize.Small] = 10,
                [ExplosionSize.Medium] = 50,
                [ExplosionSize.Large] = 250,
                [ExplosionSize.Huge] = 1000
            };

            for (int i = 0; i < particleSizes[size]; i++)
            {
                this.particles.AddFirst(new Particle(description, color, model, position));
            }
        }
    }
}