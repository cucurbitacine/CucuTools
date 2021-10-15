using System.Collections.Generic;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Patterns
{
    /// <inheritdoc cref="CucuTools.Patterns.ICucuObserver" />
    public class ObserverBehaviour : CucuBehaviour, ICucuObserver
    {
        #region SerializeField

        [SerializeField] private ObserverEntity observer;

        #endregion

        #region NotifiableTarget

        protected ICucuObserver Observer => observer ?? (observer = new ObserverEntity());

        protected readonly List<ICucuNotifier> notifiers = new List<ICucuNotifier>();

        public virtual void AddNotifier(ICucuNotifier notifier)
        {
            if (!notifiers.Contains(notifier)) notifiers.Add(notifier);
        }

        public virtual void RemoveNotifier(ICucuNotifier notifier)
        {
            if (notifiers.Contains(notifier)) notifiers.RemoveAll(n => n == notifier);
        }

        #endregion
        
        #region INotifiable

        [CucuButton(group: "Observer")]
        public virtual void Notified()
        {
            Observer.Notified();
        }

        #endregion

        #region MonoBehaviour

        protected virtual void OnDestroy()
        {
            var cache = new List<ICucuNotifier>(notifiers);
            foreach (var notifier in cache)
                notifier?.Unsubscribe(this);
        }

        #endregion
    }
}