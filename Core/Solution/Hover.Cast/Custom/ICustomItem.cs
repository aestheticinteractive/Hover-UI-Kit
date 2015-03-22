using System;
using Hover.Common.Items;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public interface ICustomItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Type GetSegmentRenderer(IBaseItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		ItemVisualSettings GetSegmentSettings(IBaseItem pNavItem);

	}

}
