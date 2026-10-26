using UnityEngine;

namespace CucuTools.NodeSystem.Features
{
    public class SlotFeature : FeatureNode
    {
        [SerializeField] private SlotRef _slotRef;
        [SerializeField] private bool _targetActive;
        
        [Space]
        [SerializeField] private GameObject _slot;
        
        private StateMachineRunner _runner;
        
        public override void OnStartRun(StateMachineRunner runner)
        {
            base.OnStartRun(runner);

            _runner = runner;

            _runner.TryGetSlot(_slotRef, out _slot);
            
            _slot.SetActive(!_targetActive);
        }

        public override void OnEnterNode(INode node, INode previous)
        {
            if (TryGetSlotFeature(previous, out var feature))
            {
                if (feature.GetSlotRef() == GetSlotRef() && feature.GetTargetActive() == GetTargetActive())
                {
                    return;
                }
            }
            
            _slot.SetActive(GetTargetActive());
        }

        public override void OnExitNode(INode node, INode next)
        {
            if (TryGetSlotFeature(next, out var feature))
            {
                if (feature.GetSlotRef() == GetSlotRef() && feature.GetTargetActive() == GetTargetActive())
                {
                    return;
                }
            }

            if (_slot.Equals(null))
            {
                return;
            }
            
            _slot.SetActive(!GetTargetActive());
        }
        
        public override void OnUpdateNode(INode node, float deltaTime)
        {
        }

        public SlotRef GetSlotRef()
        {
            return _slotRef;
        }
        
        public GameObject GetSlot()
        {
            return _slot;
        }

        public bool GetTargetActive()
        {
            return _targetActive;
        }
        
        private static bool TryGetSlotFeature(INode node, out SlotFeature feature)
        {
            if (node == null)
            {
                feature = null;
                return false;
            }
            
            var features = node.GetFeatures();
            return features.TryGet<SlotFeature>(out feature);
        }
    }
}