using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Patterns
{
    /// <summary>
    /// The object that will notify
    /// </summary>
    public interface ICucuNotifier : ICucuSubscriber<ICucuObserver>
    {
        /// <summary>
        /// Notify somebody
        /// </summary>
        void Notify();
    }

    /// <summary>
    /// The object to subscribe to
    /// </summary>
    /// <typeparam name="T">Type of subject that will be subscribed</typeparam>
    public interface ICucuSubscriber<in T>
    {
        /// <summary>
        /// Subscribe
        /// </summary>
        /// <param name="subscriber"></param>
        void Subscribe(T subscriber);
        
        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <param name="subscriber"></param>
        void Unsubscribe(T subscriber);
    }
    
    /// <summary>
    /// The subject will be notified
    /// </summary>
    public interface ICucuObserver
    {
        /// <summary>
        /// Notified by somebody
        /// </summary>
        void Notified();
    }

    /// <inheritdoc />
    [Serializable]
    public class NotifierEntity : ICucuNotifier
    {
        private readonly List<ICucuObserver> main = new List<ICucuObserver>();
        private readonly List<ICucuObserver> cache = new List<ICucuObserver>();

        private bool notifying;

        #region ICucuNotifier

        /// <inheritdoc />
        public void Notify()
        {
            // Protection from recursion call which happens by notified
            if (notifying) return;
            notifying = true;
            
            // Protection of main list which can modified by notified
            cache.Clear();
            cache.AddRange(main);
            
            try
            {
                // Safe notifying
                cache.ForEach(c => c?.Notified());
                
                // Clear from nulls
                main.RemoveAll(m => m == null);
            }
            catch (Exception exc)
            {
                Debug.LogError(exc.ToString());
            }
            finally
            {
                notifying = false;
            }
        }

        /// <inheritdoc />
        public void Subscribe(ICucuObserver subscriber)
        {
            if (!main.Contains(subscriber)) main.Add(subscriber);

            // Clear from nulls
            main.RemoveAll(m => m == null);
        }

        /// <inheritdoc />
        public void Unsubscribe(ICucuObserver subscriber)
        {
            if (main.Contains(subscriber)) main.Remove(subscriber);
            
            // Clear from nulls
            main.RemoveAll(m => m == null);
        }

        #endregion
    }

    /// <inheritdoc />
    [Serializable]
    public class ObserverEntity : ICucuObserver
    {
        [SerializeField] private UnityEvent onEventHandled;

        public UnityEvent OnEventHandled => onEventHandled ?? (onEventHandled = new UnityEvent());

        #region ICucuObserver

        /// <inheritdoc />
        public virtual void Notified()
        {
            OnEventHandled.Invoke();
        }

        #endregion
    }
}