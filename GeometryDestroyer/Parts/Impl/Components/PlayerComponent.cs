using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Components
{
    public class PlayerComponent : BaseDrawableGameComponent, IPlayerComponent
    {
        private readonly List<Player> players = new List<Player>();
        private Model playerModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerComponent"/> class.
        /// </summary>
        /// <param name="game">The game object.</param>
        public PlayerComponent(Game game) : base(game)
        {
            ServiceLocator.Register<IPlayerComponent>(this);
        }

        public IListSystem ListSystem { get; private set; }
        public IControllerSystem ControllerSystem { get; private set; }
        public IGunSystem GunSystem { get; private set; }
        public IDirectorComponent DirectorSystem { get; private set; }

        /// <inheritdoc />
        public IEnumerable<Player> Players => this.players;

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();

            this.playerModel = this.Game.Content.Load<Model>("Models/Player");
            this.ListSystem = ServiceLocator.Get<IListSystem>();
            this.ControllerSystem = ServiceLocator.Get<IControllerSystem>();
            this.GunSystem = ServiceLocator.Get<IGunSystem>();
            this.DirectorSystem = ServiceLocator.Get<IDirectorComponent>();
            this.GameSystem.GameReset += this.Reset;
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            this.ListSystem.UpdateCollection(gameTime, this.players);
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            this.ListSystem.DrawCollection(gameTime, this.players);
        }

        /// <inheritdoc />
        public void Reset(object sender, EventArgs e)
        {
            this.players.Clear();
            this.players.AddRange(this.ControllerSystem.GetControllers().Select(c => new Player(c, this.playerModel)));
        }
    }
}