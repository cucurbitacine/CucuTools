using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public static class SyncRigid
    {
        public static void Sync(Rigidbody rigid, Vector3 pos, Quaternion rot,  float maxVel = 512f, float maxAngVel = 512f)
        {
            SyncPosition(rigid, pos, maxVel);
            SyncRotation(rigid, rot, maxAngVel);
        }

        public static void SyncPosition(Rigidbody rigid, Vector3 pos, float maxVel = 512f)
        {
            rigid.velocity = CalcVelocity(rigid.position, pos, maxVel);
        }
        
        public static void SyncRotation(Rigidbody rigid, Quaternion rot, float maxAngVel = 512f)
        {
            rigid.angularVelocity = CalcAngularVelocity(rigid.rotation, rot, maxAngVel);
        }
                
        public static Vector3 CalcVelocity(Vector3 from, Vector3 to, float maxVelocity = 512f)
        {
            var dPos = to - from;
            return Vector3.ClampMagnitude(dPos / Time.fixedDeltaTime, maxVelocity);
        }
        
        public static Vector3 CalcAngularVelocity(Quaternion from, Quaternion to, float maxAngularVelocity = 512f)
        {
            var rotate = to * Quaternion.Inverse(from);
            rotate.ToAngleAxis(out var angle, out var axis);
            var dRot = axis * (angle * Mathf.Deg2Rad);

            return Vector3.ClampMagnitude(dRot / Time.fixedDeltaTime, maxAngularVelocity);
        }
    }
}