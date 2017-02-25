using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    public class ListSystem : IListSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListSystem" /> class.
        /// </summary>
        /// <param name="gameSystem">The game system to use.</param>
        public ListSystem()
        {
        }

        /// <inheritdoc />
        public void UpdateCollection<T>(GameTime gameTime, IEnumerable<T> collection) where T : ICanUpdate
        {
            foreach (var item in collection)
            {
                item.Update(gameTime);
            }
        }

        /// <inheritdoc />
        public void DrawCollection<T>(GameTime gameTime, IEnumerable<T> collection) where T : ICanDraw
        {
            foreach (var item in collection)
            {
                item.Draw(gameTime);
            }
        }

        /// <inheritdoc />
        public void RemoveDeadCollection<T>(LinkedList<T> collection) where T : ICanDie
        {
            var node = collection.First;

            while (node != null)
            {
                var nextNode = node.Next;

                if (node.Value.IsAlive == false)
                {
                    collection.Remove(node);
                }

                node = nextNode;
            }
        }
    }
}