using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.NodeSystem
{
    [DisallowMultipleComponent]
    public class StateMachineRunner : MonoBehaviour
    {
        [Header("Runner")]
        [SerializeField] private StateNode _firstNode;
        [SerializeField] private bool _isRunning;
        
        [Space]
        [SerializeField] private bool _autoRun;
        [SerializeField] private Dictionary<SlotRef, GameObject> _slots = new Dictionary<SlotRef, GameObject>();

        private INode _actualNode;
        
        public bool TryGetSlot(SlotRef slotRef, out GameObject slot)
        {
            return _slots.TryGetValue(slotRef, out slot);
        }
        
        #region Runner

        private void IsRunning(bool isRunning) => _isRunning = isRunning;
        
        public bool IsRunning() => _isRunning;
        public bool AutoRun() => _autoRun;
        public void AutoRun(bool autoRun) => _autoRun = autoRun;

        public void RunNode()
        {
            if (IsRunning()) return;
            
            IsRunning(true);

            OnRunNode();

            _actualNode = _firstNode;
            _actualNode.EnterNode(null);
        }

        public void StopNode()
        {
            if (!IsRunning()) return;
            
            _actualNode.ExitNode(null);
            _actualNode = null;
            
            OnStopNode();
            
            IsRunning(false);
        }
        
        public INode GetActualNode()
        {
            return _actualNode;
        }

        public void SetFirstNode(StateNode firstNode)
        {
            _firstNode = firstNode;
        }
        
        protected virtual void OnRunNode()
        {
            foreach (var item in GetComponentsInChildren<IRunProcess>())
            {
                item.OnStartRun(this);
            }
        }

        protected virtual void OnStopNode()
        {
            foreach (var item in GetComponentsInChildren<IRunProcess>())
            {
                item.OnStopRun(this);
            }
        }
        
        #endregion

        #region Context

        private readonly Dictionary<Type, object> _container = new Dictionary<Type, object>();
        
        public bool Contains(Type type)
        {
            return _container.ContainsKey(type);
        }
        
        public void Bind(Type type, object t)
        {
            _container[type] = t;
        }
        
        public object Unbind(Type type)
        {
            return _container.Remove(type, out var value) ? value : null;
        }
        
        public object Resolve(Type type)
        {
            if (!_container.TryGetValue(type, out var value)) return null;

            if (value != null) return value;
            
            _container.Remove(type);
            return null;
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }
        
        public void Bind<T>(T t)
        {
            Bind(typeof(T), t);
        }
        
        public T Unbind<T>()
        {
            return (T)Unbind(typeof(T));
        }
        
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
        
        #endregion

        #region MonoBehaviour

        private void Start()
        {
            if (AutoRun()) RunNode();
        }
        
        private void Update()
        {
            if (IsRunning())
            {
                if (_actualNode.TryGetNextNode(out var nextNode))
                {
                    var previousNode = _actualNode;
                
                    _actualNode.ExitNode(nextNode);
                    _actualNode = nextNode;
                    _actualNode.EnterNode(previousNode);
                }
                else
                {
                    _actualNode.UpdateNode(Time.deltaTime);
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (IsRunning())
            {
                _actualNode.FixedUpdateNode(Time.fixedDeltaTime);
            }
        }
        
        private void OnDestroy()
        {
            StopNode();
        }

        #endregion
    }
}