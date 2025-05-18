using UnityEngine;

public interface ILevelsHandler
{
	LevelConfig GetLevel(int index = 0);
}

[CreateAssetMenu(fileName = "LevelsHandler", menuName = "Scriptable Objects/LevelsHandler")]
public class LevelsHandler : ScriptableObject, ILevelsHandler
{
    [SerializeField] private LevelConfig[] _levels;

    public LevelConfig GetLevel(int index = 0)
    {
        return _levels[index];
	}
}
