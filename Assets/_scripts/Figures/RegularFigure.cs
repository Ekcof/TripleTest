using Figures;
using System;
using UnityEngine;
using static UnityEngine.Rendering.STP;

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
		IFigureConfig Config { get; }
		void ApplyConfig(RegularFigureConfig config);
	}
	public class RegularFigure : MonoBehaviour, IFigure 
	{
		[SerializeField] private FigureView _figureView;
		[SerializeField] private FigureCollider[] _colliders;
		[SerializeField] private FigureType _figureType = FigureType.Regular;
		private RegularFigureConfig _config;
		public IFigureConfig Config => _config;

		public void ApplyConfig(RegularFigureConfig config)
		{
			_config = config;
			_figureView.UpdateView(this);
			ActivateCollider(config.FormType);
		}

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