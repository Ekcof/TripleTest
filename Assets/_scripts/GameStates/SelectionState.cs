using Cysharp.Threading.Tasks;
using Figures;
using UnityEngine;
using Zenject;

public class SelectionState : GameState
{
	[Inject] private IFiguresSpawner _spawner;
	public override GameStateType StateType => GameStateType.SelectionState;

	public async override UniTask Start()
	{
		await base.Start();
		_spawner.ToggleFigureControl(true);
	}

	protected override void OnChangeState(IGameState state)
	{
		base.OnChangeState(state);
		if (state.StateType != StateType)
			return;
		Start().Forget();
	}

}
