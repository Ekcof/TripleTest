using Figures;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
[Serializable]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private readonly IconType[] _iconTypes;
    [SerializeField] private readonly FormColor[] _formColors;
	[SerializeField] private readonly FormType[] _formTypes;
    [SerializeField,Min(1)] private readonly int _rowNumber;

	[SerializeField] private readonly ExtraFigureConfig[] _extraFigures;

	public IEnumerable<RegularFigureConfig> GetAllCombinations()
	{
		var list = new List<RegularFigureConfig>();
		foreach (var iconType in _iconTypes)
		{
			foreach (var formColor in _formColors)
			{
				foreach (var formType in _formTypes)
				{
					for (int i = 0; i < _rowNumber; i++)
					{
						list.Add(new RegularFigureConfig(iconType, formColor, formType));
					}
				}
			}
		}
		return list;
	}
}
