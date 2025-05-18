using Figures;
using StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public interface ISlotsManager
{
	int SlotsNum { get; }
	bool TryRegisterFigure(IFigure figure);
	void ClearAllSlots();
}

public class SlotsManager : MonoBehaviour, ISlotsManager
{
	[Inject] private ILevelManager _levelManager;
	[Inject] private IGameStateMachine _stateMachine;
	[SerializeField] private ResultSlot[] _slots;
	public int SlotsNum => _slots.Length;
	public IEnumerable<IFigure> FiguresInSlots => _slots.Select(_slots => _slots.CurrentFigure).Where(figure => figure != null);

	public bool TryRegisterFigure(IFigure figure)
	{
		foreach (var slot in _slots)
		{
			if (!slot.IsOccupied)
			{
				slot.AssignFigure(figure);
				CheckSlots();
				return true;
			}
		}
		return false;
	}

	public void ClearAllSlots()
	{
		foreach (var slot in _slots)
		{
			slot.UnassignFigure();
		}
	}

	public void CheckSlots()
	{
		int requiredCount = _levelManager.CurrentMatchNum;
		var figureCounts = new Dictionary<(IconType Icon, FormColor Color, FigureType Type), int>();

		foreach (var slot in _slots)
		{
			if (slot.IsOccupied)
			{
				var figure = slot.CurrentFigure;
				var key = (figure.Config.Icon, figure.Config.Color, figure.Config.FigureType);

				if (figureCounts.ContainsKey(key))
				{
					figureCounts[key]++;
					if (figureCounts[key] >= requiredCount)
					{
						_stateMachine.SetState(GameStateType.StartSessionState); // remove
						return;
					}
				}
				else
				{
					figureCounts[key] = 1;
				}
			}
		}
	}
}
