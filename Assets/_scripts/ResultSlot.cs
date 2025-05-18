using UnityEngine;
using UnityEngine.UI;
using Figures;

public class ResultSlot : MonoBehaviour
{
	[SerializeField] private FigureView _view;
	private IFigure _currentFigure;
	public IFigure CurrentFigure => _currentFigure;

	public bool IsOccupied => _currentFigure != null;

	public void UnassignFigure()
	{
		_currentFigure = null;
		_view.gameObject.SetActive(false);
	}

	public void AssignFigure(IFigure figure)
	{
		_currentFigure = figure;
		_view.gameObject.SetActive(true);
		_view.UpdateView(figure);
	}
}
