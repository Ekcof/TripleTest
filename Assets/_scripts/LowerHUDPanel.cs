using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LowerHUDPanel : MonoBehaviour
{
    [Inject] private ILevelManager _levelManager;
	[SerializeField] private Button _restartButton;

	private void Awake()
    {
        _restartButton.SetListener(OnRestartLevelPressed);
    }

	private void OnRestartLevelPressed()
	{
		_levelManager.RestartLevel();
	}
}
