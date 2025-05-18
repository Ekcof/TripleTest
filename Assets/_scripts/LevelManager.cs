using UniRx;
using UnityEngine;
using Zenject;

public interface ILevelManager
{
	public int CurrentMatchNum { get; }
	public IReadOnlyReactiveProperty<LevelConfig> LevelConfig { get; }
	LevelConfig NextLevel();
}

public class LevelManager : ILevelManager
{
	[Inject] private ILevelsHandler _levelsHandler;
	private ReactiveProperty<LevelConfig> _levelConfig = new();
	public int CurrentMatchNum => _levelConfig.Value.RowNumber;

	public IReadOnlyReactiveProperty<LevelConfig> LevelConfig => _levelConfig;

	public LevelConfig NextLevel()
	{
		var level = _levelsHandler.GetLevel();
		_levelConfig.Value = level;
		return level;
	}

	public void SetLevel(LevelConfig levelConfig)
	{
		_levelConfig.Value = levelConfig;
	}

}
