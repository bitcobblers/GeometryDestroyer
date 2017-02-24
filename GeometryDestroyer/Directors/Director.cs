using System;
using System.Diagnostics;

namespace GeometryHolocaust.Directors
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
        /// <param name="spawnManager">The spawn manager to use.</param>
        protected Director(SpawnManager spawnManager)
        {
            this.SpawnManager = spawnManager;
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
        protected SpawnManager SpawnManager { get; }

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
        /// Runs the director.
        /// </summary>
        /// <param name="engine">The game engine.</param>
        public abstract void Run(IGameEngine engine);
    }
}