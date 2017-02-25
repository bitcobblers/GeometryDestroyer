using System;
using Microsoft.Xna.Framework;
using GeometryDestroyer.Parts.Impl.Enemies;

namespace GeometryDestroyer.Parts.Impl.Directors
{
    public class SpiralDirector : Director
    {
        private readonly Random rnd = new Random();
        private bool isFinished;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpiralDirector" /> class. 
        /// </summary>
        /// <param name="spawnSystem">The spawn manager to use.</param>
        public SpiralDirector(ISpawnSystem spawnSystem, ICameraSystem cameraSystem)
            : base(spawnSystem, cameraSystem)
        {
        }

        /// <inheritdoc />
        public override int MinimumLevel => 5;

        /// <inheritdoc />
        public override void Reset(int level, TimeSpan levelTime)
        {
            base.Reset(level, levelTime);
            this.isFinished = false;
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            if(this.isFinished)
            {
                return;
            }

            int spawns = (int)(Math.Ceiling(this.Level / 5.0));

            for (int i = 0; i < spawns; i++)
            {
                var bounds = this.CameraSystem.Boundary;
                var x = rnd.Next(bounds.Left, bounds.Right);
                var y = rnd.Next(bounds.Top, bounds.Bottom);

                this.SpawnSystem.Spawn(EnemyType.Spiral, new Vector3(x, y, 0));
            }

            this.isFinished = true;
        }
    }
}