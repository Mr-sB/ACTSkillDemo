using System;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    public static class RangeHelper
    {
        public delegate bool OverlapDelegate<T>(int count, ref T extraParam);
        
        public static Collider AddColliderByRangeType(this GameObject gameObject, Type rangeType)
        {
            if (!gameObject || rangeType == null) return null;
            if (rangeType == typeof(BoxRange))
                return gameObject.AddComponent<BoxCollider>();
            if (rangeType == typeof(SphereRange))
                return gameObject.AddComponent<SphereCollider>();
            return null;
        }

        public static void SetValueByRange(this Collider collider, IRange range)
        {
            if (!collider || range == null) return;
            var rotation = range.GetRotation();
            if (rotation.HasValue)
                collider.transform.localRotation = Quaternion.Euler(rotation.Value);
            switch (range)
            {
                case BoxRange boxRange:
                    if (collider is not BoxCollider boxCollider) break;
                    boxCollider.size = boxRange.Size;
                    boxCollider.center = boxRange.Offset;
                    break;
                case SphereRange sphereRange:
                    if (collider is not SphereCollider sphereCollider) break;
                    sphereCollider.radius = sphereRange.Radius;
                    sphereCollider.center = sphereRange.Offset;
                    break;
            }
        }

        public static void Overlap<T>(this RangeConfig rangeConfig, Matrix4x4 localToWorld, LayerMask mask, Collider[] colliders, OverlapDelegate<T> callback, ref T extraParam)
        {
            if (rangeConfig == null || rangeConfig.Ranges.Count == 0) return;
            foreach (var range in rangeConfig.Ranges)
            {
                int count = 0;
                switch (range)
                {
                    case BoxRange boxRange:
                        count = Physics.OverlapBoxNonAlloc(
                            localToWorld.MultiplyPoint3x4(boxRange.Offset),
                            Vector3.Scale(localToWorld.lossyScale, boxRange.Size * 0.5f),
                            colliders,
                            localToWorld.rotation * Quaternion.Euler(boxRange.Rotation),
                            mask);
                        break;
                    case SphereRange sphereRange:
                        count = Physics.OverlapSphereNonAlloc(
                            localToWorld.MultiplyPoint3x4(sphereRange.Offset),
                            sphereRange.Radius * localToWorld.lossyScale.x,
                            colliders,
                            mask);
                        break;
                }
                // Continue?
                if (!callback(count, ref extraParam))
                    break;
            }
        }
    }
}
