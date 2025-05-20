using Figures;
using StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public interface ISlotsManager
{
	bool AreAllSlotsNotEmpty { get; }
	bool NoProcessingSlots { get; }
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
	private int OccupiedSlotsNum => _slots.Where(s => s.Status != SlotStatus.Empty).Count();
	public bool AreAllSlotsNotEmpty => OccupiedSlotsNum == _slots.Length;
	public bool NoProcessingSlots => _slots.Where(s => s.Status == SlotStatus.Processing).Count() == 0;
	public int SlotsNum => _slots.Length;
	public IEnumerable<IFigureConfig> ConfigInSlots => _slots.Select(s => s.CurrentConfig).Where(figure => figure != null);
	public bool TryRegisterFigure(IFigure figure)
	{
		foreach (var slot in _slots)
		{
			if (slot.Status is SlotStatus.Empty)
			{
				slot.AssignFigure(figure, CheckSlots);
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
			if (slot.Status is SlotStatus.Occupied)
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
		var occupiedSlots = _slots.Where(slot => slot.Status == SlotStatus.Occupied).ToList();
		var processingSlots = _slots.Where(slot => slot.Status == SlotStatus.Processing).ToList();
		var emptySlots = _slots.Where(slot => slot.Status == SlotStatus.Empty).ToList();
		var orderedSlots = occupiedSlots.Concat(processingSlots).Concat(emptySlots).ToList();

		for (int i = 0; i < _slots.Length; i++)
		{
			_slots[i] = orderedSlots[i];
		}

		foreach (var slot in occupiedSlots)
		{
			slot.transform.SetAsLastSibling();
		}
		foreach (var slot in processingSlots)
		{
			slot.transform.SetAsLastSibling();
		}
		foreach (var slot in emptySlots)
		{
			slot.transform.SetAsLastSibling();
		}
	}
}
