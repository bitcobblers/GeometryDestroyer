using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust
{
    /// <summary>
    /// Defines an emitter for particles.
    /// </summary>
    public class ParticleEmitter
    {
        private readonly Model model;
        private readonly Vector3 position;
        private readonly Color color;
        private readonly EmitterDescription description;
        private readonly LinkedList<Particle> particles = new LinkedList<Particle>();
        private readonly int numParticles;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitter" /> class.
        /// </summary>
        public ParticleEmitter(EmitterDescription description, Color color, ContentManager content, Vector3 position, int numParticles)
        {
            this.model = content.Load<Model>(description.ModelName);
            this.description = description;
            this.color = color;
            this.position = position;
            this.numParticles = numParticles;

            for (int i = 0; i < numParticles; i++)
            {
                this.particles.AddFirst(this.NewParticle());
            }
        }

        /// <summary>
        /// Gets a collection of particles from the emitter.
        /// </summary>
        public IEnumerable<Particle> Particles
        {
            get
            {
                for (int i = 0; i < this.numParticles; i++)
                {
                    yield return this.NewParticle();
                }
            }
        }

        /// <summary>
        /// Creates a new particle for the emitter.
        /// </summary>
        /// <returns>A new particle.</returns>
        private Particle NewParticle()
        {
            return new Particle(this.description, this.color, this.model, new Vector3(this.position.X, this.position.Y, 0));
        }
    }
}