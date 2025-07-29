#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSoundAdder
{
	[MenuItem("Tools/Add UIButtonSound To All Buttons In Scene")]
	private static void AddUIButtonSound()
	{
		Button[] buttons = GameObject.FindObjectsOfType<Button>(true);

		foreach (var button in buttons)
		{
			if (button.GetComponent<UIButtonSound>() == null)
			{
				button.gameObject.AddComponent<UIButtonSound>();
			}
		}

		Debug.Log($"Added UIButtonSound to {buttons.Length} buttons.");
	}
}
#endif