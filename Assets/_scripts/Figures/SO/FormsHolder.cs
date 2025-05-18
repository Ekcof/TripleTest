using System;
using System.Linq;
using UnityEngine;

namespace Figures
{
	[CreateAssetMenu(fileName = "FormsHolder", menuName = "ScriptableObjects/FormsHolder", order = 1)]
	public class FormsHolder : ScriptableObject
	{
		[SerializeField] private FormConfig[] _forms;

		public string GetFormByID(string id)
		{
			var form = _forms.FirstOrDefault(f => f.ID == id);
			if (form != null)
			{
				return form.ID;
			}
			Debug.LogError($"Form with ID {id} not found.");
			return null;
		}
	}

	[Serializable]
	public class FormConfig
	{
		public string ID;
		public FormImage[] FormImages;
		public GameObject Prefab;

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