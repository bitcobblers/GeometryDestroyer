namespace GeometryDestroyer.Parts
{
    public interface IMathSystem
    {
        /// <summary>
        /// Calculates the angle between a vector provided and (1,0).
        /// </summary>
        /// <param name="x">The x coordinate of the vector.</param>
        /// <param name="y">The y coordinate of the vector.</param>
        /// <returns>The angle of the vector.</returns>
        float AngleOf(float x, float y);
    }
}