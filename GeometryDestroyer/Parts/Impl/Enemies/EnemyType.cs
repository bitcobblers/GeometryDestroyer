using System;

namespace GeometryDestroyer.Parts.Impl.Enemies
{
    /// <summary>
    /// Defines the type of enemies.
    /// </summary>
    /// <remarks>
    /// This enum is used by the spawn manager to determine which type of enemy type to instantiate by the directors.
    /// </remarks>
    [Flags]
    public enum EnemyType : int
    {
        Any,
        Pinwheel,
        Spiral
    }
}