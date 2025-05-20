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
		FigureType FigureType { get; }
		Vector3 Position { get; }
		Vector3 Rotation { get; }
		Rigidbody2D Rigidbody2D { get; }
		IFigureConfig Config { get; }
		void ApplyConfig(RegularFigureConfig config);
		void SetButtonInteractable(bool isActive);
		void Deactivate();
	}
	public class RegularFigure : MonoBehaviour, IFigure 
	{
		[SerializeField] private FigureView _figureView;
		[SerializeField] private FigureCollider[] _colliders;
		[SerializeField] private Rigidbody2D _rigidbody2D;
		private RegularFigureConfig _config;
		public FigureType FigureType => FigureType.Regular;
		public Vector3 Position => transform.position;
		public Vector3 Rotation => transform.eulerAngles;
		public IFigureConfig Config => _config;
		public Rigidbody2D Rigidbody2D => _rigidbody2D;

		public void ApplyConfig(RegularFigureConfig config)
		{
			_config = config;
			_figureView.AssignFigure(this);
			ActivateCollider(config.FormType);
		}

		public void Deactivate()
		{
			gameObject.SetActive(false);
			_figureView.SetInteractable(false);
		}

		public void SetButtonInteractable(bool isActive)
		{
			_figureView.SetInteractable(isActive);
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