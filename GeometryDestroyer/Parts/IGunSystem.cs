using GeometryDestroyer.Parts.Impl.Guns;

namespace GeometryDestroyer.Parts
{
    public interface IGunSystem
    {
        /// <summary>
        /// Gets a random gun for the player to use.
        /// </summary>
        /// <param name="owner">The owner of the gun.</param>
        /// <returns>A new gun to use.</returns>
        Gun GetGun(Player owner);
    }
}