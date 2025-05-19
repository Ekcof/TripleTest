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
		IReadOnlyReactiveCollection<RegularFigure> ActiveFigures { get;}
		void Remove(IFigure figure);
		void Clear();
		UniTask SpawnFigures(LevelConfig levelConfig, CancellationToken token);
		void ToggleFigureControl(bool isActive);
	}

	public class FiguresSpawner : MonoBehaviour, IFiguresSpawner
	{
		[Inject] private DiContainer _diContainer;
		[Inject] private IPhysicManager _physicManager;

		[SerializeField] private Transform[] _spawnPoints;
		[SerializeField] private RegularFigure _figurePrefab;
		[SerializeField, Min(0.05f)] private float _spawnDelay = 0.1f;

		private CommonPool<RegularFigure> _pool;
		private ReactiveCollection<RegularFigure> _activeFigures = new();
		public IReadOnlyReactiveCollection<RegularFigure> ActiveFigures => _activeFigures;

		private void Awake()
		{
			_pool = new CommonPool<RegularFigure>(_figurePrefab, transform, _diContainer);
		}

		public void Clear()
		{
			_pool.Push(_activeFigures);
			_activeFigures.Clear();
			_physicManager.ClearAllRigidBodies();
		}
		
		public void Remove(IFigure figure)
		{
			if (figure is RegularFigure regular && _activeFigures.Contains(regular))
			{
				_activeFigures.Remove(regular);
				_pool.Push(regular);
			}
		}

		public async UniTask SpawnFigures(LevelConfig levelConfig, CancellationToken token)
		{
			_pool.Push(_activeFigures);

			try
			{
				await SpawnFiguresInternal(levelConfig.GetAllCombinations(), token);
			}
			catch
			{
				Debug.LogError("Failed to spawn figures");
				return;
			}
		}

		private async UniTask SpawnFiguresInternal(IEnumerable<RegularFigureConfig> configs, CancellationToken token)
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
				
				_physicManager.RegisterRigidBody(figure.Rigidbody2D);

				currentIndex += direction;

				if (currentIndex >= _spawnPoints.Length || currentIndex < 0)
				{
					direction *= -1;
					currentIndex += direction;
				}
				_activeFigures.Add(figure);
				try
				{
					await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay), cancellationToken: token);
				}
				catch
				{
					Debug.LogError("Failed to spawn figures. Delay has been interrupted");
					return;
				}
			}
		}

		public void ToggleFigureControl(bool isActive)
		{
			foreach (var figure in _activeFigures)
			{
				figure.SetButtonInteractable(isActive);
			}
		}
	}
}