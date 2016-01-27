using System;
using Hover.Common.Items;
using Hover.Common.Renderers;
using Hover.Common.State;
using Hover.Common.Styles;

namespace Hover.Common.Providers {

	/*================================================================================================*/
	public interface IItemProvider {

		IBaseItem Item { get; }
		IBaseItemState State { get; }
		IItemStyle Style { get; }
		IHoverItemRenderer Renderer { get; }
		Type RendererType { get; }

	}

}
