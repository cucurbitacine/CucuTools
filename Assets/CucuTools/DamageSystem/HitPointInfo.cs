using UnityEngine;

namespace CucuTools.DamageSystem
{
    public readonly struct HitPointInfo : IDamagePayload
    {
        public readonly Vector3 Point;
        public readonly Vector3 Normal;

        public HitPointInfo(Vector3 point, Vector3 normal)
        {
            Point = point;
            Normal = normal;
        }
    }
}