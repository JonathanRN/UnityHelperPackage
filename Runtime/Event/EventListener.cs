using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jroynoel
{
	public class EventListener : MonoBehaviour
	{
		private readonly HashSet<Event> subscribedEvents = new HashSet<Event>();
		private bool isRegistered;

		/// <summary>
		/// Automatically retrieve the last event sent upon subscription?
		/// </summary> 
		public bool SyncLastEvent { get; set; }

		public HashSet<Event> GetSubscribedEvents => subscribedEvents;

		/// <summary>
		/// Emit an event to the specified event name with optional parameter
		/// </summary>
		/// <param name="eventName">The event name to emit to</param>
		/// <param name="arg">The optional data to send to the event</param>
		public void Emit(string eventName, object arg = null)
		{
			EventManager.Instance.EmitEvent(eventName, arg);
		}

		/// <summary>
		/// Subscribe to an event specified by the event name
		/// </summary>
		/// <param name="eventName">The name of the event to be subscribed to</param>
		/// <param name="action">The callback action with optional parameter</param>
		public void SubscribeTo(string eventName, Action<object> action)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException($"Invalid Event Name \"{eventName}\".");
			}

			Register();

			Event ev = new Event(eventName, action);
			subscribedEvents.Add(ev);
			if (SyncLastEvent)
			{
				if (EventManager.Instance.TryGetLastEvent(eventName, out object arg))
				{
					ev.Action?.Invoke(arg);
				}
			}
		}

		/// <summary>
		/// Unsubscribe from an event specified
		/// </summary>
		/// <param name="eventName">The event name to unsubscribe from</param>
		public void UnsubscribeFrom(string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException($"Invalid Event Name \"{eventName}\".");
			}

			subscribedEvents.RemoveWhere(x => x.EventName.Equals(eventName));
		}

		protected virtual void OnDestroy()
		{
			EventManager.Instance.UnregisterListener(this);
		}

		private void Register()
		{
			if (!isRegistered)
			{
				EventManager.Instance.RegisterListener(this);
				isRegistered = true;
			}
		}
	}
}