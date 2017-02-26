using System;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    public interface IGameSystem
    {
        /// <summary>
        /// Gets the current state of the game.
        /// </summary>
        GameState State { get; }

        /// <summary>
        /// Triggered whenever the state changes.
        /// </summary>
        event EventHandler<GameState> StateChanged;

        /// <summary>
        /// Triggered whenever the game resets.
        /// </summary>
        event EventHandler GameReset;

        /// <summary>
        /// Resets the game.
        /// </summary>
        void Reset();

        /// <summary>
        /// Registers an active player with the game.
        /// </summary>
        /// <param name="player">The player to register.</param>
        void RegisterPlayer(Player player);

        /// <summary>
        /// Toggles whether the game is paused or unpaused.
        /// </summary>
        void TogglePaused();
    }
}