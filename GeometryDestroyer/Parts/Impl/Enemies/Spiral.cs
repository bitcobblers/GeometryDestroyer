using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Enemies
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
            : base(model)
        {
            this.Position = position;
            this.Scale = new Vector3(3, 3, 3);
        }

        /// <inheritdoc />
        public override int Value => 500;

        /// <inheritdoc />
        public override int Health { get; set; } = 250;

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            this.Rotation += OneDegree;

            if (this.Rotation > MathHelper.TwoPi)
            {
                this.Rotation -= MathHelper.TwoPi;
            }

            // Update boundaries.
            base.Update(gameTime);
        }

        /// <inheritdoc />
        protected override void OnDestroyed()
        {
            this.ParticleComponent.AddExplosion(EmitterDescription.Explosion, this.Position, Color.SteelBlue, ExplosionSize.Huge);
        }
    }
}