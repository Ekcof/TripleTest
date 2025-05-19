using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Figures
{
	public interface IFigureView
	{
		RectTransform RectTransform { get; }
		void AssignFigure(IFigure figure);
		void UpdateView(IFigureConfig figureConfig);
		void SetInteractable(bool isActive);
	}


	public class FigureView : MonoBehaviour, IFigureView
	{
		[Inject] private IIconsHolder _iconsHolder;
		[Inject] private IFormsHolder _formsHolder;
		[Inject] private ISlotsManager _slotsManager;
		[Inject] private IFiguresSpawner _figuresSpawner;

		[SerializeField] private Image _form;
		[SerializeField] private Image _icon;
		[SerializeField] private Button _button;

		private IFigure _currentFigure;
		public RectTransform RectTransform => (RectTransform)transform;

		private void Awake()
		{
			if (_button != null)
				_button.SetListener(OnClick);
		}

		private void OnClick()
		{
			if (_currentFigure != null && _slotsManager.TryRegisterFigure(_currentFigure))
			{
				_figuresSpawner.Remove(_currentFigure);
			}
		}

		public void SetInteractable(bool isActive)
		{
			_button.interactable = isActive;
		}

		public void AssignFigure(IFigure figure)
		{
			_currentFigure = figure;
			UpdateView(figure.Config);
		}

		public void UpdateView(IFigureConfig config)
		{
			if (config == null)
				return;

			if (_formsHolder == null)
			{
				Debug.LogError("FormsHolder is not set");
			}
			_form.sprite = _formsHolder.GetFormSprite(config);
			_icon.sprite = _iconsHolder.GetIconByType(config.Icon).Sprite;
		}
	}
}