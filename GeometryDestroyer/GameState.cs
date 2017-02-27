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
        /// Game engine is about to start (fires once per game).
        /// </summary>
        Starting,

        /// <summary>
        /// Game is in progress (fires at startup and whenever the game is unpaused).
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