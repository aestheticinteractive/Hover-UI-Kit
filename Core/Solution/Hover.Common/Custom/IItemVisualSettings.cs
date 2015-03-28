using System;

namespace Hover.Common.Custom {

	/*================================================================================================*/
	public interface IItemVisualSettings {

		Type Renderer { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void FillWith(IItemVisualSettings pSourceSettings, bool pIncludeRenderer);

	}

}
