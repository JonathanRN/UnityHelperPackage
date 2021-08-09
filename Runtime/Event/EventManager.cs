using System.Collections.Generic;
using System.Linq;

namespace Jroynoel
{
	public class EventManager : Singleton<EventManager>
	{
		public HashSet<EventListener> eventListeners = new HashSet<EventListener>();
		private readonly Dictionary<string, object> lastEvents = new Dictionary<string, object>();

		public void EmitEvent(string eventName, object arg = null)
		{
			var listeners = eventListeners.ToArray();
			for (int i = 0; i < listeners.Length; i++)
			{
				// Caching the events beforehand in case the action is subscribing to another event
				var events = listeners[i].GetSubscribedEvents.ToArray();
				for (int j = 0; j < events.Length; j++)
				{
					if (events[j].EventName.Equals(eventName))
					{
						events[j].Action?.Invoke(arg);
					}
				}
			}
			lastEvents[eventName] = arg;
		}

		public bool TryGetLastEvent(string eventName, out object arg)
		{
			arg = null;
			try
			{
				arg = lastEvents[eventName];
				return true;
			}
			catch (System.Exception)
			{
				return false;
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