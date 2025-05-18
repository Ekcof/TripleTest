using Cysharp.Threading.Tasks;
using Figures;
using System.Diagnostics;
using Zenject;

public class StartSessionState : GameState
{
	[Inject] private IFiguresSpawner _spawner;
	[Inject] private ILevelsHandler _levels;
	[Inject] private IPhysicManager _physicManager;
	[Inject] private ILevelManager _levelManager;
	[Inject] private ISlotsManager _slotsManager;
	public override GameStateType StateType => GameStateType.StartSessionState;

	public async override UniTask Start()
	{
		await base.Start();
		var level = _levelManager.NextLevel(); // TODO: REMOVE
		_slotsManager.ClearAllSlots();

		try
		{
			await _spawner.SpawnFigures(level, _cts.Token);
			await _physicManager.WaitForStop(_cts.Token);
		}
		catch
		{
			UnityEngine.Debug.LogError($"{nameof(StartSessionState)}: Failed to wait for start");
		}
		GameStateMachine.SetState(GameStateType.SelectionState);
	}

	protected override void OnChangeState(IGameState state)
	{
		UnityEngine.Debug.Log($"{nameof(StartSessionState)}: OnChangeState {state}");
		base.OnChangeState(state);
		if (state.StateType != StateType)
			return;
		Start().Forget();
	}
}
