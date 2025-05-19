using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class FailState : GameState
{
	[Inject] private IWindowManager _windowManager;
	public override GameStateType StateType => GameStateType.FailState;

	public override async UniTask Start()
	{
		await base.Start();
		_windowManager.ShowSingleWindow<FailWindow>();

	}

}
