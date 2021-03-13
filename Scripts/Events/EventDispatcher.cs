// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mineral
{
	/// <summary>
	/// A simple event dispatcher that works with <see cref="UnityEvent"/>s
	/// Allows passing of implementations of <see cref="IEvent"/>s in a decoupled way
	/// </summary>
	public class EventDispatcher
	{
		private Dictionary<Type, UnityEvent<object>> eventDictionary;
		private Dictionary<object, UnityAction<object>> actionDictionary;

		public EventDispatcher()
		{
			eventDictionary = new Dictionary<Type, UnityEvent<object>>();
			actionDictionary = new Dictionary<object, UnityAction<object>>();
		}

		/// <summary>
		/// Starts listening to events of that event type
		/// </summary>
		/// <typeparam name="T">The event type</typeparam>
		/// <param name="listener">The action to take when the event is fired</param>
		public void StartListening<T>(UnityAction<T> listener) where T : IEvent
		{
			Type type = typeof(T);
			if (!eventDictionary.ContainsKey(type))
			{
				eventDictionary.Add(type, new UnityEvent<object>());
			}

			UnityAction<object> newListener = l => listener((T)l);
			eventDictionary[type].AddListener(newListener);

			if (actionDictionary.ContainsKey(listener))
			{
				actionDictionary[listener] = newListener;
			}
			else
			{
				actionDictionary.Add(listener, newListener);
			}
		}

		/// <summary>
		/// Stops listening to events of that event type
		/// </summary>
		/// <typeparam name="T">The event type</typeparam>
		/// <param name="listener">The action that should no longer listen</param>
		public void StopListening<T>(UnityAction<T> listener) where T : IEvent
		{
			Type type = typeof(T);

			if (eventDictionary.ContainsKey(type) && actionDictionary.ContainsKey(listener))
			{
				eventDictionary[type].RemoveListener(actionDictionary[listener]);
				actionDictionary.Remove(listener);
				if (eventDictionary[type].GetPersistentEventCount() == 1)
				{
					eventDictionary.Remove(type);
				}
			}
		}

		/// <summary>
		/// Fires a new event, where every listening action will get invoked
		/// </summary>
		/// <typeparam name="T">The event type</typeparam>
		/// <param name="eventObject">The actual event object</param>
		public void Invoke<T>(object eventObject) where T : IEvent
		{
			Type type = typeof(T);
			if (eventDictionary.TryGetValue(type, out UnityEvent<object> thisEvent))
			{
				thisEvent.Invoke(eventObject);
			}
		}
	}
}