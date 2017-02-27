using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    public class GameSystem : IGameSystem
    {
        private readonly Dictionary<PlayerIndex, Player> registeredPlayers = new Dictionary<PlayerIndex, Player>();
        private GameState state = GameState.NotStarted;

        /// <inheritdoc />
        public event EventHandler GameReset = delegate { };

        /// <inheritdoc />
        public event EventHandler<GameState> StateChanged = delegate { };

        /// <inheritdoc />
        public GameState State
        {
            get
            {
                return this.state;
            }
            private set
            {
                this.state = value;
                this.StateChanged(this, this.state);
            }
        }

        /// <inheritdoc />
        public void Reset()
        {
            this.GameReset(this, EventArgs.Empty);
            this.State = GameState.Starting;
            this.State = GameState.Running;
        }

        /// <summary>
        /// Registers a player with the game.
        /// </summary>
        /// <param name="player"></param>
        public void RegisterPlayer(Player player)
        {
            player.PlayerEliminated += this.PlayerEliminated;
        }

        /// <inheritdoc />
        public void TogglePaused()
        {
            if (this.State == GameState.Paused)
            {
                this.State = GameState.Running;
            }
            else if (this.State == GameState.Running)
            {
                this.State = GameState.Paused;
            }
        }

        /// <summary>
        /// Handler that is triggered when a player has been eliminated.
        /// </summary>
        /// <param name="sender">The player that was eliminated.</param>
        /// <param name="e">Not used.</param>
        private void PlayerEliminated(object sender, EventArgs e)
        {
            this.EliminatePlayer(((Player)sender).Id);

            if (this.registeredPlayers.Count == 0)
            {
                this.State = GameState.GameOver;
            }
        }

        private void EliminatePlayer(PlayerIndex id)
        {
            Player player;

            if (this.registeredPlayers.TryGetValue(id, out player))
            {
                this.registeredPlayers.Remove(id);

                // Unhook the handler.
                player.PlayerEliminated -= this.PlayerEliminated;
            }
        }
    }
}