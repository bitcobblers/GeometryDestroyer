using System;
using GeometryDestroyer.Parts.Impl.Guns;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    public class GunSystem : IGunSystem
    {
        private readonly Random rnd = new Random();
        private readonly Func<Player, Gun>[] allGuns;

        /// <summary>
        /// Initializes a new instance of the <see cref="GunSystem" /> class.
        /// </summary>
        public GunSystem()
        {
            this.allGuns = new Func<Player, Gun>[]
            {
                p => new ScatterGun(p),
                p => new ConcentratedGun(p)
            };
        }

        /// <inheritdoc />
        public Gun GetGun(Player owner) => this.allGuns[rnd.Next(0, this.allGuns.Length)](owner);
    }
}