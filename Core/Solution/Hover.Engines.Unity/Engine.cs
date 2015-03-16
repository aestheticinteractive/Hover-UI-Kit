using UnityEngine;

namespace Hover.Engines.Unity {

	/*================================================================================================*/
	public class Engine : IEngine {

		private static bool IsProviderFilled = FillEngineProvider();

		public string Name { get; private set; }
		public IEngineMath Math { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static bool FillEngineProvider() {
			EngineProvider.SetEngine(new Engine());
			return true;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Engine() {
			Name = "Unity3D";
			Math = new EngineMath();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IContainer FindContainer(string pName) {
			return new ContainerWrap(GameObject.Find(pName));
		}

	}

}
