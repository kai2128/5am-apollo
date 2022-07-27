using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class ExtensionMethods
    {
        public static Transform Clone(this Transform transform)
        {
            GameObject go = new GameObject();
            var pos = transform.position;
            go.transform.position.Set(pos.x, pos.y, pos.z);
            return go.transform;
        }

        public static void LookAtTarget(this Transform transform, Transform target)
        {
            Vector3 localScale = transform.localScale;
            if (transform.position.x >= target.position.x)
            {
                localScale.x = Mathf.Abs(localScale.x) * -1;
            }
            else if (transform.position.x <= target.position.x)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }

            transform.localScale = localScale;
        }

        public static void LookAtTarget(this Transform transform, Vector2 target)
        {
            Vector3 localScale = transform.localScale;
            if (transform.position.x >= target.x)
            {
                localScale.x = Mathf.Abs(localScale.x) * -1;
            }
            else if (transform.position.x <= target.x)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }

            transform.localScale = localScale;
        }

        public static Vector2 GetOppositeDirection(this Transform transform)
        {
            return transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        }

        public static Vector2 GetFacingDirection(this Transform transform)
        {
            return transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        }

        public static float GetFacingFloat(this Transform transform)
        {
            return transform.localScale.x < 0 ? -1 : 1;
        }

        public static bool IsFacingRight(this Transform transform)
        {
            return transform.GetFacingFloat() > 0;
        }

        public static bool IsFacingTarget(this Transform transform, Vector2 target)
        {
            // in front of target
            if (transform.position.x >= target.x && transform.IsFacingLeft())
            {
                return true;
            }

            // behind of target
            if (transform.position.x <= target.x && transform.IsFacingRight())
            {
                return true;
            }

            return false;
        }

        public static bool IsFacingLeft(this Transform transform)
        {
            return transform.GetFacingFloat() < 0;
        }

        // public static void FlipDirection(this Transform transform)
        // {
        //     var localScale = transform.localScale;
        //     transform.localScale.Set( localScale.x * -1, localScale.y, localScale.z);
        // }

        public static Vector3 GetRoamingPostition(this Vector3 pos, float minDis, float maxDis)
        {
            return pos + Utils.GetRandomDirection() * Random.Range(minDis, maxDis);
        }

        public static Vector2 GetRoamingPosition2D(this Vector3 pos, float minDis, float maxDis)
        {
            Vector3 randomPos = pos.GetRoamingPostition(minDis, maxDis);
            randomPos.y = pos.y;
            randomPos.z = 0;
            return randomPos;
        }

        public static Vector2 GetRoamingPosition2D(this Vector2 pos, float minDis, float maxDis)
        {
            Vector2 randomPos = pos + Utils.GetRandomDirectionX2D() * Random.Range(minDis, maxDis);
            randomPos.y = pos.y;
            return randomPos;
        }

        public static float GetCurrentPlayTime(this Animator anim)
        {
            var animInfo = anim.GetCurrentAnimatorStateInfo(0);
            return animInfo.normalizedTime;
        }

        public static bool HasPlayedOver(this Animator anim, float percentage = 1f, string stateName = null)
        {
            if (stateName != null && !anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                return false;

            return anim.GetCurrentPlayTime() >= percentage && !anim.IsInTransition(0);
        }

        public static void ResetAllTriggers(this Animator anim)
        {
            foreach (var trigger in anim.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                    anim.ResetTrigger(trigger.name);
            }
        }

        public static void BlinkWhite(this SpriteRenderer sr, float duration = .2f)
        {
            sr.BlinkColor(new Color(255f,255f,255f), duration);
        }

        public static void BlinkRed(this SpriteRenderer sr, float duration = 0.2f)
        {
            sr.BlinkColor(new Color(255f, 1f, 1f), duration);
        }

        public static void BlinkColor(this SpriteRenderer sr, Color color, float duration = 0.2f)
        {
            if(sr == null)
                return;
            sr.material.color = color;
            DOVirtual.Color(sr.material.color, new Color(1f, 1f, 1f), duration,
                (Color oriColor) =>
                {
                    if(sr == null)
                        return;
                    sr.material.color = oriColor;
                });
        }
        
        public static void BlinkColor(this TilemapRenderer tr, Color color, float duration = 0.2f)
        {
            if(tr == null)
                return;
            tr.material.color = color;
            DOVirtual.Color(tr.material.color, new Color(1f, 1f, 1f), duration,
                (Color oriColor) =>
                {
                    if(tr == null)
                        return;
                    tr.material.color = oriColor;
                });
        }

        public static T RandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T RandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
    }
}
