using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using GeometryDestroyer.Parts;
using GeometryDestroyer.Parts.Impl.Enemies;

namespace GeometryDestroyer.Parts.Impl.Directors
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
        /// <param name="spawnSystem">The spawn manager to use.</param>
        public ScatterDirector(ISpawnSystem spawnSystem, ICameraSystem cameraSystem)
            : base(spawnSystem, cameraSystem)
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
        public override void Update(GameTime gameTime)
        {
            if (this.spawnTimer.Elapsed > this.spawnInterval)
            {
                var bounds = this.CameraSystem.Boundary;
                var x = rnd.Next(bounds.Left, bounds.Right);
                var y = rnd.Next(bounds.Top, bounds.Bottom);

                this.SpawnSystem.Random(EnemyType.Pinwheel, new Vector3(x, y, 0));
                this.spawnTimer.Restart();
            }
        }
    }
}