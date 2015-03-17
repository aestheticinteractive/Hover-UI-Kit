using System;
using Hover.Common.Items;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public interface ICustomSegment {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Type GetSegmentRenderer(IBaseItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		SegmentSettings GetSegmentSettings(IBaseItem pNavItem);

	}

}
