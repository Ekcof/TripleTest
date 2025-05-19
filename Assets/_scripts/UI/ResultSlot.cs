using UnityEngine;
using UnityEngine.UI;
using Figures;
using Zenject;
using DG.Tweening;

public class ResultSlot : MonoBehaviour
{
	[Inject(Id = "mainCam")] private Camera _camera;
	[SerializeField] private FigureView _view;
	private IFigureConfig _currentConfig;
	public IFigureConfig CurrentConfig => _currentConfig;

	public bool IsOccupied => _currentConfig != null;

	public void UnassignFigure()
	{
		_currentConfig = null;
		_view.gameObject.SetActive(false);
	}

	public void AssignFigure(IFigure figure)
	{
		_currentConfig = figure.Config;
		_view.gameObject.SetActive(true);
		_view.UpdateView(figure.Config);
		SetFlight(figure);
	}

	private void SetFlight(IFigure figure)
	{
		var localPos = _view.Parent.WorldToLocalPoint(figure.Position, _camera);

		DOTween.Kill(_view.RectTransform);

		Debug.Log($"AnchorPosition {localPos}");
		_view.RectTransform.anchoredPosition = localPos;

		_view.RectTransform.eulerAngles = figure.Rotation;

		_view.RectTransform
			.DOAnchorPos(Vector2.zero, 0.5f)
			.SetEase(Ease.InOutQuad)
			.OnComplete(() =>
			{
				_view.SetInteractable(true);
			});

		_view.RectTransform
			.DORotate(Vector3.zero, 0.5f)
			.SetEase(Ease.OutBack);
	}
}
