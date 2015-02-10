using System;
using Hovercast.Core.Navigation;

namespace Hovercast.Core.Custom {

	/*================================================================================================*/
	public interface ICustomSegment {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Type GetSegmentRenderer(NavItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		SegmentSettings GetSegmentSettings(NavItem pNavItem);

	}

}
