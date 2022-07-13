using UnityEngine;

namespace Utils
{
    public class Utils
    {
        public static Vector3 GetRandomDirection()
        {
            return new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0).normalized;
        }
        
        public static Vector2 GetRandomDirectionX2D()
        {
            return new Vector2(Random.Range(-1f,1f), 0).normalized;
        }
    }
}