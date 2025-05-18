using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Figures
{
    public interface IFigureView
    {
        void UpdateView(IFigure figure);
        RectTransform RectTransform { get; }
    }


	public class FigureView : MonoBehaviour, IFigureView
	{
		[Inject] private IIconsHolder _iconsHolder;
		[Inject] private IFormsHolder _formsHolder;

		[SerializeField] private Image _form;
		[SerializeField] private Image _icon;

		private IFigure _currentFigure;
		public RectTransform RectTransform => (RectTransform)transform;


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