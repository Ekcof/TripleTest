using UnityEngine;

namespace Figures
{
	public class ExtraFigureConfig
	{
		public ExtraFigureConfig(FigureType type, int count)
		{
			Type = type;
			Count = count;
		}
		public FigureType Type { get; private set; }
		public int Count { get; private set; }
	}

	public class RegularFigureConfig
	{
		public RegularFigureConfig(IconType iconType, FormColor formColor, FormType formType)
		{
			Icon = iconType;
			Color = formColor;
			FormType = formType;
		}

		public FormColor Color { get; private set; }
		public FormType FormType { get; private set; }
		public IconType Icon { get; private set; }
	}
}