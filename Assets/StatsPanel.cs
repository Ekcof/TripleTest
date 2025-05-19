using Figures;
using UnityEngine;
using Zenject;
using UniRx;
using System;
using TMPro;
public class StatsPanel : MonoBehaviour
{
	[Inject] private IFiguresSpawner _figuresSpawner;
	[SerializeField] private TMP_Text _text;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
		_figuresSpawner.ActiveFigures.ObserveAdd()
			.Subscribe(OnFigureAdded)
			.AddTo(this);

		_figuresSpawner.ActiveFigures.ObserveRemove()
			.Subscribe(OnFigureRemoved)
			.AddTo(this);
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
		_text.text = $"{_figuresSpawner.ActiveFigures.Count}";
	}
}
