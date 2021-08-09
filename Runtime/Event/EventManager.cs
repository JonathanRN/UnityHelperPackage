﻿using System.Collections.Generic;
using System.Linq;

namespace Jroynoel
{
	public class EventManager : Singleton<EventManager>
	{
		public HashSet<EventListener> eventListeners = new HashSet<EventListener>();
		private readonly Dictionary<string, object> lastEvents = new Dictionary<string, object>();

		public void EmitEvent(string eventName, object arg = null)
		{
			foreach (var listener in eventListeners)
			{
				// Caching the events beforehand in case the action is subscribing to another event
				var events = listener.GetSubscribedEvents.ToArray();
				for (int i = 0; i < events.Length; i++)
				{
					if (events[i].EventName.Equals(eventName))
					{
						events[i].Action?.Invoke(arg);
					}
				}
			}
			lastEvents[eventName] = arg;
		}

		public object GetLastEvent(string eventName)
		{
			try
			{
				return lastEvents[eventName];
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public void RegisterListener(EventListener listener)
		{
			eventListeners.Add(listener);
		}

		public void UnregisterListener(EventListener listener)
		{
			eventListeners.Remove(listener);
		}
	}
}