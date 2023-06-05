using System;
using System.Collections.Generic;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    public abstract class MachineController : MonoBehaviour, IMachineController
    {
        public MachineBehaviour MachineBehaviour => machineBehaviour;
        public Matrix4x4 LocalToWorld => gameObject.transform.localToWorldMatrix;
        public Dictionary<string, object> Datas { get; } = new Dictionary<string, object>();
        public bool TryGetData<T>(string key, out T value)
        {
            bool success;
            if (Datas.TryGetValue(key, out var obj) && obj is T v)
            {
                success = true;
                value = v;
            }
            else
            {
                success = false;
                value = default;
            }
            return success;
        }
        public T GetData<T>(string key, T defaultValue = default)
        {
            return TryGetData(key, out T value) ? value : defaultValue;
        }

        public void SetData<T>(string key, T value)
        {
            Datas[key] = value;
        }

        public void RemoveData(string key)
        {
            Datas.Remove(key);
        }

        [SerializeField]
        protected TextAsset configAsset;
        protected MachineBehaviour machineBehaviour;
        [SerializeField]
        protected int layer;
        public int Layer => layer;
        [SerializeField]
        protected LayerMask attackMask = -1;
        public LayerMask AttackMask => attackMask;
        public int PauseFrameCount { private set; get; }
        
        public Animator Animator { protected set; get; }
        public Rigidbody Rigidbody { protected set; get; }
        protected Dictionary<Type, List<Collider>> colliders = new Dictionary<Type, List<Collider>>();
        protected Dictionary<Type, Stack<Collider>> cachedColliders = new Dictionary<Type, Stack<Collider>>();
        protected Dictionary<Type, int> reuseColliderCount = new Dictionary<Type, int>();
        protected string toChangeAnimName;
        protected float toChangeAnimTransitionDuration;
        protected float toChangeAnimTimeOffset;
        protected float animatorUpdateTime;
        protected bool isPauseFrame;
        protected bool oriIsKenematic;
        protected Vector3 oriVelocity;

        protected virtual void Awake()
        {
            Datas.Add(MachineDataKeys.InjuredInfos, new Queue<InjuredInfo>());
            machineBehaviour = new MachineBehaviour(this, MachineHelper.DeserializeMachineConfig(configAsset.text));
            machineBehaviour.SetCanUpdateLogicHandler(CanUpdateLogic);
            machineBehaviour.OnLogicUpdated += OnLogicUpdated;
            machineBehaviour.StateHandler.OnStateChanged += OnStateChanged;
            machineBehaviour.StateHandler.OnStateUpdating += OnStateUpdating;
        }

        protected virtual void OnEnable()
        {
            machineBehaviour.OnEnable();
        }

        protected virtual void OnDisable()
        {
            machineBehaviour.OnDisable();
        }

        protected virtual void OnDestroy()
        {
            machineBehaviour.SetCanUpdateLogicHandler(null);
            machineBehaviour.OnLogicUpdated -= OnLogicUpdated;
            machineBehaviour.StateHandler.OnStateChanged -= OnStateChanged;
            machineBehaviour.StateHandler.OnStateUpdating -= OnStateUpdating;
        }

        protected virtual void Update()
        {
            //Separate logic and rendering
            if (!string.IsNullOrEmpty(toChangeAnimName))
            {
                Animator.CrossFadeInFixedTime(toChangeAnimName, toChangeAnimTransitionDuration, 0, toChangeAnimTimeOffset);
                toChangeAnimName = null;
                Animator.Update(0);
            }
            if (animatorUpdateTime >= Time.deltaTime)
            {
                animatorUpdateTime -= Time.deltaTime;
                Animator.Update(Time.deltaTime);
            }
        }

        protected virtual void FixedUpdate()
        {
            machineBehaviour.Update(Time.fixedDeltaTime);
        }

        protected virtual bool CanUpdateLogic()
        {
            bool isPause = PauseFrameCount > 0;
            if (isPause)
                PauseFrameCount--;
            if (isPause != isPauseFrame)
            {
                isPauseFrame = isPause;
                if (isPauseFrame)
                {
                    oriIsKenematic = Rigidbody.isKinematic;
                    oriVelocity = Rigidbody.velocity;
                    Rigidbody.isKinematic = true;
                    Rigidbody.velocity = Vector3.zero;
                }
                else
                {
                    Rigidbody.isKinematic = oriIsKenematic;
                    Rigidbody.velocity = oriVelocity;
                }
            }
            return !isPause;
        }
        
        protected virtual void OnStateChanged(string animName, float animTransitionDuration, float animTimeOffset)
        {
            toChangeAnimName = animName;
            toChangeAnimTransitionDuration = animTransitionDuration;
            toChangeAnimTimeOffset = animTimeOffset;
            animatorUpdateTime = 0;
        }
        
        protected virtual void OnStateUpdating()
        {
            if (machineBehaviour.StateHandler.TotalFrame > 0)
                animatorUpdateTime += machineBehaviour.LogicPerFrameTime;
            var rangeConfig = machineBehaviour.StateHandler.Config.GetBodyRange(machineBehaviour.StateHandler.Frame);
            if (rangeConfig != null)
            {
                foreach (var range in rangeConfig.Ranges)
                {
                    var collider = GetCollider(range.GetType());
                    if (!collider) continue;
                    collider.SetValueByRange(range);
                }   
            }
            foreach (var (type, list) in colliders)
            {
                if (!reuseColliderCount.TryGetValue(type, out var count))
                    count = 0;
                var removeCount = list.Count - count;
                if (removeCount > 0)
                {
                    if (!cachedColliders.TryGetValue(type, out var stack))
                    {
                        stack = new Stack<Collider>();
                        cachedColliders.Add(type, stack);
                    }

                    for (int i = 0; i < removeCount; i++)
                    {
                        var collider = list[count + i];
                        stack.Push(collider);
                        collider.gameObject.SetActive(false);
                    }
                    list.RemoveRange(count, removeCount);
                }
            }
            reuseColliderCount.Clear();
        }

        protected Collider GetCollider(Type type)
        {
            if (type == null) return null;
            Collider collider = null;
            if (colliders.TryGetValue(type, out var list) && list.Count > (reuseColliderCount.TryGetValue(type, out var count) ? count : 0))
            {
                collider = list[count];
                reuseColliderCount[type] = count + 1;
            }
            else if (cachedColliders.TryGetValue(type, out var stack) && stack.Count > 0)
            {
                collider = stack.Pop();
                collider.gameObject.SetActive(true);
            }
            else
            {
                var colliderGo = new GameObject(type.Name);
                colliderGo.layer = layer;
                colliderGo.transform.SetParent(transform, false);
                collider = colliderGo.AddColliderByRangeType(type);
                if (collider)
                {
                    if (list == null)
                    {
                        list = new List<Collider>();
                        colliders.Add(type, list);
                    }
                    list.Add(collider);
                    reuseColliderCount[type] = list.Count;
                }
            }
            return collider;
        }

        public void SetPauseFrameCount(int pauseFrameCount, bool stayMax)
        {
            PauseFrameCount = !stayMax ? pauseFrameCount : Mathf.Max(PauseFrameCount, pauseFrameCount);
        }
        
        public virtual void OnAttack(InjuredInfo injuredInfo)
        {
            PauseFrameCount = Mathf.Max(PauseFrameCount, injuredInfo.Config.PauseFrameCount);
        }

        public virtual void Injured(InjuredInfo injuredInfo)
        {
            GetData<Queue<InjuredInfo>>(MachineDataKeys.InjuredInfos).Enqueue(injuredInfo);
        }
        
        protected virtual void OnLogicUpdated() { }
    }
}
