using System;
using System.Linq;
using UnityEngine;

namespace Figures
{
	public interface IIconsHolder
	{
		Icon GetIconByType(IconType type);
	}

	[CreateAssetMenu(fileName = "IconsHolder", menuName = "ScriptableObjects/IconsHolder")]
	public class IconsHolder : ScriptableObject, IIconsHolder
	{
		[SerializeField] private Icon[] _icons;

		Icon IIconsHolder.GetIconByType(IconType type)
		{
			// Find the icon with the matching type in the _icons array  
			return _icons.FirstOrDefault(icon => icon.Type == type);
		}
	}

	[Serializable]
	public class Icon
	{
		public IconType Type;
		public Sprite Sprite;
	}

	public enum IconType
	{
		None = 0,
		Pig = 1,
		Wolf = 2,
		Sheep = 3,
		Cat = 4,
		Lizard = 5
	}
}
