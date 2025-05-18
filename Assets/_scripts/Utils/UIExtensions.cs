
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtensions
{
	/// <summary>
	/// Converts the <paramref name="fromRect"/>'s anchoredPosition from its local coordinate system
	/// to the local coordinate system of <paramref name="toRect"/>.
	/// 
	/// Under the hood, it finds the parent canvases via <see cref="GetParentCanvas()"/>, so you do not need
	/// to explicitly pass them.
	/// 
	/// You can also apply an optional offset in pixels to the final result in the <paramref name="toRect"/> coordinate space.
	/// </summary>
	/// <param name="fromRect">The RectTransform whose anchoredPosition will be transformed.</param>
	/// <param name="toRect">The target RectTransform in which we want to get local coordinates.</param>
	/// <param name="xOffset">Additional offset in the x-axis (in pixels), applied after conversion.</param>
	/// <param name="yOffset">Additional offset in the y-axis (in pixels), applied after conversion.</param>
	/// <returns>Local coordinates in <paramref name="toRect"/> (or Vector2.zero if something went wrong).</returns>
	public static Vector2 ConvertLocalRectToLocalRect(
		this RectTransform fromRect,
		RectTransform toRect,
		float xOffset = 0f,
		float yOffset = 0f
	)
	{
		if (fromRect == null || toRect == null)
			return Vector2.zero;

		var fromCanvas = fromRect.GetParentCanvas();
		var toCanvas = toRect.GetParentCanvas();
		if (fromCanvas == null || toCanvas == null)
			return Vector2.zero;

		// 1) Pivot world → screen
		var fromCam = fromCanvas.renderMode == RenderMode.ScreenSpaceOverlay
			? null
			: fromCanvas.worldCamera;
		Vector2 screenPt = RectTransformUtility.WorldToScreenPoint(fromCam, fromRect.position);

		// 2) Screen → local toRect
		var toCam = toCanvas.renderMode == RenderMode.ScreenSpaceOverlay
			? null
			: toCanvas.worldCamera;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			toRect, screenPt, toCam, out Vector2 localPt);

		// 3) optional offset
		localPt.x += xOffset;
		localPt.y += yOffset;
		return localPt;
	}

	public static Canvas GetParentCanvas(this Component component)
	{
		if (component == null) return null;
		return component.GetComponentInParent<Canvas>();
	}

	public static RectTransform GetRectTransform(this Button button) => button.GetComponent<RectTransform>();

	public static void SetRect(this RectTransform rt, Rect newRect)
	{
		rt.anchoredPosition = new Vector2(newRect.x, newRect.y);
		rt.sizeDelta = new Vector2(newRect.width, newRect.height);
	}

	public static int GetSpriteCharacterIndex(this TMP_Text textField, int spriteIndex)
	{
		string text = textField.text;

		string spriteTag = $"<sprite={spriteIndex}>";

		for (int i = 0; i < text.Length - spriteTag.Length; i++)
		{
			if (text.Substring(i, spriteTag.Length) == spriteTag)
			{
				return i;
			}
		}

		Debug.LogError($"Sprite with index {spriteIndex} not found.");
		return -1;
	}

	public static RectTransform CreateRectTransformForSymbol(this TMP_Text textField, int characterIndex, RectTransform parentRect, float padding = 20f)
	{
		TMP_TextInfo textInfo = textField.textInfo;
		if (characterIndex < 0 || characterIndex >= textInfo.characterCount)
		{
			Debug.LogError("Character index out of range.");
			return null;
		}

		TMP_CharacterInfo charInfo = textInfo.characterInfo[characterIndex];

		var topLeft = charInfo.topLeft;
		var bottomRight = charInfo.bottomRight;

		var worldTopLeft = parentRect.TransformPoint(topLeft);
		var worldBottomRight = parentRect.TransformPoint(bottomRight);
		GameObject boundingBox = new("SpriteBoundingBox");
		RectTransform rectTransform = boundingBox.AddComponent<RectTransform>();

		rectTransform.SetParent(parentRect);

		rectTransform.position = (worldTopLeft + worldBottomRight) / 2;

		rectTransform.sizeDelta = new Vector2((worldBottomRight.x - worldTopLeft.x) + padding, (worldTopLeft.y - worldBottomRight.y) + padding);

		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

		return rectTransform;
	}
}
