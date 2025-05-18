using Figures;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "Configs Installer", menuName = "Installers/Configs Installer")]
	public class ConfigsInstaller : ScriptableObjectInstaller
	{
		[SerializeField] private FormsHolder _formsHolder;
		[SerializeField] private IconsHolder _iconsHolder;
		[SerializeField] private LevelsHandler _levelsHandler;

		public override void InstallBindings()
		{
			Bind(_formsHolder);
			Bind(_iconsHolder);
			Bind(_levelsHandler);
		}

		private void Bind<T>(T instance) where T : ScriptableObject
		{
			if (typeof(T).GetInterfaces().Any())
			{
				Container.BindInterfacesAndSelfTo<T>().FromInstance(instance);
				return;
			}

			Container.BindInstance(instance);
		}
	}
}