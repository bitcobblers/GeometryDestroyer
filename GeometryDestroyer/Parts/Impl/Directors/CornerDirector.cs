using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Directors
{
    public class CornerDirector : Director
    {
        private const int SpawnConstant = 25;

        private readonly Stopwatch spawnTimer = new Stopwatch();
        private readonly Point[] corners;

        private TimeSpan spawnInterval;
        private int index = 0;

        public CornerDirector(ISpawnSystem spawnSystem, ICameraSystem cameraSystem)
            : base(spawnSystem, cameraSystem)
        {
            this.corners = new[]
            {
                new Point(cameraSystem.Boundary.Left, cameraSystem.Boundary.Top),
                new Point(cameraSystem.Boundary.Right, cameraSystem.Boundary.Top),
                new Point(cameraSystem.Boundary.Left, cameraSystem.Boundary.Bottom),
                new Point(cameraSystem.Boundary.Right, cameraSystem.Boundary.Bottom)
            };
        }

        /// <inheritdoc />
        public override int MinimumLevel => 1;

        /// <inheritdoc />
        public override void Reset(int level, TimeSpan levelTime)
        {
            base.Reset(level, levelTime);

            this.index = 0;
            this.LevelTime = TimeSpan.FromMilliseconds(levelTime.TotalMilliseconds / 5.0f);
            this.spawnInterval = TimeSpan.FromMilliseconds(levelTime.TotalMilliseconds / (level * SpawnConstant));
            this.spawnTimer.Start();
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            if(this.spawnTimer.Elapsed > this.spawnInterval)
            {
                var corner = this.corners[(this.index++) % 4];

                this.SpawnSystem.Spawn(Enemies.EnemyType.Diamond, new Vector3(corner.X, corner.Y, 0));
                this.spawnTimer.Restart();
            }
        }
    }
}