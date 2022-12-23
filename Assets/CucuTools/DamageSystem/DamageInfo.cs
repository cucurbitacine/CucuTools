using System;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Basic information about damage. Customizable
    /// </summary>
    [Serializable]
    public struct DamageInfo
    {
        public int amount;
        public DamageType type;
        public DamageCrit crit;
    }

    public struct DamageCrit
    {
        public bool isOn;
        public int amount;
    }
    
    public enum DamageType
    {
        Physical,
        Fire,
        Water,
        Earth,
        Air,
        Lightning,
    }
}