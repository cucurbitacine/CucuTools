using CucuTools.Avatar;
using UnityEngine;

namespace Example.Scripts
{
    public class BodyScaler : MonoBehaviour
    {
        public bool scaleXZ = false;

        public Transform body;
        public CucuAvatar avatar;

        private void Update()
        {
            if (scaleXZ)
            {
                body.localScale = new Vector3(2 * avatar.CharacterSetting.radius, avatar.CharacterSetting.heightScale,
                    2 * avatar.CharacterSetting.radius);
            }
            else
            {
                body.localScale = new Vector3(1, avatar.CharacterSetting.heightScale, 1);
            }
        }
    }
}