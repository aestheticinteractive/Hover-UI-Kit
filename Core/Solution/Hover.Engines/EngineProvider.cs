using System;

namespace Hover.Engines {

	/*================================================================================================*/
	public static class EngineProvider {

		public static IEngine Engine { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void SetEngine(IEngine pEngine) {
			if ( Engine != null ) {
				throw new Exception("EngineProvder.Engine is already set!");
			}

			Engine = pEngine;
		}

	}

}
