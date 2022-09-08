using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Player
{
    public class PlayerGrowShrink : MonoBehaviour
    {
        // Start is called before the first frame update
        private Vector3 _growScaleTo = new Vector3(2f, 2f, 1f);
        private Vector3 _shrinkScaleTo = new Vector3(0.5f, 0.5f, 1f);
        private Vector3 _originalScale = new Vector3(1f, 1f, 1f);

        public bool growed = false;
        public bool shrinked = false;
        public Rigidbody2D rb;
        private TweenerCore<Vector3, Vector3, VectorOptions> growToTween;
        private TweenerCore<Vector3, Vector3, VectorOptions> growBackTween;
        private TweenerCore<Vector3, Vector3, VectorOptions> shrinkToTween;
        private TweenerCore<Vector3, Vector3, VectorOptions> shrinkBackTween;


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            // if (growed)
            // {
            //     gameObject.transform.localScale = new Vector3(2f, 2f, 1f);//modify local scale y manually
            // }
            // if (shrinked)
            // {
            //     gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);//modify local scale y manually
            // }
        }

        public void Grow()
        {
            var currentFacingDirection = transform.localScale.x < 0 ? -1 : 1;
            Vector3 scaleTo = new Vector3(_growScaleTo.x * currentFacingDirection, _growScaleTo.y, _growScaleTo.z);

            // DOVirtual.Vector3(new Vector3(1f, 1f, 1f), new Vector3(2f, 2f, 2f), 2f, () =>
            // {
            //     
            // });

            growToTween = transform.DOScale(scaleTo, 2.0f).OnComplete(() =>
            {
                growed = true;
                PlayerManager.Instance.GrowDmg(growed);
                PlayerManager.Instance.GrowMovementSpeed(growed);
                Vector3 scaleTo = new Vector3(_originalScale.x * currentFacingDirection, _originalScale.y,
                    _originalScale.z);
                growBackTween = transform.DOScale(scaleTo, 2.0f)
                    .SetDelay(15.0f) //will go back to original size after 15 seconds
                    .OnComplete(BackFromGrow);
            });
        }

        [ContextMenu("Reset cooldown")]
        public void ResetCooldown()
        {
            PlayerManager.Instance.playerAbilityUI.ResetCooldown();
            ResetGrowShrink();
        }

        public void ResetGrowShrink()
        {
            shrinkToTween.Kill();
            shrinkBackTween.Kill();
            growBackTween.Kill();
            growToTween.Kill();
            if (growed)
            {
                BackFromGrow();
            }

            if (shrinked)
            {
                BackFromShrink();
            }
        }

        void BackFromGrow()
        {
            growed = false;
            PlayerManager.Instance.GrowDmg(growed);
            PlayerManager.Instance.GrowMovementSpeed(growed);
        }

        void BackFromShrink()
        {
            shrinked = false;
            PlayerManager.Instance.ShrinkDmg(shrinked);
            PlayerManager.Instance.ShrinkMovementSpeed(shrinked);
        }

        public void Shrink()
        {
            var currentFacingDirection = transform.localScale.x < 0 ? -1 : 1;
            Vector3 scaleTo = new Vector3(_shrinkScaleTo.x * currentFacingDirection, _shrinkScaleTo.y, _shrinkScaleTo.z);
            shrinkToTween = transform.DOScale(_shrinkScaleTo, 2.0f).OnComplete(() =>
            {
                shrinked = true;
                PlayerManager.Instance.ShrinkDmg(shrinked);
                PlayerManager.Instance.ShrinkMovementSpeed(shrinked);
                Vector3 scaleTo = new Vector3(_originalScale.x * currentFacingDirection, _originalScale.y,
                    _originalScale.z);
                shrinkBackTween = transform.DOScale(scaleTo, 2.0f)
                    .SetDelay(15.0f)
                    .OnComplete(BackFromShrink);
            });
        }
    }

}
