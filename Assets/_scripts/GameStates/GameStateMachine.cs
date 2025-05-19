using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using Zenject;

namespace StateMachine
{
	public interface IGameStateMachine
	{
		GameStateType CurrentStateType { get; }
		IReadOnlyReactiveProperty<IGameState> CurrentState { get; }
		void Initialize();
		void SetState(GameStateType type);
	}

	public class GameStateMachine : IGameStateMachine, IInitializable
	{
		[Inject] private DiContainer _diContainer;
		private readonly ReactiveProperty<IGameState> _currentState = new();
		private List<IGameState> _states = new()
		{
			new StartSessionState(),
			new SelectionState(),
			new FailState(),
			new WinState()
		};
		public IReadOnlyReactiveProperty<IGameState> CurrentState => _currentState;
		public GameStateType CurrentStateType => _currentState.Value != null ? _currentState.Value.StateType : GameStateType.None;

		public void Initialize()
		{
			foreach (var state in _states)
			{
				_diContainer.Inject(state);
				state.Subscribe();
			}	

			// Initialization logic for the state machine
			var startState = GetState(GameStateType.StartSessionState);
			_currentState.Value = startState; // Set to an initial state if applicable
		}

		public void SetState(GameStateType type)
		{
			_currentState.Value = _states.FirstOrDefault(s => s.StateType == type);
			// Additional logic for transitioning to the new state
		}

		private IGameState GetState(GameStateType type)
		{
			return _states.FirstOrDefault(s => s.StateType == type);
		}
	}
}
