using System;
using UnityEngine;
using Zenject;
using UniRx;

public enum ColldierLevelType
{
	Rectangle,
	Triangle,
	Goblet
}

public class ColliderManager : MonoBehaviour
{
	[Serializable] private class ColliderLevelForm
	{
		public ColldierLevelType Type;
		public GameObject ColliderGO;
	}

	[Inject] private ILevelManager _levelManager;
	[SerializeField] private ColliderLevelForm[] _colliderLevels;

	private void Awake()
	{
		_levelManager.LevelConfig.Subscribe(OnLevelChanged).AddTo(this);
	}

	private void OnLevelChanged(LevelConfig config)
	{
		if (config == null)
			return;
		foreach (var colliderLevel in _colliderLevels)
		{
			colliderLevel.ColliderGO.SetActive(colliderLevel.Type == config.ColliderLevelType);
		}
	}

}
