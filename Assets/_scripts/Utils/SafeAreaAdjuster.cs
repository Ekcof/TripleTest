using UnityEngine;

public class SafeAreaAdjuster : MonoBehaviour
{
	[SerializeField] private RectTransform _rectTransform;

	void Start()
	{
		AdjustToSafeArea();
	}

	void AdjustToSafeArea()
	{

		Rect safeArea = Screen.safeArea;

		Vector2 safeAreaMin = safeArea.position;
		Vector2 safeAreaMax = safeArea.position + safeArea.size;

		Vector2 currentPosition = _rectTransform.anchoredPosition;
		Vector2 currentSize = _rectTransform.sizeDelta;

		currentPosition.y = Mathf.Max(currentPosition.y, safeAreaMin.y);
		_rectTransform.anchoredPosition = currentPosition;

		_rectTransform.sizeDelta = new Vector2(currentSize.x, currentSize.y);
	}
}