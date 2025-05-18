using UniRx;

namespace StateMachine
{
	public interface IGameStateMachine
	{
		IReadOnlyReactiveProperty<IGameState> CurrentState { get; }
		void Initialize();
		void SetState(IGameState state);
	}

	public class GameStateMachine : IGameStateMachine
	{
		private readonly ReactiveProperty<IGameState> _currentState = new();

		public IReadOnlyReactiveProperty<IGameState> CurrentState => _currentState;

		public void Initialize()
		{
			// Initialization logic for the state machine
			_currentState.Value = null; // Set to an initial state if applicable
		}

		public void SetState(IGameState state)
		{
			_currentState.Value = state;
			// Additional logic for transitioning to the new state
		}
	}
}
