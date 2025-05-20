using UnityEngine;

/// <summary>
/// Use only for top-strech RectTransform
/// </summary>
public class SafeAreaTopAdjuster : MonoBehaviour
{
	void Start()
	{
		ApplyTopInset();
	}

	void ApplyTopInset()
	{
		Rect safe = Screen.safeArea;
		var rt = GetComponent<RectTransform>();
		var canvas = GetComponentInParent<Canvas>();
		float scale = canvas ? canvas.scaleFactor : 1f;

		float insetTop = (Screen.height - (safe.y + safe.height)) / scale;

		rt.offsetMax = new Vector2(rt.offsetMax.x, -insetTop);
	}
}