using System.Collections;
using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class DodgeController2D : CucuBehaviour
    {
        public bool dodging = false;
        
        [Space]
        [Min(0)] public float dodgeSpeed = 20;
        [Min(0)] public float dodgeDuration = 0.1f;
        
        [Space] public PlayerController2D player2d;

        private Coroutine _dodging = default;
        
        public void Dodge(Vector2 direction)
        {
            if (_dodging != null) StopCoroutine(_dodging);
            _dodging = StartCoroutine(_Dodge(direction.normalized));
        }

        IEnumerator _Dodge(Vector2 direction)
        {
            dodging = true;
            
            var timer = 0f;
            while (timer < dodgeDuration)
            {
                player2d.rigidbody2d.velocity = direction * dodgeSpeed;
                timer += Time.deltaTime;

                yield return null;
            }
            
            dodging = false;
        }
    }
}