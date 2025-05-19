using StateMachine;
using UniRx;
using UnityEngine;
using Zenject;

public interface ILevelManager
{
	public int CurrentMatchNum { get; }
	public IReadOnlyReactiveProperty<LevelConfig> LevelConfig { get; }
	LevelConfig NextLevel();
	void RestartLevel();
	void StartNextLevel();
}

public class LevelManager : ILevelManager
{
	[Inject] private ILevelsHandler _levelsHandler;
	[Inject] private IGameStateMachine _gameStateMachine;

	private ReactiveProperty<LevelConfig> _levelConfig = new();
	public int CurrentMatchNum => _levelConfig.Value.RowNumber;

	public IReadOnlyReactiveProperty<LevelConfig> LevelConfig => _levelConfig;

	public LevelConfig NextLevel()
	{
		var level = _levelsHandler.GetLevel();
		_levelConfig.Value = level;
		return level;
	}

	public void RestartLevel()
	{
		_gameStateMachine.SetState(GameStateType.StartSessionState);
	}

	public void StartNextLevel()
	{
		var level = _levelsHandler.GetLevel(_levelConfig.Value);
		Debug.Log($"_____Get level {level.Id}");
		_levelConfig.Value = level;
		_gameStateMachine.SetState(GameStateType.StartSessionState);
	}
}
