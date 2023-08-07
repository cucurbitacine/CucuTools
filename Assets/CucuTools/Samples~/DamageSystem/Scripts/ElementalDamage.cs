using System;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    [Serializable]
    public class ElementalDamage : Damage
    {
        [Space] public Elemental elemental;
    }

    public enum Elemental
    {
        Fire,
        Grass,
        Water,
    }
}