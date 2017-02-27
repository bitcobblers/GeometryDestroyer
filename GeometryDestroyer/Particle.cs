using System;
using GeometryDestroyer.Parts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer
{
    /// <summary>
    /// Defines a single particle.
    /// </summary>
    public class Particle : BoundedObject, ICanUpdate, ICanDie
    {
        private static Random rnd = new Random();

        private readonly Vector3 color;
        private readonly Matrix positionMatrix;
        private readonly Matrix rotationMatrix;

        private readonly float moveBy;
        private readonly float scaleChange;

        private float offset = 0.0f;
        private float scaleFactor = 1.0f;
        private int timeToLive;

        /// <summary>
        /// Initializes a new instance of the <see cref="Particle" /> class.
        /// </summary>
        /// <param name="description">The description of the particle.</param>
        /// <param name="color">The color of the particle.</param>
        /// <param name="model">The model to draw.</param>
        /// <param name="position">The position to root the particle at.</param>
        public Particle(EmitterDescription description, Color color, Model model, Vector3 position)
            : base(model)
        {
            this.Position = position;
            this.moveBy = ((float)(rnd.NextDouble() / description.SpeedFactor) * (rnd.Next(0, 2) == 0 ? -1.0f : 1.0f)) * 2;

            float rotateX = MathHelper.ToRadians(rnd.Next(0, 360));
            float rotateY = MathHelper.ToRadians(rnd.Next(0, 360));
            float rotateZ = MathHelper.ToRadians(rnd.Next(0, 360));

            this.rotationMatrix = Matrix.CreateRotationX(rotateX) * Matrix.CreateRotationY(rotateY) * Matrix.CreateRotationZ(rotateZ);
            this.positionMatrix = Matrix.CreateTranslation(this.Position);

            this.timeToLive = rnd.Next(description.MinTimeToLive, description.MaxTimeToLive);
            this.color = color.ToVector3();
            this.scaleChange = 1.0f / this.timeToLive;
        }

        /// <inheritdoc />
        public bool IsAlive => this.timeToLive > 0;

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            this.timeToLive--;
            this.offset += this.moveBy;
            this.scaleFactor -= this.scaleChange;

            this.World = Matrix.CreateScale(this.scaleFactor) * Matrix.CreateTranslation(new Vector3(0.0f, this.offset, 0.0f)) * this.rotationMatrix * this.positionMatrix;
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            foreach (var mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = this.color;
                    effect.World = this.World;
                    effect.View = this.CameraSystem.View;
                    effect.Projection = this.CameraSystem.Projection;
                }

                mesh.Draw();
            }
        }
    }
}