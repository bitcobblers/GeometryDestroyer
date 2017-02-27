using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GeometryDestroyer
{
    /// <summary>
    /// Defines a game controller for the game.
    /// </summary>
    /// <remarks>
    /// This is a simple wrapper to a gamepad.  It contains helper methods to enable state tracking of buttons.
    /// </remarks>
    public class GameController
    {
        private GamePadState previousState;
        private GamePadState currentState;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController" /> class.
        /// </summary>
        /// <param name="id">The id of the controller to track.</param>
        public GameController(PlayerIndex id)
        {
            this.Id = id;
            this.previousState = this.currentState = GamePad.GetState(id);
        }

        /// <summary>
        /// Gets the id of the controller.
        /// </summary>
        public PlayerIndex Id { get; }

        /// <summary>
        /// Gets the current state of the game input.
        /// </summary>
        public GamePadState State => this.currentState;

        /// <summary>
        /// Refreshes the controller state.
        /// </summary>
        public void Update()
        {
            this.previousState = this.currentState;
            this.currentState = GamePad.GetState(this.Id);
        }

        /// <summary>
        /// Checks whether a button has been pressed.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns>True if the button has been pressed.</returns>
        public bool IsKeyPressed(Buttons button) => this.currentState.IsButtonDown(button) && this.previousState.IsButtonUp(button);

        /// <summary>
        /// Checks whether the right trigger has been pressed.
        /// </summary>
        /// <returns>True if the right trigger has been pressed.</returns>
        public bool IsTriggerPressed() => this.currentState.Triggers.Right > 0 && this.previousState.Triggers.Right == 0;
    }
}