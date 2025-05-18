using Figures;
using System;
using UnityEngine;

namespace Figures
{
	public enum FigureType
	{
		Regular,
		Freeze,
		Sticky,
		Bomb
	}

	public interface IFigure
	{
		FormColor FormColor { get; }
	}
	public class Figure : MonoBehaviour, IFigure
	{
		[SerializeField] private FigureView _figureView;
		[SerializeField] private FigureCollider[] _colliders;
		[SerializeField] private FigureType _figureType = FigureType.Regular;
		public FormColor FormColor { get; private set; }

		private void ActivateCollider(FormType formType)
		{
			foreach (var collider in _colliders)
			{
				if (collider.FormType == formType)
				{
					collider.SetActive(true);
				}
				else
				{
					collider.SetActive(false);
				}
			}
		}
	}

	[Serializable]
	public class FigureCollider
	{
		public FormType FormType;
		public GameObject ColliderObject;

		public void SetActive(bool isActive)
		{
			if (ColliderObject == null)
			{
				Debug.LogError($"ColliderObject is null for FormType: {FormType}");
				return;
			}
			ColliderObject.SetActive(isActive);
		}
	}
}