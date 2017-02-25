using System;
using GeometryDestroyer.Parts.Impl.Directors;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Components
{
    public class DirectorComponent : BaseDrawableGameComponent, IDirectorComponent
    {
        private const double TimeCoefficient = 2.5 * 1000.0;

        private readonly Random rnd = new Random();

        private Director[] allDirectors;
        private Director currentDirector;
        private double levelTime = TimeSpan.FromSeconds(30).TotalMilliseconds;
        private int level = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectorComponent" /> class.
        /// </summary>
        public DirectorComponent(Game game)
            : base(game)
        {
            ServiceLocator.Register<IDirectorComponent>(this);

        }

        /// <inheritdoc />
        public event EventHandler LevelIncreased = delegate { };

        // Dependencies.
        public ISpawnSystem SpawnSystem { get; private set; }
        public ICameraSystem CameraSystem { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            this.SpawnSystem = ServiceLocator.Get<ISpawnSystem>();
            this.CameraSystem = ServiceLocator.Get<ICameraSystem>();
            this.GameSystem.GameReset += this.Reset;

            this.allDirectors = new Director[]
            {
                new ScatterDirector(this.SpawnSystem, this.CameraSystem),
                new SpiralDirector(this.SpawnSystem, this.CameraSystem)
            };
        }

        /// <inheritdoc />
        public void Reset(object sender, EventArgs e)
        {
            this.currentDirector = null;
            this.levelTime = TimeSpan.FromSeconds(30).TotalMilliseconds;
            this.level = 0;
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            if (this.SuppressUpdate)
            {
                return;
            }

            if (this.currentDirector == null || this.currentDirector.RemainingTime <= TimeSpan.Zero)
            {
                // Automatically increment the level if all of the enemies have been wiped out. or the level timer has elapsed.
                this.IncreaseLevel();
            }
            else
            {
                // Update the director.
                this.currentDirector.Update(gameTime);
            }
        }

        /// <summary>
        /// Incraases the level director.
        /// </summary>
        private void IncreaseLevel()
        {
            this.level++;
            this.levelTime -= TimeCoefficient * (1.0 / this.level);
            this.LevelIncreased(this, EventArgs.Empty);

            do
            {
                int id = this.rnd.Next(0, this.allDirectors.Length);

                if (this.level >= this.allDirectors[id].MinimumLevel)
                {
                    this.currentDirector = this.allDirectors[id];
                    this.currentDirector.Reset(this.level, TimeSpan.FromMilliseconds(this.levelTime));

                    return;
                }
            } while (true);
        }
    }
}