namespace GeometryDestroyer
{
    /// <summary>
    /// Defines a type that can be explicitly killed by another type.
    /// </summary>
    public interface ICanBeKilled
    {
        /// <summary>
        /// Forces an object to die.
        /// </summary>
        void Kill();
    }
}