using System;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    public class GameSystem : IGameSystem
    {
        /// <inheritdoc />
        public event EventHandler GameReset;

        /// <inheritdoc />
        public event EventHandler<GameState> StateChanged;

        /// <inheritdoc />
        public GameState State { get; private set; }

        /// <inheritdoc />
        public void Reset()
        {
            this.GameReset?.Invoke(this, EventArgs.Empty);
            this.State = GameState.Running;
            this.StateChanged?.Invoke(this, this.State);
        }

        /// <inheritdoc />
        public void TogglePaused()
        {
            if(this.State == GameState.Paused)
            {
                this.State = GameState.Running;
                this.StateChanged(this, this.State);
            }
            else if(this.State == GameState.Running)
            {
                this.State = GameState.Paused;
                this.StateChanged(this, this.State);
            }
        }
    }
}