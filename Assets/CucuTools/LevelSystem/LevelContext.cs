using System;
using System.Collections.Generic;

namespace CucuTools.LevelSystem
{
    public class LevelContext
    {
        private readonly Dictionary<Type, object> context = new Dictionary<Type, object>();

        public bool Contains(Type type)
        {
            return context.ContainsKey(type);
        }
        
        public void Bind(Type type, object t)
        {
            context[type] = t;
        }
        
        public object Unbind(Type type)
        {
            return context.Remove(type, out var value) ? value : null;
        }
        
        public object Resolve(Type type)
        {
            if (!context.TryGetValue(type, out var value)) return null;

            if (value != null) return value;
            
            context.Remove(type);
            return null;
        }
    }

    public static class LevelContextExtension
    {
        public static bool Contains<T>(this LevelContext context)
        {
            return context.Contains(typeof(T));
        }
        
        public static void Bind<T>(this LevelContext context, T t)
        {
            context.Bind(typeof(T), t);
        }
        
        public static T Unbind<T>(this LevelContext context)
        {
            return (T)context.Unbind(typeof(T));
        }
        
        public static T Resolve<T>(this LevelContext context)
        {
            return (T)context.Resolve(typeof(T));
        }
    }
}