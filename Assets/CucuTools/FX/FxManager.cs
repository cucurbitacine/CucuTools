using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.FX
{
    public class FxManager : MonoBehaviour
    {
        private readonly Dictionary<BaseFx, List<BaseFx>> cache = new Dictionary<BaseFx, List<BaseFx>>();

        public List<BaseFx> GetList(BaseFx fx)
        {
            if (!cache.TryGetValue(fx, out var list))
            {
                list = new List<BaseFx>();
                cache.Add(fx, list);
            }
            else
            {
                list.RemoveAll(t => t == null);
            }

            return list;
        }

        public BaseFx GetOrCreate(BaseFx prefab)
        {
            var list = GetList(prefab);

            var available = list.Find(fx => fx.gameObject.activeSelf && !fx.isPlaying);

            if (available == null)
            {
                available = Instantiate(prefab);
                list.Add(available);
            }

            return available;
        }
    }
}