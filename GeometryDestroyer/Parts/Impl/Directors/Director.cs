using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Directors
{
    /// <summary>
    /// Defines a base-line director.
    /// </summary>
    public abstract class Director
    {
        private readonly Stopwatch timer = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="Director" /> class.
        /// </summary>
        /// <param name="spawnSystem">The spawn manager to use.</param>
        protected Director(ISpawnSystem spawnSystem, ICameraSystem cameraSystem)
        {
            this.SpawnSystem = spawnSystem;
            this.CameraSystem = cameraSystem;

            this.timer.Start();
        }

        /// <summary>
        /// Gets the remaining time for the director.
        /// </summary>
        public TimeSpan RemainingTime => this.LevelTime - this.timer.Elapsed;

        /// <summary>
        /// Gets the current level.
        /// </summary>
        /// <remarks>
        /// This value is fixed and cannot be modified by the director.
        /// </remarks>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the length of time allocated for the level.
        /// </summary>
        /// <remarks>
        /// This is the total length of time allocated for the director to run.
        /// Note: this value can be overridden by the director.
        /// </remarks>
        public TimeSpan LevelTime { get; protected set; }

        /// <summary>
        /// Gets the minimum level where the director can be used.
        /// </summary>
        public abstract int MinimumLevel { get; }

        /// <summary>
        /// Gets the spawn manager to use.
        /// </summary>
        protected ISpawnSystem SpawnSystem { get; }

        /// <summary>
        /// Gets the camera system to use.
        /// </summary>
        protected ICameraSystem CameraSystem { get; }

        /// <summary>
        /// Resets the director so that it can be run.
        /// </summary>
        /// <param name="level">The level of the director.</param>
        /// <param name="levelTime">The time allocated for the level.</param>
        public virtual void Reset(int level, TimeSpan levelTime)
        {
            this.Level = level;
            this.LevelTime = levelTime;
            this.timer.Restart();
        }

        /// <summary>
        /// Updates the director.
        /// </summary>
        /// <param name="gameSystem">The game system.</param>
        public abstract void Update(GameTime gameTime);
    }
}