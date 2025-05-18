using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Figures
{
    public interface IFigureView
    {
		RectTransform RectTransform { get; }
		void UpdateView(IFigure figure);
		void SetInteractable(bool isActive);
	}


	public class FigureView : MonoBehaviour, IFigureView
	{
		[Inject] private IIconsHolder _iconsHolder;
		[Inject] private IFormsHolder _formsHolder;

		[SerializeField] private Image _form;
		[SerializeField] private Image _icon;
		[SerializeField] private Button _button;

		private IFigure _currentFigure;
		public RectTransform RectTransform => (RectTransform)transform;

		public void SetInteractable(bool isActive)
		{
			_button.interactable = isActive;
		}

		public void UpdateView(IFigure figure)
		{
			if (figure == null)
				return;

			_currentFigure = figure;

			if (_formsHolder == null)
			{
				Debug.LogError("FormsHolder is not set");
			}
			_form.sprite = _formsHolder.GetFormSprite(figure.Config);
			_icon.sprite = _iconsHolder.GetIconByType(figure.Config.Icon).Sprite;
		}
	}
}