using UnityEngine;

public interface IUIWindow
{
    bool IsActive { get; }
	void Show();
	void Hide();
}

public abstract class UIWindow : MonoBehaviour, IUIWindow
{
    [SerializeField] private Canvas _canvas;
    public bool IsActive => _canvas.enabled;

    public virtual void Show()
	{
		_canvas.enabled = true;
	}

	public virtual void Hide()
	{
		_canvas.enabled = false;
	}
}
