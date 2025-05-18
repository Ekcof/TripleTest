using StateMachine;
using System.Threading;
using UnityEngine;
using Zenject;

public interface IGameState
{
	void Start();
	void Abort();
}

public abstract class GameState : IGameState
{
	[Inject] protected DiContainer DiContainer { get; private set; }
	[Inject] protected IGameStateMachine GameStateMachine { get; private set; }

	protected CancellationTokenSource _cts = new();

	public virtual void Abort()
	{
		_cts?.CancelAndDispose();
	}

	public abstract void Start();
}
