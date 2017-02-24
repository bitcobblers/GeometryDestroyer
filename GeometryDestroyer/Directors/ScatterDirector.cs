using System;
using System.Diagnostics;
using GeometryHolocaust.Enemies;
using Microsoft.Xna.Framework;

namespace GeometryHolocaust.Directors
{
    /// <summary>
    /// Defines a director that creates enemies at random locations.
    /// </summary>
    public class ScatterDirector : Director
    {
        private const int SpawnConstant = 10;

        private readonly Random rnd = new Random();
        private readonly Stopwatch spawnTimer = new Stopwatch();

        private TimeSpan spawnInterval;
        private TimeSpan overflowSpan;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterDirector" /> class.
        /// </summary>
        /// <param name="spawnManager">The spawn manager to use.</param>
        public ScatterDirector(SpawnManager spawnManager)
            : base(spawnManager)
        {
        }

        /// <inheritdoc />
        public override int MinimumLevel => 1;

        /// <inheritdoc />
        public override void Reset(int level, TimeSpan levelTime)
        {
            base.Reset(level, levelTime);

            this.spawnInterval = TimeSpan.FromMilliseconds(levelTime.TotalMilliseconds / (level * SpawnConstant));
            this.spawnTimer.Start();
        }

        /// <inheritdoc />
        public override void Run(IGameEngine engine)
        {
            if (this.spawnTimer.Elapsed > this.spawnInterval)
            {
                var x = rnd.Next(engine.Bounds.Left, engine.Bounds.Right);
                var y = rnd.Next(engine.Bounds.Top, engine.Bounds.Bottom);

                engine.AddEnemy(this.SpawnManager.Random(EnemyType.Pinwheel, new Vector3(x, y, 0)));
                this.spawnTimer.Restart();
            }
        }
    }
}