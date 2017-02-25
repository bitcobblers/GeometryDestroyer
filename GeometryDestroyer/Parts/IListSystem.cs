using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GeometryDestroyer.Parts
{
    /// <summary>
    /// Defines a system to simplify common list processing tasks.
    /// </summary>
    public interface IListSystem
    {
        /// <summary>
        /// Processes a collection and updates each item.
        /// </summary>
        /// <typeparam name="T">The type of collection to process.</typeparam>
        /// <param name="gameTime">The time in the game.</param>
        /// <param name="collection">The collection to update.</param>
        void UpdateCollection<T>(GameTime gameTime, IEnumerable<T> collection) where T : ICanUpdate;

        /// <summary>
        /// Processes a collection and draws each item.
        /// </summary>
        /// <typeparam name="T">The type of collection to process.</typeparam>
        /// <param name="gameTime">The time in the game.</param>
        /// <param name="collection">The collection to draw.</param>
        void DrawCollection<T>(GameTime gameTime, IEnumerable<T> collection) where T : ICanDraw;

        /// <summary>
        /// Processes a collection and removes any 'dead' items.
        /// </summary>
        /// <typeparam name="T">The type of collection to process.</typeparam>
        /// <param name="collection">The collection to process.</param>
        void RemoveDeadCollection<T>(LinkedList<T> collection) where T : ICanDie;
    }
}