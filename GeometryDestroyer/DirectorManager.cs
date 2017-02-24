using System;
using GeometryHolocaust.Directors;
using Microsoft.Xna.Framework.Content;

namespace GeometryHolocaust
{
    public class DirectorManager
    {
        private const double TimeCoefficient = 2.5 * 1000.0;

        private readonly Random rnd = new Random();
        private readonly IGameEngine engine;
        private readonly Director[] allDirectors;

        private Director currentDirector;
        private double levelTime = TimeSpan.FromSeconds(30).TotalMilliseconds;
        private int level = 0;

        /// <summary>
        /// Triggered whenever the level is incremented.
        /// </summary>
        public event Action LevelIncreased = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="GameDirector" /> class.
        /// </summary>
        /// <param name="engine">The current game engine.</param>
        /// <param name="content">The content manager to use.</param>
        public DirectorManager(IGameEngine engine, ContentManager content)
        {
            var spawnManager = new SpawnManager(content);

            this.engine = engine;
            this.allDirectors = new[]
            {
                new ScatterDirector(spawnManager)
            };
        }

        public void Reset()
        {
            this.currentDirector = null;
            this.levelTime = TimeSpan.FromSeconds(30).TotalMilliseconds;
            int level = 0;
        }

        /// <summary>
        /// Updated the director.
        /// </summary>
        public void Update()
        {
            if (this.currentDirector == null || this.currentDirector.RemainingTime <= TimeSpan.Zero)
            {
                // Automatically increment the level if all of the enemies have been wiped out. or the level timer has elapsed.
                this.IncreaseLevel();
            }
            else
            {
                // Update the director.
                this.currentDirector.Run(this.engine);
            }
        }

        /// <summary>
        /// Incraases the level director.
        /// </summary>
        private void IncreaseLevel()
        {
            this.level++;
            this.levelTime -= TimeCoefficient * (1.0 / this.level);
            this.LevelIncreased();

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