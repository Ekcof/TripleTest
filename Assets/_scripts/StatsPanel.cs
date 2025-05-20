using Figures;
using UnityEngine;
using Zenject;
using UniRx;
using System;
using TMPro;
public class StatsPanel : MonoBehaviour
{
	[Inject] private IFiguresSpawner _figuresSpawner;
	[Inject] private ILevelManager _levelManager;
	[SerializeField] private TMP_Text _figuresText;
	[SerializeField] private TMP_Text _levelText;

	private void Awake()
	{
		_figuresSpawner.ActiveFigures.ObserveAdd()
			.Subscribe(OnFigureAdded)
			.AddTo(this);

		_figuresSpawner.ActiveFigures.ObserveRemove()
			.Subscribe(OnFigureRemoved)
			.AddTo(this);

		_levelManager.LevelConfig
			.Subscribe(OnLevelChanged)
			.AddTo(this);
	}

	private void OnLevelChanged(LevelConfig config)
	{
			_levelText.text = config != null ? _levelManager.LevelConfig.Value.Id : "unknown";
	}

	private void OnFigureRemoved(CollectionRemoveEvent<RegularFigure> @event)
	{
		UpdateText();
	}

	private void OnFigureAdded(CollectionAddEvent<RegularFigure> @event)
	{
		UpdateText();
	}

	private void UpdateText()
	{
		_figuresText.text = $"{_figuresSpawner.ActiveFigures.Count}";
	}
}
