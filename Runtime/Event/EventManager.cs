using System.Collections.Generic;

namespace Jroynoel
{
	public class EventManager : Singleton<EventManager>
	{
		public HashSet<EventListener> eventListeners = new HashSet<EventListener>();

		public void EmitEvent(string eventName, object arg = null)
		{
			foreach (var listener in eventListeners)
			{
				foreach (var ev in listener.GetSubscribedEvents)
				{
					if (ev.EventName.Equals(eventName))
					{
						if (ev.Action != null)
						{
							ev.Action(arg);
						}
					}
				}
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