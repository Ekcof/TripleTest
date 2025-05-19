using UnityEngine;

public interface ILevelsHandler
{
	LevelConfig GetLevel(LevelConfig config = null);
}

[CreateAssetMenu(fileName = "LevelsHandler", menuName = "Scriptable Objects/LevelsHandler")]
public class LevelsHandler : ScriptableObject, ILevelsHandler
{
    [SerializeField] private LevelConfig[] _levels;


	public LevelConfig GetLevel(LevelConfig config = null)
	{
		if (_levels == null || _levels.Length == 0)
			return null;

		if (config == null)
			return _levels[0];

		for (int i = 0; i < _levels.Length; i++)
		{
			if (_levels[i].Id == config.Id)
			{
				return (i + 1 < _levels.Length) ? _levels[i + 1] : _levels[0];
			}
		}

		return _levels[0];
	}
}
