using System;
using System.Collections.Generic;

namespace Jroynoel
{
	public struct Event
	{
		public readonly string EventName;
		public readonly Action<object> Action;

		public Event(string eventName, Action<object> action)
		{
			EventName = eventName;
			Action = action;
		}
	}

	public class EventComparer : IEqualityComparer<Event>
	{
		public bool Equals(Event x, Event y)
		{
			return x.EventName.Equals(y.EventName);
		}

		public int GetHashCode(Event obj)
		{
			return obj.GetHashCode();
		}
	}
}