using System;
using System.Collections;
using System.Collections.Generic;
using ObjectEventList = System.Collections.Generic.List<(object obj, System.Action<object, object> action)>;

namespace Jroynoel
{
	public static class EventManager
	{
		private static readonly Dictionary<string, ObjectEventList> dict = new Dictionary<string, ObjectEventList>();

		public static void SubscribeTo(this object obj, string eventName, Action<object, object> handler)
		{
			if (!dict.ContainsKey(eventName))
			{
				dict.Add(eventName, new ObjectEventList());
				UnityEngine.Debug.Log($"[EventManager] Added new event \"{eventName}\".");
			}
			dict[eventName].Add((obj, handler));
		}

		public static void UnsubscribeFrom(this object obj, string eventName)
		{
			if (dict.TryGetValue(eventName, out ObjectEventList observers))
			{
				observers.RemoveAll(x => x.obj.Equals(obj));

				if (observers.Count == 0)
				{
					dict.Remove(eventName);
					UnityEngine.Debug.Log($"[EventManager] Removed unsubscribed event \"{eventName}\".");
				}
			}
			else
			{
				throw new NullReferenceException($"Couldn't remove observer from {obj.GetType().Name}. Event \"{eventName}\" not found.");
			}
		}

		public static void Emit(this object obj, string eventName, object args = null)
		{
			if (!dict.ContainsKey(eventName))
			{
				throw new NullReferenceException($"Couldn't emit from {obj.GetType().Name}. Event \"{eventName}\" not found.");
			}

			foreach (var observer in dict[eventName])
			{
				observer.action?.Invoke(obj, args);
			}
		}

		public static IEnumerator WaitForEvent(this object obj, string eventName)
		{
			bool done = false;
			obj.SubscribeTo(eventName, (sender, args) =>
			{
				done = true;
			});

			yield return new UnityEngine.WaitUntil(() => done);
			obj.UnsubscribeFrom(eventName);
		}
	}
}