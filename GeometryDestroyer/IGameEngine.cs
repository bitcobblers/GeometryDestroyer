using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GeometryHolocaust.Enemies;

namespace GeometryHolocaust
{
    public enum GameState
    {
        /// <summary>
        /// Game engine has been loaded but hasn't been started.
        /// </summary>
        NotStarted,

        /// <summary>
        /// Game is in progress.
        /// </summary>
        Running,

        /// <summary>
        /// Game is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Game is over.
        /// </summary>
        GameOver
    }

    public interface IGameEngine
    {
        /// <summary>
        /// Gets the current state of the game.
        /// </summary>
        GameState State { get; }

        /// <summary>
        /// Gets the 2D boundary for the scene.
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// Gets the view matrix to use.
        /// </summary>
        Matrix View { get; }

        /// <summary>
        /// Gets the projection matrix to use.
        /// </summary>
        Matrix Projection { get; }

        /// <summary>
        /// Gets a collection of controllers in the game.
        /// </summary>
        IEnumerable<GameController> Controllers { get; }

        /// <summary>
        /// Gets a collection of players in the game.
        /// </summary>
        IEnumerable<Player> Players { get; }

        /// <summary>
        /// Gets a collection of enemies in the game.
        /// </summary>
        IEnumerable<Enemy> Enemies { get; }

        /// <summary>
        /// Gets a collection of projectiles in the game.
        /// </summary>
        IEnumerable<Projectile> Projectiles { get; }

        /// <summary>
        /// Adds an explosion to the screen.
        /// </summary>
        /// <param name="position">The position to start the explosion.</param>
        /// <param name="color">The color of the explosion.</param>
        void AddExplosion(Vector3 position, Color color, ExplosionSize size);

        /// <summary>
        /// Adds a projectile to the engine.
        /// </summary>
        /// <param name="owner">The player that owns the projectile.</param>
        /// <param name="position">The starting position for the projectile.</param>
        /// <param name="direction">The direction the projectile is traveling.</param>
        void AddProjectile(Player owner, Vector3 position, Vector2 direction);

        /// <summary>
        /// Adds an enemy to the engine.
        /// </summary>
        /// <param name="enemy">The enemy to add.</param>
        void AddEnemy(Enemy enemy);
    }
}