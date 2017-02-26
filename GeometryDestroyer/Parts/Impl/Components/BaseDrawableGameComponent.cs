using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Components
{
    /// <summary>
    /// Defines a base draawable component that can suppress updates based on the game state.
    /// </summary>
    public class BaseDrawableGameComponent : DrawableGameComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDrawableGameComponent" /> class.
        /// </summary>
        /// <param name="game">THe game object.</param>
        public BaseDrawableGameComponent(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BaseDrawableGameComponent"/> class.
        /// </summary>
        ~BaseDrawableGameComponent()
        {
            this.Dispose(disposing: false);
        }

        /// <summary>
        /// Gets the game system to use.
        /// </summary>
        public IGameSystem GameSystem { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the update method should be suppressed.
        /// </summary>
        public bool SuppressUpdate { get; private set; }

        /// <inheritdoc />
        public override void Initialize()
        {
            this.GameSystem = ServiceLocator.Get<IGameSystem>();
            this.GameSystem.StateChanged += GameSystem_StateChanged;
        }

        /// <summary>
        /// Triggered whenever the state of the game changes.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The new state of the game.</param>
        private void GameSystem_StateChanged(object sender, GameState e)
        {
            this.SuppressUpdate = (e != GameState.Running);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.GameSystem.StateChanged -= this.GameSystem_StateChanged;
            }

            base.Dispose(disposing);
        }
    }
}