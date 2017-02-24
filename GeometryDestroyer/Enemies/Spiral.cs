using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust.Enemies
{
    /// <summary>
    /// Defines a spiral enemy.  Spirals are stationary and emit a gravity field that draws everything towards it.
    /// </summary>
    public class Spiral : Enemy
    {
        private static readonly float OneDegree = MathHelper.ToRadians(1.0f);

        /// <summary>
        /// Initializes a new instance of the <see cref="Spiral" /> class.
        /// </summary>
        public Spiral(Model model, Vector3 position)
            : base(model, 500.0f)
        {
            this.Position = position;
            this.Scale = new Vector3(3, 3, 3);
        }

        /// <inheritdoc />
        public override int Value => 500;

        /// <inheritdoc />
        public override int Health { get; set; } = 250;

        /// <inheritdoc />
        public override void Move(IGameEngine engine, GameTime gameTime)
        {
            this.Rotation += OneDegree;

            if(this.Rotation > MathHelper.TwoPi)
            {
                this.Rotation -= MathHelper.TwoPi;
            }

            base.Move(engine, gameTime);
        }

        /// <inheritdoc />
        public override void Update(IGameEngine engine, GameTime gameTime)
        {
        }

        /// <inheritdoc />
        public override void Die(IGameEngine engine)
        {
            engine.AddExplosion(this.Position, Color.SteelBlue, ExplosionSize.Huge);
        }
    }
}