using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryHolocaust.Enemies;
using Microsoft.Xna.Framework;

namespace GeometryHolocaust.Directors
{
    public class SpiralDirector : Director
    {
        private readonly Random rnd = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpiralDirector" /> class. 
        /// </summary>
        /// <param name="spawnManager">The spawn manager to use.</param>
        public SpiralDirector(SpawnManager spawnManager) 
            : base(spawnManager)
        {
        }

        /// <inheritdoc />
        public override int MinimumLevel => 5;

        /// <inheritdoc />
        public override void Run(IGameEngine engine)
        {
            int spawns = (int)(Math.Ceiling(this.Level / 5.0));

            for(int i=0; i<spawns; i++)
            {
                var x = rnd.Next(engine.Bounds.Left, engine.Bounds.Right);
                var y = rnd.Next(engine.Bounds.Top, engine.Bounds.Bottom);

                engine.AddEnemy(this.SpawnManager.Spawn(EnemyType.Spiral, new Vector3(x, y, 0)));
            }
        }
    }
}
