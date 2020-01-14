using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Unicorn.Utilities.Extensions
{
    public static class ExtensionMethods
    {
        public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object source, TEventArgs args) where TEventArgs : EventArgs
        {
            if (eventHandler == null)
                return;
            eventHandler(source, args);
        }

        public static void RaiseEvent(this EventHandler eventHandler, object source)
        {
            eventHandler.RaiseEvent(source, EventArgs.Empty);
        }

        public static void RaiseEvent(this EventHandler eventHandler, object source, EventArgs args)
        {
            if (eventHandler == null)
                return;
            eventHandler(source, args);
        }

        public static void RaiseEvent(this CancelEventHandler eventHandler, object source, CancelEventArgs args)
        {
            if (eventHandler == null)
                return;
            eventHandler(source, args);
        }

        public static void RaiseEvent(this PropertyChangedEventHandler eventHandler, object source, PropertyChangedEventArgs args)
        {
            if (eventHandler == null)
                return;
            eventHandler(source, args);
        }

        public static void RaiseEvent(this PropertyChangedEventHandler eventHandler, object source, string propertyName)
        {
            if (eventHandler == null)
                return;
            eventHandler(source, new PropertyChangedEventArgs(propertyName));
        }

        public static void RaiseEvent(this PropertyChangingEventHandler eventHandler, object source, PropertyChangingEventArgs args)
        {
            if (eventHandler == null)
                return;
            eventHandler(source, args);
        }

        public static void RaiseEvent(this PropertyChangingEventHandler eventHandler, object source, string propertyName)
        {
            if (eventHandler == null)
                return;
            eventHandler(source, new PropertyChangingEventArgs(propertyName));
        }

        public static void RaiseEvent(this NotifyCollectionChangedEventHandler eventHandler, object source, NotifyCollectionChangedEventArgs args)
        {
            if (eventHandler == null)
                return;
            eventHandler(source, args);
        }

    }

}
