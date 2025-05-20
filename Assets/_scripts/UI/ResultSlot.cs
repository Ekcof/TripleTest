using UnityEngine;
using Figures;
using Zenject;
using DG.Tweening;
using System;

public enum SlotStatus
{
	Empty,
	Processing,
	Occupied
}

public class ResultSlot : MonoBehaviour
{
	[Inject(Id = "mainCam")] private Camera _camera;
	[SerializeField] private FigureView _view;
	private IFigureConfig _currentConfig;
	public IFigureConfig CurrentConfig => _currentConfig;

	public SlotStatus Status { get; private set; } = SlotStatus.Empty;

	public void UnassignFigure()
	{
		Status = SlotStatus.Empty;
		_currentConfig = null;
		_view.gameObject.SetActive(false);
	}

	public void AssignFigure(IFigure figure, Action onArrive)
	{
		_currentConfig = figure.Config;
		_view.gameObject.SetActive(true);
		_view.UpdateView(figure.Config);
		SetFlight(figure, onArrive);
	}

	private void SetFlight(IFigure figure, Action onArrive)
	{
		var localPos = _view.Parent.WorldToLocalPoint(figure.Position, _camera);
		Status = SlotStatus.Processing;
		_view.SetParticlesActive(true);

		DOTween.Kill(_view.RectTransform);

		Debug.Log($"AnchorPosition {localPos}");
		_view.RectTransform.anchoredPosition = localPos;

		_view.RectTransform.eulerAngles = figure.Rotation;

		_view.RectTransform
			.DOAnchorPos(Vector2.zero, 0.5f)
			.SetEase(Ease.InOutQuad)
			.OnComplete(() =>
			{
				_view.SetParticlesActive(false);
				Status = SlotStatus.Occupied;
				onArrive?.Invoke();
			});

		_view.RectTransform
			.DORotate(Vector3.zero, 0.5f)
			.SetEase(Ease.OutBack);
	}
}
