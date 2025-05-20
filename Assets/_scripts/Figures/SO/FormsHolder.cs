using System;
using System.Linq;
using UnityEngine;

namespace Figures
{
	public interface IFormsHolder
	{
		Sprite GetFormSprite(IFigureConfig config);
	}

	[CreateAssetMenu(fileName = "FormsHolder", menuName = "Scriptable Objects/FormsHolder")]
	public class FormsHolder : ScriptableObject, IFormsHolder
	{
		[SerializeField] private FormConfig[] _forms;

		public Sprite GetFormSprite(IFigureConfig config)
		{
			var form = _forms.FirstOrDefault(f => f.FormType == config.FormType);
			if (form != null)
			{
				return form.FormImages.FirstOrDefault(f => f.Color == config.Color).Sprite;
			}
			Debug.LogError($"Sprite with type {config.FormType} and color {config.Color} not found.");
			return null;
		}
	}

	[Serializable]
	public class FormConfig
	{
		public FormType FormType;
		public FormImage[] FormImages;

		public Sprite GetImageByColor(FormColor color)
		{
			var formImage = FormImages.FirstOrDefault(f => f.Color == color);
			if (formImage.Color != default)
			{
				return formImage.Sprite;
			}
			Debug.LogError($"Form image with color {color} not found.");
			return null;
		}
	}

	[Serializable]
	public struct FormImage
	{
		public FormColor Color;
		public Sprite Sprite;
	}

	public enum FormColor
	{
		Gray,
		Blue,
		Green,
		Red,
		Yellow
	}

	public enum FormType
	{
		Circle,
		Triangle,
		Square,
		Blob
	}
}