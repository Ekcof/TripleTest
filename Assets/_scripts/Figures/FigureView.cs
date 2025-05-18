using UnityEngine;

namespace Figures
{
    public interface IFigureView
    {
        void UpdateView(IFigure figure);
        RectTransform RectTransform { get; }
    }


    public class FigureView : MonoBehaviour, IFigureView
    {
        private IFigure _currentFigure;
        public RectTransform RectTransform => (RectTransform)transform;


        public void UpdateView(IFigure figure)
        {
            if (figure == null)
                return;

            _currentFigure = figure;
        }
    }
}