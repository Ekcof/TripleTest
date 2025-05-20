using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using StateMachine;
public class NextLevelButton : MonoBehaviour
{
	[Inject] private ILevelManager _levelManager;
	[Inject] private IGameStateMachine _gameStateMachine;
	[SerializeField] private Button _button;

	private void Awake()
	{
		_button.SetListener(_levelManager.StartNextLevel);
		_gameStateMachine.CurrentState.Subscribe(OnChangeState).AddTo(this);
	}

	private void OnChangeState(IGameState state)
	{
		gameObject.SetActive(state?.StateType is GameStateType.WinState);
	}
}
