using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public interface IWindowManager
{
	void CloseAllWindows(bool hideFade = false);
	void ShowSingleWindow<T>() where T : class, IUIWindow;
}

public class WindowManager : MonoBehaviour, IWindowManager
{
	[SerializeField] private GameObject _fade;
	private List<IUIWindow> _windows = new();

	private void Awake()
	{
		var windows = FindObjectsByType<UIWindow>(FindObjectsSortMode.None);
		foreach (var window in windows)
		{
			_windows.Add(window);
		}
	}

	public void CloseAllWindows(bool hideFade = false)
	{
		if (hideFade)
			_fade.SetActive(false);

		_windows.ForEach(w => w.Hide());
	}

	private T GetWindowByType<T>() where T : class, IUIWindow
	{
		return _windows.Find(w => w is T) as T;
	}

	public void ShowSingleWindow<T>() where T : class, IUIWindow
	{
		var window = GetWindowByType<T>();
		if (window != null)
		{
			CloseAllWindows();
			_fade.SetActive(true);
			window.Show();
		}
		else
		{
			Debug.LogError($"Window of type {typeof(T)} not found.");
		}
	}
}
