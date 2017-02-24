namespace GeometryHolocaust
{
    public enum ExplosionSize
    {
        Small,
        Medium,
        Large,
        Huge
    }

    /// <summary>
    /// Defines a description for an emitter.
    /// </summary>
    public class EmitterDescription
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="EmitterDescription" /> class.
        /// </summary>
        /// <param name="modelName">The name of the model to load.</param>
        /// <param name="speedFactor">The speed factor for velocity.</param>
        /// <param name="minTimeToLive">The minimum time to live (in frames).</param>
        /// <param name="maxTimeToLive">The maximum time to live (in frames).</param>
        /// <param name="isContinuous">True if the emitter should continually create new particles.</param>
        /// <param name="isBurst">True if all of the particles should be created at once.</param>
        public EmitterDescription(string modelName, float speedFactor, int minTimeToLive, int maxTimeToLive, bool isContinuous, bool isBurst)
        {
            this.ModelName = modelName;
            this.SpeedFactor = speedFactor;
            this.MinTimeToLive = minTimeToLive;
            this.MaxTimeToLive = maxTimeToLive;
            this.IsContinuous = false;
            this.IsBurst = true;
        }

        /// <summary>
        /// Gets the name of the model to load.
        /// </summary>
        public string ModelName { get; }

        /// <summary>
        /// Gets the speed factor for velocity.
        /// </summary>
        public float SpeedFactor { get; }

        /// <summary>
        /// Gets the minimum time to live (in frames);
        /// </summary>
        public int MinTimeToLive { get; }

        /// <summary>
        /// Gets the maximum time to live (in frames).
        /// </summary>
        public int MaxTimeToLive { get; }

        /// <summary>
        /// Gets a value indicating whether this particle effect should continue indefinitely.
        /// </summary>
        public bool IsContinuous { get; }

        /// <summary>
        /// Gets a value indicating whether the particles should be burst.
        /// </summary>
        public bool IsBurst { get; }

        /// <summary>
        /// Stock description for an explosion effect.
        /// </summary>
        public static EmitterDescription Explosion = new EmitterDescription(@"Models\particle", 1.0f, 15, 30, false, true);
    }
}