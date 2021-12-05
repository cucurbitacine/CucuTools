using System.Collections.Generic;
using CucuTools;
using CucuTools.Avatar;
using UnityEngine;

namespace Example.Scripts
{
    public class GravityExample : MonoBehaviour
    {
        public GravitySetting CurrentSettings;

        private Dictionary<CucuAvatar, GravitySetting> avatars = new Dictionary<CucuAvatar, GravitySetting>();
        
        public void On(Collider other)
        {
            var avatar = other.GetComponent<CucuAvatar>();
            if (avatar == null) return;
            avatars.Add(avatar, avatar.GravitySetting);
            avatar.GravitySetting = CurrentSettings.CucuClone();
        }

        public void Off(Collider other)
        {
            var avatar = other.GetComponent<CucuAvatar>();
            if (avatar == null) return;
            if (avatars.TryGetValue(avatar, out var setting))
            {
                avatar.GravitySetting = setting;
                avatars.Remove(avatar);
            }
        }
    }
}
