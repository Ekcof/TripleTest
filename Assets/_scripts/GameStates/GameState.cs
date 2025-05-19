using Cysharp.Threading.Tasks;
using StateMachine;
using System;
using System.Threading;
using UniRx;
using Zenject;

public enum GameStateType
{
	None,
	StartSessionState,
	SelectionState,
	WinState,
	FailState
}

public interface IGameState
{
	GameStateType StateType { get; }
	void Subscribe();
	UniTask Start();
	void Abort();
}

public abstract class GameState : IGameState
{
	[Inject] protected DiContainer DiContainer { get; private set; }
	[Inject] protected IGameStateMachine GameStateMachine { get; private set; }

	protected CancellationTokenSource _cts = new();
	protected CompositeDisposable _disposables = new();
	public abstract GameStateType StateType { get; }


	public virtual void Subscribe()
	{
		GameStateMachine.CurrentState.SkipLatestValueOnSubscribe().Subscribe(OnChangeState).AddTo(_disposables);
	}

	public virtual void Abort()
	{
		_cts?.CancelAndDispose();
		//_disposables?.Clear();
	}

	public virtual async UniTask Start()
	{
		//GameStateMachine.CurrentState.Subscribe(OnChangeState).AddTo(_disposables);
	}

	protected virtual void OnChangeState(IGameState state)
	{
		if (state.StateType != StateType)
		{
			Abort();
		}
		else
		{
			Start().Forget();
		}
	}
}
