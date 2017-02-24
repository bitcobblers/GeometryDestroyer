using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust
{
    public class PhysicsObject
    {
        /// <summary>
        /// Gravitational constant.
        /// </summary>
        private const float G = 6.674f;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicsObject" /> class.
        /// </summary>
        /// <param name="mass">The mass of the object.</param>
        public PhysicsObject(float mass)
        {
            this.Mass = mass;
        }

        /// <summary>
        /// Gets or sets the mass of the object.
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Gets or sets the position of the object.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the object.
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Applies a force to the object.
        /// </summary>
        /// <param name="force">The force vector to apply.</param>
        protected void ApplyForce(Vector3 force)
        {
            this.Velocity += force;
        }

        /// <summary>
        /// Pulls an object toward itself (or vice versa).
        /// </summary>
        /// <param name="obj">The object to pull.</param>
        protected void Pull(PhysicsObject obj)
        {
            obj.ApplyForce(GravitationalForce(this, obj));
        }

        /// <summary>
        /// Calculates the gravitational force between two objects and returns a vector from the smaller object to larger one.
        /// </summary>
        /// <param name="obj1">The first object to compare.</param>
        /// <param name="obj2">The second object to compare.</param>
        /// <returns>The resultant vector between two objects with gravity applied.</returns>
        private static Vector3 GravitationalForce(PhysicsObject obj1, PhysicsObject obj2)
        {
            var distance = Vector3.DistanceSquared(obj1.Position, obj2.Position);
            var force = (G * obj1.Mass * obj2.Mass) / distance;

            if (obj1.Mass > obj2.Mass)
            {
                return (obj1.Position - obj2.Position) * force;
            }
            else
            {
                return (obj2.Position - obj1.Position) * force;
            }
        }
    }

    /// <summary>
    /// Defines an object that is aware of the game boundaries.
    /// </summary>
    public class BoundedObject : PhysicsObject, ICanDraw, ICanMove
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedObject" /> class.
        /// </summary>
        /// <param name="model">The model for the object.</param>
        public BoundedObject(Model model, float mass)
            : base(mass)
        {
            this.Model = model;
        }

        /// <summary>
        /// Gets or sets the model to draw.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Gets the bounding spheres for the model.
        /// </summary>
        public IList<BoundingSphere> Spheres { get; protected set; } = new List<BoundingSphere>();

        /// <summary>
        /// Gets or sets the position of the object.
        /// </summary>
        //public Vector3 Position { get; set; }

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
        public virtual void Draw(IGameEngine engine, GameTime gameTime)
        {
            foreach (var mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = this.World;
                    effect.View = engine.View;
                    effect.Projection = engine.Projection;
                }

                mesh.Draw();
            }
        }

        /// <inheritdoc />
        public virtual void Move(IGameEngine engine, GameTime gameTime)
        {
            this.World = Matrix.CreateScale(this.Scale) * Matrix.CreateRotationZ(this.Rotation) * Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0.0f);
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