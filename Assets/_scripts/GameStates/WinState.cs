using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WinState : GameState
{
	[Inject] private IWindowManager _windowManager;
	public override GameStateType StateType => GameStateType.WinState;

	public override async UniTask Start()
	{
		await base.Start();
		Debug.Log("WinState started");
		_windowManager.ShowSingleWindow<WinWindow>();
	}
}
