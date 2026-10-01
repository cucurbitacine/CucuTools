using System.Threading;

namespace CucuTools.DamageSystem
{
    public static class DamagePayloadTypeId<T> where T : IDamagePayload
    {
        public static readonly int Id = DamagePayloadTypeIdGenerator.Next();
    }
    
    public static class DamagePayloadTypeIdGenerator
    {
        private static int _nextId = 0;

        public static int Next() => Interlocked.Increment(ref _nextId);
    }
}