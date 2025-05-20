using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtensions
{
	/// <summary>
	/// Преобразует мировую позицию в локальные координаты этого RectTransform.
	/// </summary>
	/// <param name="rect">Целевой RectTransform.</param>
	/// <param name="worldPosition">Позиция в мировых координатах.</param>
	/// <param name="worldCamera">
	/// Камера, через которую рендерится мир. Если null, будет использована Camera.main.
	/// </param>
	/// <returns>Локальные координаты в пространстве rect, соответствующие worldPosition.</returns>
	public static Vector2 WorldToLocalPoint(
		this RectTransform rect,
		Vector3 worldPosition,
		Camera worldCamera = null)
	{
		if (worldCamera == null)
			worldCamera = Camera.main;

		// 1) мир → экран
		Vector2 screenPoint = worldCamera.WorldToScreenPoint(worldPosition);

		// 2) найти Canvas и его UI-камеру (если есть)
		Canvas canvas = rect.GetComponentInParent<Canvas>();
		Camera uiCamera = null;
		if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
			uiCamera = canvas.worldCamera;

		// 3) экран → локальные координаты внутри rect
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			rect, screenPoint, uiCamera, out Vector2 localPoint);

		return localPoint;
	}

	/// <summary>
	/// Перегрузка: принимает Transform и автоматически берёт его позицию.
	/// </summary>
	public static Vector2 WorldToLocalPoint(
		this RectTransform rect,
		Transform worldTransform,
		Camera worldCamera = null)
	{
		return rect.WorldToLocalPoint(worldTransform.position, worldCamera);
	}

	public static void SetListener(this Button button, Action action)
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() =>
		{
			action?.Invoke();
		});
	}
}
