using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;
//using CySharp.Threading.Tasks;

namespace Figures
{
	public interface IFiguresSpawner
	{
		void SpawnFigures();
	}

	public class FiguresSpawner : MonoBehaviour, IFiguresSpawner
	{
		[Inject] private ILevelsHandler _levelsHandler;
		[Inject] private DiContainer _diContainer;

		[SerializeField] private Transform[] _spawnPoints;
		[SerializeField] private RegularFigure _figurePrefab;
		[SerializeField,Min(0.05f)] private float _spawnDelay = 0.1f;

		private CancellationTokenSource _cts = new();
		private CommonPool<RegularFigure> _pool;
		private ReactiveCollection<RegularFigure> _activeFigures = new();

		private void Awake()
		{
			_pool = new CommonPool<RegularFigure>(_figurePrefab, transform, _diContainer);
		}

		[ContextMenu("Spawn Items")]
		public void SpawnFigures()
		{
			_pool.Push(_activeFigures);
			_cts?.CancelAndDispose();
			_cts = new();
			if (_levelsHandler == null)
			{
				Debug.LogError("LevelsHandler is not set");
				return;
			}
			var level = _levelsHandler.GetLevel(); // REMOVE
			SpawnFigures(level.GetAllCombinations(), _cts.Token).Forget();
		}

		public async UniTask SpawnFigures(IEnumerable<RegularFigureConfig> configs, CancellationToken token)
		{
			int direction = 1;
			int currentIndex = _spawnPoints.GetRandomIndex();

			foreach (var config in configs)
			{
				var figure = _pool.Pop();
				figure.gameObject.SetActive(true);
				figure.transform.position = _spawnPoints[currentIndex].position;
				figure.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)); 
				figure.ApplyConfig(config);

				currentIndex += direction;

				if (currentIndex >= _spawnPoints.Length || currentIndex < 0)
				{
					direction *= -1;
					currentIndex += direction;
				}
				_activeFigures.Add(figure);
				await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay), cancellationToken: token);
			}
		}
	}
}