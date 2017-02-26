using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Components
{
    public class PlayerComponent : BaseDrawableGameComponent, IPlayerComponent
    {
        private readonly static TimeSpan RespawnInterval = TimeSpan.FromSeconds(5);

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

        public IGameSystem GameSystem { get; private set; }
        public IControllerSystem ControllerSystem { get; private set; }
        public IGunSystem GunSystem { get; private set; }
        public IDirectorComponent DirectorSystem { get; private set; }

        /// <inheritdoc />
        public int AvailablePlayers => this.players.Count(p => p.LivesRemaining >= 0);

        /// <inheritdoc />
        public int ActivePlayers => this.players.Count(p => p.IsActive);

        /// <inheritdoc />
        public IEnumerable<Player> Players => this.players;

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();

            this.playerModel = this.Game.Content.Load<Model>("Models/Player");
            this.GameSystem = ServiceLocator.Get<IGameSystem>();
            this.ControllerSystem = ServiceLocator.Get<IControllerSystem>();
            this.GunSystem = ServiceLocator.Get<IGunSystem>();
            this.DirectorSystem = ServiceLocator.Get<IDirectorComponent>();
            this.GameSystem.GameReset += this.Reset;
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            if (this.SuppressUpdate)
            {
                return;
            }

            foreach (var player in this.players)
            {
                if (player.IsActive)
                {
                    player.Update(gameTime);
                }
                else if (player.LivesRemaining >= 0 && (DateTime.Now - player.KillTime) > RespawnInterval)
                {
                    player.IsActive = true;
                    player.Position = new Vector3(0, 0, 0);
                }
            }
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            foreach (var player in this.players)
            {
                if (player.IsActive)
                {
                    player.Draw(gameTime);
                }
            }
        }

        /// <inheritdoc />
        public void Reset(object sender, EventArgs e)
        {
            this.players.Clear();
            this.players.AddRange(this.ControllerSystem.GetControllers().Select(c => new Player(c, this.playerModel)));

            foreach(var player in this.players)
            {
                this.GameSystem.RegisterPlayer(player);
            }
        }
    }
}