using CucuTools.Avatar;
using CucuTools.Interactables;
using UnityEngine;

namespace Example.Scripts
{
    public class CucuAvatarInteractHandler : InteractHandlerBehaviour
    {
        [Header("Avatar Settings")]
        [SerializeField] private CucuAvatar cucuAvatar;
        [SerializeField] private float originOffset = 0f;
        
        public CucuAvatar CucuAvatar
        {
            get => cucuAvatar;
            set => cucuAvatar = value;
        }

        public float OriginOffset
        {
            get => originOffset;
            set => originOffset = value;
        }

        #region InteractHandlerBehaviour

        public override bool Pressed { get; protected set; }
        public override Vector3 OriginCast => CucuAvatar.Head.position + DirectionCast * OriginOffset;
        public override Vector3 DirectionCast => CucuAvatar.Head.forward;

        public override float MaxDistanceCast
        {
            get => base.MaxDistanceCast - OriginOffset;
            set => base.MaxDistanceCast = value + OriginOffset;
        }

        #endregion

        private void Update()
        {
            Pressed = Input.GetKey(KeyCode.Mouse0);
        }
    }
}