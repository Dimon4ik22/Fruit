using System;
using DG.Tweening;
using HyperCasualPack.Pickables;
using UnityEngine;

namespace HyperCasualPack
{
    public static class ArcadeIdleHelper
    {
        public static Vector3 GetPoint(int currentIndex, RowColumnHeight rowColumnHeight)
        {
            int columnIndex = currentIndex % rowColumnHeight.ColumnCount;
            int rowIndex = currentIndex / rowColumnHeight.ColumnCount % rowColumnHeight.RowCount;
            int heightIndex = currentIndex / (rowColumnHeight.RowCount * rowColumnHeight.ColumnCount);
            Vector3 targetPos = Vector3.up * (rowColumnHeight.HeightOffset * heightIndex) + Vector3.right * (columnIndex * rowColumnHeight.RowColumnOffset) +
                                Vector3.forward * (rowIndex * rowColumnHeight.RowColumnOffset);
            return targetPos;
        }

        public static void JumpOrganized(JumpOrganizedData jData)
        {
            Vector3 targetPos = GetPoint(jData.Index, jData.RowColumnHeight);
            Vector3 adjustedPos = jData.PivotPoint.TransformPoint(targetPos);
            jData.Item.DOJump(adjustedPos, 1f, 1, jData.Duration).SetRecyclable().SetAutoKill();
            jData.Item.DOLocalRotate(new Vector3(0f, 180f, 0f), jData.Duration).SetRecyclable().SetAutoKill();
        }
        public static void JumpOrganizedPrePositionSet(JumpOrganizedData jData,Transform startPos)
        {
            Vector3 targetPos = GetPoint(jData.Index, jData.RowColumnHeight);
            Vector3 adjustedPos = jData.PivotPoint.TransformPoint(targetPos);
            jData.Item.transform.position = startPos.transform.position;
            Run.After(0.5f, () =>
            {
                jData.Item.DOJump(adjustedPos, 1f, 1, jData.Duration).SetRecyclable().SetAutoKill();
                jData.Item.DOLocalRotate(new Vector3(0f, 180f, 0f), jData.Duration).SetRecyclable().SetAutoKill();
            });
        }

        public static void JumpOrganized(JumpOrganizedData jData, TweenCallback tweenCallback)
        {
            Vector3 targetPos = GetPoint(jData.Index, jData.RowColumnHeight);
            Vector3 adjustedPos = jData.PivotPoint.TransformPoint(targetPos);
            jData.Item.DOJump(adjustedPos, 1f, 1, jData.Duration).SetRecyclable().SetAutoKill();
            jData.Item.DOLocalRotate(new Vector3(0f, 180f, 0f), jData.Duration).SetRecyclable().SetAutoKill().OnComplete(tweenCallback);
        }

        public static void SpendResource(int requiredResource, int collectedResource, int myResource, out Tween resourceSpendingTween, float spendingSpeed,
            Ease resourceSpendEase, TweenCallback<int> onTweenUpdate)
        {
            int remainedMoney = requiredResource - collectedResource;
            int to = myResource >= remainedMoney ? requiredResource : collectedResource + myResource;
            resourceSpendingTween = DOVirtual.Int(collectedResource, to, (float)to / requiredResource * spendingSpeed, onTweenUpdate)
                .SetEase(resourceSpendEase).SetAutoKill().SetRecyclable();
        }

        public static void SetParentAndJump(this Transform transform, Transform to, Action onJumped)
        {
            transform.SetParent(to);
            transform.DOLocalJump(Vector3.zero, 1f, 1, 0.4f).SetRecyclable().SetAutoKill().OnComplete(() => onJumped?.Invoke());
            transform.DOScale(Vector3.kEpsilon, 0.4f).SetEase(Ease.InBack, 5f).SetRecyclable().SetAutoKill().OnComplete(() => transform.gameObject.SetActive(false));
        }

        public static Sequence Jump(this Transform transform, Transform targetPoint)
        {
            return transform.DOJump(targetPoint.position, 1f, 1, 1f).SetRecyclable().SetAutoKill();
        }

        public static Sequence Jump(this Transform transform, Transform targetPoint, float jumpPower, int numJumps, float duration)
        {
            return transform.DOJump(targetPoint.position, jumpPower, numJumps, duration).SetRecyclable().SetAutoKill();
        }

        public static void Jump(this Transform transform, Transform targetPoint, float jumpPower, int numJumps, float duration, TweenCallback onComplete)
        {
            transform.DOJump(targetPoint.position, jumpPower, numJumps, duration).SetRecyclable().SetAutoKill().OnComplete(onComplete);
        }

        public static void JumpToDisappearSlowly(this Transform transform, Transform targetPoint, float jumpPower, int numJumps, float duration)
        {
            Jump(transform, targetPoint, jumpPower, numJumps, duration).OnComplete(() => DisappearSlowly(transform));
        }

        public static void ShowSlowly(this Transform transform, Vector3 targetScale, float duration)
        {
            transform.DOScale(targetScale, duration);
        }
        
        public static void DisappearSlowly(Transform transform)
        {
            transform.DOScale(Vector3.one * Mathf.Epsilon, 0.2f).SetAutoKill().SetRecyclable().OnComplete(() => { transform.gameObject.SetActive(false); });
        }
        
        public static void DisappearSlowlyToPool(this Pickable pickable)
        {
            pickable.transform.DOScale(Vector3.one * Mathf.Epsilon, 0.2f).SetRecyclable().SetAutoKill().OnComplete(pickable.ReleasePool);
        }

        public static void MoveThenDissappearSlowlyToPool(this Pickable pickable, Transform movePos,float moveDuration)
        {
            pickable.transform.DOJump(movePos.position,1,1, moveDuration).OnComplete(() =>
            {
                DisappearSlowlyToPool(pickable);
            }).SetRecyclable().SetAutoKill();
        }

        public static void JumpToDisappearIntoPool(this Pickable pickable, Transform targetPoint, float jumpPower, int numJumps, float duration)
        {
            Jump(pickable.transform, targetPoint, jumpPower, numJumps, duration).SetAutoKill().SetRecyclable().OnComplete(() => DisappearSlowlyToPool(pickable));
        }

        public static void Clear(this Transform transform)
        {
            transform.DOKill();
        }
    }
}