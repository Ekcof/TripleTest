using Installers;
using System.Diagnostics;
using UnityEditor.PackageManager.UI;

public class MainSceneInstaller : BaseInstaller
{
	public override void InstallBindings()
	{
		//var windows = FindObjectsOfType<UIWindow>();

		//foreach (var window in windows)
		//{
		//	UnityEngine.Debug.Log($"_____Add {window.name} to container");
		//	Container.Bind(window.GetType()).FromInstance(window).AsSingle();
		//}
		Bind<PhysicManager>();
	}
}
