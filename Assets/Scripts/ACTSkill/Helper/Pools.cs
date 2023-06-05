using System;
using System.Collections.Generic;
using ACTSkill;
using UnityEngine.Pool;

namespace ACTSkillDemo
{
    public static class Pools
    {
        public static ObjectPool<InjuredInfo> InjuredInfoPool { get; } = new ObjectPool<InjuredInfo>(
            () => new InjuredInfo(),
            null,
            obj => obj.Reset());
        
        private static readonly Dictionary<Type, object> actionHandlerPools = new Dictionary<Type, object>
        {
            {
                typeof(AttackActionHandler), new ObjectPool<AttackActionHandler>(
                    () => new AttackActionHandler(),
                    null,
                    null)
            },
            {
                typeof(InjuredActionHandler), new ObjectPool<InjuredActionHandler>(
                    () => new InjuredActionHandler(),
                    null,
                    null)
            },
            {
                typeof(HitActionHandler), new ObjectPool<HitActionHandler>(
                    () => new HitActionHandler(),
                    null,
                    null)
            },
            {
                typeof(VelocityActionHandler), new ObjectPool<VelocityActionHandler>(
                    () => new VelocityActionHandler(),
                    null,
                    null)
            },
            {
                typeof(ConditionActionHandler), new ObjectPool<ConditionActionHandler>(
                    () => new ConditionActionHandler(),
                    null,
                    null)
            },
        };

        public static T GetActionHandler<T>() where T : class, IActionHandler, new()
        {
            if (actionHandlerPools.TryGetValue(typeof(T), out var obj) && obj is ObjectPool<T> pool)
                return pool.Get();
            return new T();
        }

        public static void ReleaseActionHandler<T>(T handler) where T : class, IActionHandler, new()
        {
            if (actionHandlerPools.TryGetValue(typeof(T), out var obj) && obj is ObjectPool<T> pool)
                pool.Release(handler);
        }
    }
}
