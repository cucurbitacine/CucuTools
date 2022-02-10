using UnityEngine;

namespace CucuTools.Terrains
{
    [CreateAssetMenu(fileName = nameof(NoiseSeedAsset), menuName = "Noises/Noise Seed")]
    public class NoiseSeedAsset : ScriptableObject
    {
        public NoiseSeed Seed = NoiseSeed.Default;
    }
}