using System;

namespace GeometryDestroyer.Parts.Impl.Systems
{
    public class MathSystem : IMathSystem
    {
        /// <inheritdoc />
        public float AngleOf(float x, float y)
        {
            if (x == 1 && y == 0)
            {
                return 0.0f;
            }
            else if (x == 0 && y == 1)
            {
                return (float)(Math.PI / 2);
            }
            else if (x == -1 && y == 0)
            {
                return (float)Math.PI;
            }
            else if (x == 0 && y == -1)
            {
                return (float)(3 * Math.PI / 2);
            }
            else
            {
                var tan = y / x;

                if (x < 0)
                {
                    return (float)(Math.Atan(tan) + Math.PI);
                }
                else
                {
                    return (float)Math.Atan(tan);
                }
            }
        }
    }
}