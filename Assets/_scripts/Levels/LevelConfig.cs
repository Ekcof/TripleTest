using Figures;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
[Serializable]
public class LevelConfig : ScriptableObject
{
	[SerializeField] private IconType[] _iconTypes;
	[SerializeField] private FormColor[] _formColors;
	[SerializeField] private FormType[] _formTypes;
	[SerializeField, Min(1)] private int _rowNumber;

	[SerializeField] private ExtraFigureConfig[] _extraFigures;

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
