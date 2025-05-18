using Figures;
using Installers;
using StateMachine;
using UnityEngine;

public class MainSceneInstaller : BaseInstaller
{
	[SerializeField] private FiguresSpawner _figureSpawner;
	[SerializeField] private SlotsManager _slotsManager;

	public override void InstallBindings()
	{
		Bind(_figureSpawner);
		Bind(_slotsManager);
		Bind<GameStateMachine>();
		Bind<PhysicManager>();
		Bind<LevelManager>();
	}
}
