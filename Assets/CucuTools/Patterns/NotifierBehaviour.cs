using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Patterns
{
    public class NotifierBehaviour : CucuBehaviour, ICucuNotifier
    {
        #region SerializeField

        [SerializeField] private NotifierEntity notifier;

        #endregion

        #region NotifierSource

        protected ICucuNotifier Notifier => notifier ?? (notifier = new NotifierEntity());
        
        /// <summary>
        /// Subscribe
        /// </summary>
        /// <param name="subscriber"></param>
        public virtual void Subscribe(ObserverBehaviour subscriber)
        {
            Subscribe((ICucuObserver) subscriber);
        }
        
        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <param name="subscriber"></param>
        public virtual void Unsubscribe(ObserverBehaviour subscriber)
        {
            Unsubscribe((ICucuObserver) subscriber);
        }

        #endregion
        
        #region INotifier

        [CucuButton(group: "Notifier")]
        public void Notify()
        {
            Notifier.Notify();
        }

        public void Subscribe(ICucuObserver subscriber)
        {
            Notifier.Subscribe(subscriber);

            if (subscriber is ObserverBehaviour target)
                target.AddNotifier(this);
        }

        public void Unsubscribe(ICucuObserver subscriber)
        {
            Notifier.Unsubscribe(subscriber);

            if (subscriber is ObserverBehaviour target)
                target.RemoveNotifier(this);
        }

        #endregion
    }
}