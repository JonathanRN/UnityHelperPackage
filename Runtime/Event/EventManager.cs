using System.Collections.Generic;

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
				foreach (var ev in listener.GetSubscribedEvents)
				{
					if (ev.EventName.Equals(eventName))
					{
						ev.Action?.Invoke(arg);
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