using UnityEngine;

[CreateAssetMenu(fileName = "LevelsHandler", menuName = "Scriptable Objects/LevelsHandler")]
public class LevelsHandler : ScriptableObject
{
    [SerializeField] private LevelConfig[] _levels;

    public LevelConfig GetLevel(int index = 0)
    {
        return _levels[index];
	}
}
