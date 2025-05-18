using Figures;
using Installers;
using UnityEngine;

public class MainSceneInstaller : BaseInstaller
{
	[SerializeField] private FiguresSpawner _figureSpawner;

	public override void InstallBindings()
	{
		Bind(_figureSpawner);

		Bind<PhysicManager>();
	}
}
