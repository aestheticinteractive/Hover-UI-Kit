using System;
using Hoverboard.Core.Navigation;

namespace Hoverboard.Core.Custom {

	/*================================================================================================*/
	public interface ICustomSegment {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Type GetSegmentRenderer(NavItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		ButtonSettings GetSegmentSettings(NavItem pNavItem);

	}

}
