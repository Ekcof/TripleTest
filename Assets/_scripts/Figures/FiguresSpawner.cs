using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
//using CySharp.Threading.Tasks;

namespace Figures
{
	public interface IFiguresSpawner
	{
		void SpawnFigures(int num);
	}

	public class FiguresSpawner : MonoBehaviour, IFiguresSpawner
	{
		[SerializeField] private Transform[] _spawnPoints;
		[SerializeField] private Figure _figurePrefab;
		[SerializeField,Min(0.05f)] private float _spawnDelay = 0.1f;

		private CancellationTokenSource _cts = new();
		private CommonPool<Figure> _pool;

		public void SpawnFigures(int num)
		{
			_cts?.CancelAndDispose();
			_cts = new();



		}

		private void Awake()
		{
			_pool = new CommonPool<Figure>(_figurePrefab);
		}

		//public async UniTask SpawnFigure()
		//{

		//}
		public async UniTask SpawnFigures(int num, CancellationToken token)
		{
			int direction = 1;
			int currentIndex = _spawnPoints.GetRandomIndex();

			for (int i = 0; i < num; i++)
			{
				var figure = _pool.Pop();
				figure.gameObject.SetActive(true);
				figure.transform.position = _spawnPoints[currentIndex].position;

				currentIndex += direction;

				if (currentIndex >= _spawnPoints.Length || currentIndex < 0)
				{
					direction *= -1;
					currentIndex += direction;
				}

				await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay), cancellationToken: token);
			}
		}
	}
}