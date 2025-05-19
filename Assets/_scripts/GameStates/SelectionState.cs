using Cysharp.Threading.Tasks;
using Figures;
using System;
using System.Threading;
using UniRx;
using Zenject;

public class SelectionState : GameState
{
	[Inject] private IFiguresSpawner _spawner;
	[Inject] private IPhysicManager _physicManager;
	[Inject] private ISlotsManager _slotsManager;

	public override GameStateType StateType => GameStateType.SelectionState;

	public override void Subscribe()
	{
		base.Subscribe();
		_spawner.ActiveFigures.ObserveRemove().Subscribe(OnFigureRemoved)
			.AddTo(_disposables);
	}

	private void OnFigureRemoved(CollectionRemoveEvent<RegularFigure> @event)
	{
		if (GameStateMachine.CurrentStateType is GameStateType.SelectionState)
		{
			_cts?.CancelAndDispose();
			_cts = new();
			OnRemoveFigureAsync(_cts.Token).Forget();

			if (_spawner.ActiveFigures.Count == 0)
			{
				GameStateMachine.SetState(GameStateType.WinState);
			}
			else if(_slotsManager.AreAllSlotsOccupied)
			{
				GameStateMachine.SetState(GameStateType.FailState);
			}
		}
	}

	private async UniTask OnRemoveFigureAsync(CancellationToken token)
	{
		_physicManager.ToggleSimulation(true);
		try
		{
			await _physicManager.WaitForStop(token);
		}
		catch
		{
			return;
		}
	}


	public async override UniTask Start()
	{
		await base.Start();
		_spawner.ToggleFigureControl(true);
	}

	protected override void OnChangeState(IGameState state)
	{
		base.OnChangeState(state);
		if (state.StateType != StateType)
		{
			_spawner.ToggleFigureControl(false);
			return;
		}
	}
}
