namespace GeometryDestroyer
{
    /// <summary>
    /// Enumerates the possible states of the game.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Game engine has been loaded but hasn't been started.
        /// </summary>
        NotStarted,

        /// <summary>
        /// Game is in progress.
        /// </summary>
        Running,

        /// <summary>
        /// Game is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Game is over.
        /// </summary>
        GameOver
    }
}