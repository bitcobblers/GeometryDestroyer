using System;
using System.Collections.Generic;
using System.Linq;
using GeometryDestroyer.Parts.Impl.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    public class SpawnSystem : ISpawnSystem
    {
        private readonly Random rnd = new Random();
        private readonly Dictionary<EnemyType, Func<Vector3, Enemy>> enemyMap;
        private readonly EnemyType[] enemyKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnSystem" /> class.
        /// </summary>
        public SpawnSystem(ContentManager content)
        {
            this.enemyMap = new Dictionary<EnemyType, Func<Vector3, Enemy>>
            {
                [EnemyType.Pinwheel] = p => new Pinwheel(content.Load<Model>("Models/pinwheel"), p),
                [EnemyType.Spiral] = p => new Spiral(content.Load<Model>("Models/spiral"), p)
            };

            this.enemyKeys = this.enemyMap.Keys.ToArray();
        }

        /// <inheritdoc />
        public void Random(EnemyType filter, Vector3 position)
        {
            do
            {
                var keyIndex = this.rnd.Next(0, this.enemyKeys.Length);
                var key = this.enemyKeys[keyIndex];

                if (filter == EnemyType.Any || filter.HasFlag(key))
                {
                    this.Spawn(key, position);
                    break;
                }

            } while (true);
        }

        /// <inheritdoc />
        public void Spawn(EnemyType type, Vector3 position)
        {
            var enemySystem = ServiceLocator.Get<IEnemyComponent>();

            if (type == EnemyType.Any)
            {
                throw new InvalidOperationException("An enemy cannot be spawned of the 'Any' type.");
            }
            else
            {
                Func<Vector3, Enemy> handler;

                if (this.enemyMap.TryGetValue(type, out handler))
                {
                    enemySystem.AddEnemy(handler(position));
                }
                else
                {
                    throw new InvalidOperationException("The spcified enemy type cannot be found.");
                }
            }
        }
    }
}