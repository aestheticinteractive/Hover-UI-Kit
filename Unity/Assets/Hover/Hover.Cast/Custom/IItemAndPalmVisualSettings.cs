using System;
using Hover.Common.Custom;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public interface IItemAndPalmVisualSettings : IItemVisualSettings {

		Type PalmRenderer { get; set; }

	}

}
