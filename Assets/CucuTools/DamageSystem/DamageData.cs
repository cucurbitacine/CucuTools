using System.Collections.Generic;

namespace CucuTools.DamageSystem
{
    public sealed class DamageData
    {
        public int Amount { get; set; }
        
        private readonly Dictionary<int, IDamagePayload> payloads = new Dictionary<int, IDamagePayload>(8);

        public DamageData(int amount)
        {
            Amount = amount;
        }
        
        public void AddPayload<T>(T payload) where T : IDamagePayload
        {
            AddPayload(DamagePayloadTypeId<T>.Id, payload);
        }

        public bool TryGetPayload<T>(out T payload) where T : IDamagePayload
        {
            if (TryGetPayload(DamagePayloadTypeId<T>.Id, out var value))
            {
                payload = (T)value;
                return true;
            }

            payload = default;
            return false;
        }
        
        public bool RemovePayload<T>() where T : IDamagePayload
        {
            return RemovePayload(DamagePayloadTypeId<T>.Id);
        }
        
        public DamageData Copy()
        {
            var data = new DamageData(Amount);
            foreach (var pair in payloads)
            {
                data.AddPayload(pair.Key, pair.Value);
            }
            return data;
        }
        
        private void AddPayload(int id, IDamagePayload payload)
        {
            payloads[id] = payload;
        }
        
        private bool TryGetPayload(int id, out IDamagePayload payload)
        {
            return payloads.TryGetValue(id, out payload);
        }
        
        private bool RemovePayload(int id)
        {
            return payloads.Remove(id);
        }
    }

    public interface IDamagePayload
    {
    }
}