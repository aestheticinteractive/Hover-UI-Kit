using System;
using Hover.Board.Navigation;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public interface ICustomSegment {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Type GetSegmentRenderer(NavItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		ButtonSettings GetSegmentSettings(NavItem pNavItem);

	}

}
