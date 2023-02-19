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
			var instance = FindObjectOfType<T>();
			if (instance == null)
			{
				instance = new GameObject(typeof(T).Name).AddComponent<T>();
			}
			
			try
			{
				DontDestroyOnLoad(instance.gameObject);
			}
			catch (InvalidOperationException)
			{
				// Catch specific exception so test runner passes
			}

			return instance;
		}
	}
}