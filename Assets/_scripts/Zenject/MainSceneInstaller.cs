using Figures;
using Installers;
using StateMachine;
using UnityEngine;

public class MainSceneInstaller : BaseInstaller
{
	[SerializeField] private Camera _camera;
	[SerializeField] private FiguresSpawner _figureSpawner;
	[SerializeField] private SlotsManager _slotsManager;
	[SerializeField] private WindowManager _windowManager;

	public override void InstallBindings()
	{
		Container.BindInstance(_camera).WithId("mainCam");

		Bind(_figureSpawner);
		Bind(_slotsManager);
		Bind(_windowManager);
		Bind<GameStateMachine>();
		Bind<PhysicManager>();
		Bind<LevelManager>();
	}
}
