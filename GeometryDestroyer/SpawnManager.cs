using System;
using System.Collections.Generic;
using System.Linq;
using GeometryHolocaust.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryHolocaust
{
    /// <summary>
    /// Defines a manager used to spawn enemies by the director.
    /// </summary>
    public class SpawnManager
    {
        private readonly Random rnd = new Random();
        private readonly Dictionary<EnemyType, Func<Vector3, Enemy>> enemyMap;
        private readonly EnemyType[] enemyKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnManager" /> class.
        /// </summary>
        /// <param name="content">The content manager to load models from.</param>
        public SpawnManager(ContentManager content)
        {
            this.enemyMap = new Dictionary<EnemyType, Func<Vector3, Enemy>>
            {
                [EnemyType.Pinwheel] = p => new Pinwheel(content.Load<Model>("Models/pinwheel"), p),
                [EnemyType.Spiral] = p => new Spiral(content.Load<Model>("Models/spiral"), p)
            };

            this.enemyKeys = this.enemyMap.Keys.ToArray();
        }

        /// <summary>
        /// Spawns an enemy.
        /// </summary>
        /// <param name="type">The type of enemy to spawn.</param>
        /// <param name="position">The position to spawn the enemy in.</param>
        /// <returns></returns>
        public Enemy Spawn(EnemyType type, Vector3 position)
        {
            if (type == EnemyType.Any)
            {
                throw new InvalidOperationException("An enemy cannot be spawned of the 'Any' type.");
            }
            else
            {
                Func<Vector3, Enemy> handler;

                if (this.enemyMap.TryGetValue(type, out handler))
                {
                    return handler(position);
                }
                else
                {
                    throw new InvalidOperationException("The spcified enemy type cannot be found.");
                }
            }
        }

        /// <summary>
        /// Spawns a random enemy.
        /// </summary>
        /// <param name="filter">A filter of enemy types to spawn.</param>
        /// <param name="position">The position to spawn the enemy in.</param>
        /// <returns>The spawned enemy.</returns>
        public Enemy Random(EnemyType filter, Vector3 position)
        {
            do
            {
                var keyIndex = this.rnd.Next(0, this.enemyKeys.Length);
                var key = this.enemyKeys[keyIndex];

                if (filter == EnemyType.Any || filter.HasFlag(key))
                {
                    return this.enemyMap[key](position);
                }

            } while (true);
        }
    }
}