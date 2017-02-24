namespace GeometryHolocaust
{
    /// <summary>
    /// Defines an object that can be 'killed'.
    /// </summary>
    public interface ICanDie
    {
        /// <summary>
        /// Gets a value indicating whether the object is alive.
        /// </summary>
        /// <param name="engine">The game engine.</param>
        /// <returns>True if the object is still alive.</returns>
        bool IsAlive { get; }

        /// <summary>
        /// Triggered whenever the object died.
        /// </summary>
        /// <param name="engine">The game engine.</param>
        void Die(IGameEngine engine);
    }
}