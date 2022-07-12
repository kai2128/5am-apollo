using UnityEngine;

namespace Utils
{
    public static class ExtensionMethods
    {
        public static void LookAtTarget(this Transform transform, Transform target)
        {
            Vector3 localScale = transform.localScale;
            if (transform.position.x >= target.position.x)
            {
                localScale.x = Mathf.Abs(localScale.x) * -1;
            }
            else if(transform.position.x <= target.position.x)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }
            transform.localScale = localScale;
        }

        public static Vector2 GetOppositeDirection(this Transform transform)
        {
            return transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        }
    }
}
