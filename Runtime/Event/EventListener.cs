﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jroynoel
{
	public class EventListener : MonoBehaviour
	{
		private readonly HashSet<Event> subscribedEvents = new HashSet<Event>();
		private bool isRegistered;

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

			subscribedEvents.Add(new Event(eventName, action));
			if (!isRegistered)
			{
				EventManager.Instance.RegisterListener(this);
				isRegistered = true;
			}
		}

		protected virtual void OnDestroy()
		{
			EventManager.Instance.UnregisterListener(this);
		}
	}
}