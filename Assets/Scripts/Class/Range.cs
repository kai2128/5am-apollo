using UnityEngine;

namespace Class
{
    public class Range
    {
        public float min;
        public float max;

        public float GetRange()
        {
            return Random.Range(min, max);
        }
        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}