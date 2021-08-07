using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jroynoel
{
	[InitializeOnLoad]
	public static class GameViewButton
	{
		private readonly static VisualElement toolbar = new VisualElement();

		static GameViewButton()
		{
			toolbar.style.flexDirection = FlexDirection.Row;
			toolbar.style.alignItems = Align.FlexStart;
			toolbar.style.top = 30;

			EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
		}

		private static void EditorApplication_playModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingPlayMode)
			{
				toolbar.RemoveFromHierarchy();
			}
		}

		/// <summary>
		/// Add a button to the Game View with the text specified
		/// </summary>
		/// <param name="text">The text of the button</param>
		/// <param name="onClick">The callback event whenever the button is pressed</param>
		public static void Show(string text, Action onClick)
		{
			Assembly assembly = typeof(EditorWindow).Assembly;
			Type type = assembly.GetType("UnityEditor.GameView");
			var gameViews = Resources.FindObjectsOfTypeAll(type);

			foreach (EditorWindow gameView in gameViews)
			{
				var btn = new Button
				{
					text = text
				};
				btn.clicked += onClick;
				toolbar.Add(btn);

				gameView.rootVisualElement.Add(toolbar);
			}
		}
	}
}