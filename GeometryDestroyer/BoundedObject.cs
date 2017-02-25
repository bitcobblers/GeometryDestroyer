using System.Collections.Generic;
using GeometryDestroyer.Parts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer
{
    /// <summary>
    /// Defines an object that is aware of the game boundaries.
    /// </summary>
    public class BoundedObject : ICanDraw, ICanUpdate
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedObject" /> class.
        /// </summary>
        /// <param name="model">The model for the object.</param>
        public BoundedObject(Model model)
        {
            this.Model = model;
            this.CameraSystem = ServiceLocator.Get<ICameraSystem>();
        }

        /// <summary>
        /// Gets the camera system to use.
        /// </summary>
        public ICameraSystem CameraSystem { get; }

        /// <summary>
        /// Gets or sets the model to draw.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Gets the bounding spheres for the model.
        /// </summary>
        public IList<BoundingSphere> Spheres { get; protected set; } = new List<BoundingSphere>();

        // <summary>
        // Gets or sets the position of the object.
        // </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the angle of ratation for the object.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets the world matrix for te object.
        /// </summary>
        public Matrix World { get; protected set; }

        /// <summary>
        /// Gets or sets the scale for the object.
        /// </summary>
        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        /// <inheritdoc />
        public virtual void Draw(GameTime gameTime)
        {
            foreach (var mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = this.World;
                    effect.View = this.CameraSystem.View;
                    effect.Projection = this.CameraSystem.Projection;
                }

                mesh.Draw();
            }
        }

        /// <inheritdoc />
        public virtual void Update(GameTime gameTime)
        {
            this.RecalculateWorld();
            this.RecalculateSpheres();
        }

        /// <summary>
        /// Tests whether two bounded objects intersect with each other.
        /// </summary>
        /// <param name="other">The object to check against.</param>
        /// <returns>True if the objects intersect.</returns>
        protected bool IntersectsWith(BoundedObject other)
        {
            foreach (var thisSphere in this.Spheres)
            {
                foreach (var otherSphere in other.Spheres)
                {
                    if (thisSphere.Intersects(otherSphere))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Recalcualtes the world matrix.
        /// </summary>
        protected virtual void RecalculateWorld()
        {
            this.World = Matrix.CreateScale(this.Scale) * Matrix.CreateRotationZ(this.Rotation) * Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0.0f);
        }

        /// <summary>
        /// Recalculates the bounding spheres for the object.
        /// </summary>
        protected virtual void RecalculateSpheres()
        {
            this.Spheres.Clear();

            foreach (var mesh in this.Model.Meshes)
            {
                this.Spheres.Add(mesh.BoundingSphere.Transform(this.World));
            }
        }

        /// <summary>
        /// Checks whether the object is within the bounded area.
        /// </summary>
        /// <param name="boundary">The boundary to check.</param>
        /// <returns>True if the objct is within the boundary.</returns>
        protected bool IsWithinBounds(Rectangle boundary) => boundary.Contains(this.Position.X, this.Position.Y);

        /// <summary>
        /// Calibrates the movement of an object, correctin the vector to ensure it stays within the given boundary.
        /// </summary>
        /// <param name="boundary">The boundary to confine the object to.</param>
        /// <param name="direction">The vector to move the object by.</param>
        /// <returns>An updated vector to translate the object by.</returns>
        protected Vector3 CalibrateMovement(Rectangle boundary, Vector3 direction)
        {
            if (this.IsWithinBounds(boundary) == true)
            {
                return direction;
            }
            else
            {
                float x = direction.X;
                float y = direction.Y;

                if ((Position.X < boundary.Left && direction.X < 0) ||
                    (Position.X > boundary.Right && direction.X > 0))
                {
                    x = 0.0f;
                }

                if ((Position.Y < boundary.Top && direction.Y < 0) ||
                    (Position.Y > boundary.Bottom && direction.Y > 0))
                {
                    y = 0.0f;
                }

                return new Vector3(x, y, direction.Z);
            }
        }

        /// <summary>
        /// Calibrates the movement of an object and provides a 'bounce' if the object strikes the edge of the boundary.
        /// </summary>
        /// <param name="boundary">The boundary to confine the object to.</param>
        /// <param name="direction">A vector the object is moving in.</param>
        /// <returns>An updated vector to translate the object by.</returns>
        protected Vector3 BounceMovement(Rectangle boundary, Vector3 direction)
        {
            if (this.IsWithinBounds(boundary) == true)
            {
                return direction;
            }
            else
            {
                float x = direction.X;
                float y = direction.Y;

                if ((Position.X < boundary.Left && direction.X < 0) ||
                    (Position.X > boundary.Right && direction.X > 0))
                {
                    x = -x;
                }

                if ((Position.Y < boundary.Top && direction.Y < 0) ||
                    (Position.Y > boundary.Bottom && direction.Y > 0))
                {
                    y = -y;
                }

                return new Vector3(x, y, direction.Z);
            }
        }
    }
}