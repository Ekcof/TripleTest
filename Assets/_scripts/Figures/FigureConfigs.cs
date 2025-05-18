using UnityEngine;

namespace Figures
{
	public interface IFigureConfig
	{
		FigureType FigureType { get; }
		public FormColor Color { get; }
		public FormType FormType { get; }
		public IconType Icon { get; }
	}

	public class ExtraFigureConfig: IFigureConfig
	{
		public ExtraFigureConfig(FigureType type, int count)
		{
			FigureType = type;
			Count = count;
		}
		public FigureType FigureType { get; private set; }
		public int Count { get; private set; }

		public FormColor Color { get; private set; }

		public FormType FormType { get; private set; }

		public IconType Icon { get; private set; }
	}

	public class RegularFigureConfig: IFigureConfig
	{
		public FigureType FigureType => FigureType.Regular;

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