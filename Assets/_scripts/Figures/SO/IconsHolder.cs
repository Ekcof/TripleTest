using System;
using UnityEngine;

namespace Figures
{
	[CreateAssetMenu(fileName = "IconsHolder", menuName = "ScriptableObjects/IconsHolder")]
	public class IconsHolder : ScriptableObject
{
		[SerializeField] private Icon[] _icons;

		public Icon GetIconByType {  get { return _icons[0]; } }
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
