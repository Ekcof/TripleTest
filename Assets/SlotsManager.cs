using Figures;
using Mono.Cecil;
using StateMachine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public interface ISlotsManager
{
	bool AreAllSlotsOccupied { get; }
	int SlotsNum { get; }
	bool TryRegisterFigure(IFigure figure);
	void ClearAllSlots();
}

public class SlotsManager : MonoBehaviour, ISlotsManager
{
	[Inject] private ILevelManager _levelManager;
	[Inject] private IGameStateMachine _stateMachine;
	[Inject] private IFiguresSpawner _spawner;
	[SerializeField] private ResultSlot[] _slots;
	private int OccupiedSlotsNum => _slots.Where(s => s.IsOccupied).Count();
	public bool AreAllSlotsOccupied => OccupiedSlotsNum == _slots.Length;
	public int SlotsNum => _slots.Length;
	public IEnumerable<IFigureConfig> ConfigInSlots => _slots.Select(s => s.CurrentConfig).Where(figure => figure != null);
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

	private void CheckSlots()
	{
		int requiredCount = _levelManager.CurrentMatchNum;

		var figureDict = new Dictionary<string, List<ResultSlot>>();

		foreach (var slot in _slots)
		{
			if (slot.IsOccupied)
			{
				var config = slot.CurrentConfig;
				var key = $"{config.Icon}{config.Color}{config.FormType}";

				if (figureDict.ContainsKey(key))
				{
					figureDict[key].Add(slot);
				}
				else
				{
					figureDict[key] = new List<ResultSlot> { slot };
				}
			}
		}
		foreach (var kvp in figureDict)
		{
			if (kvp.Value.Count >= requiredCount)
			{
				Debug.Log($"{nameof(SlotsManager)}: {nameof(CheckSlots)}: {kvp.Key} has match. Removing them");
				foreach (var slot in kvp.Value)
				{
					slot.UnassignFigure();
				}
				OrderSlots();
				return;
			}
		}
		Debug.Log($"{nameof(SlotsManager)}: {nameof(CheckSlots)}: No match found");
		Debug.Log($"OccupiedSlotsNum {OccupiedSlotsNum} ActiveFigures Count {_spawner.ActiveFigures.Count}");
	}

	private void OrderSlots()
	{
		var occupiedSlots = _slots.Where(slot => slot.IsOccupied).ToList();
		var emptySlots = _slots.Where(slot => !slot.IsOccupied).ToList(); 
		var orderedSlots = occupiedSlots.Concat(emptySlots).ToList();

		for (int i = 0; i < _slots.Length; i++)
		{
			_slots[i] = orderedSlots[i];
		}

		// Move ResultSlot transforms to the end of sibling indices  
		foreach (var slot in emptySlots)
		{
			slot.transform.SetAsLastSibling();
		}
	}
}
