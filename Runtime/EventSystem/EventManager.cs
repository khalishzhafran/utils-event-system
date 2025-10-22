// ------------------------------------------------------------------------------
//  File: EventManager.cs
//  Author: Ran
//  Description: A static, type-based event management system.
//  Created: 2025
//  
//  Copyright (c) 2025 Ran.
//  This script is part of the ran.utilities namespace.
//  Permission is granted to use, modify, and distribute this file freely
//  for both personal and commercial projects, provided that this notice
//  remains intact.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ran.utilities
{
    /// <summary>
    ///     Interface to be implemented by all event classes.
    /// </summary>
    public interface IGameEvent { }

    /// <summary>
    ///     Manages event listeners and dispatches events to the appropriate listeners.
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        ///     A dictionary that holds the event type as the key and the associated event listener action as the value.
        /// </summary>
        private static readonly Dictionary<Type, Action<IGameEvent>> eventCollections = new();

        /// <summary>
        ///     A dictionary that maps event listener actions to their corresponding event handler actions.
        /// </summary>
        private static readonly Dictionary<Delegate, Action<IGameEvent>> eventLookups = new();

        /// <summary>
        ///     Adds a listener for a specific event type.
        /// </summary>
        /// <typeparam name="T">
        ///     The event type to listen for, must implement
        ///     <c>
        ///         <see cref="IGameEvent" />
        ///     </c>
        ///     .
        /// </typeparam>
        /// <param name="evt">The action to execute when the event is triggered.</param>
        public static void AddListener<T>(Action<T> evt) where T : IGameEvent
        {
            if (!eventLookups.ContainsKey(evt))
            {
                Action<IGameEvent> newAction = e => evt((T)e);
                eventLookups[evt] = newAction;

                if (eventCollections.TryGetValue(typeof(T), out Action<IGameEvent> existingAction))
                    eventCollections[typeof(T)] = existingAction += newAction;
                else
                    eventCollections[typeof(T)] = newAction;
            }
        }

        /// <summary>
        ///     Removes a listener for a specific event type.
        /// </summary>
        /// <typeparam name="T">
        ///     The event type to stop listening for, must implement
        ///     <c>
        ///         <see cref="IGameEvent" />
        ///     </c>
        ///     .
        /// </typeparam>
        /// <param name="evt">The action to remove from the event listener list.</param>
        public static void RemoveListener<T>(Action<T> evt) where T : IGameEvent
        {
            if (eventLookups.TryGetValue(evt, out Action<IGameEvent> action))
            {
                if (eventCollections.TryGetValue(typeof(T), out Action<IGameEvent> existingAction))
                {
                    existingAction -= action;
                    if (existingAction == null)
                        eventCollections.Remove(typeof(T));
                    else
                        eventCollections[typeof(T)] = existingAction;
                }

                eventLookups.Remove(evt);
            }
        }

        /// <summary>
        ///     Broadcasts an event to all listeners of that event type.
        /// </summary>
        /// <param name="evt">The event to broadcast.</param>
        public static void Broadcast(IGameEvent evt)
        {
            if (eventCollections.TryGetValue(evt.GetType(), out Action<IGameEvent> action))
                action.Invoke(evt);
        }

        /// <summary>
        ///     Clears all registered listeners and event collections.
        /// </summary>
        public static void Clear()
        {
            eventCollections.Clear();
            eventLookups.Clear();
        }
    }

}
