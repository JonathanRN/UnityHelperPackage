using System;
using UnityEngine;

namespace Jroynoel
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

		public static T Instance => LazyInstance.Value;

		private static T CreateSingleton()
		{
			var ownerObject = new GameObject(typeof(T).Name);
			var instance = ownerObject.AddComponent<T>();
			try
			{
				DontDestroyOnLoad(ownerObject);
			}
			catch (InvalidOperationException)
			{
				// Catch specific exception so test runner passes
			}
			return instance;
		}
	}
}