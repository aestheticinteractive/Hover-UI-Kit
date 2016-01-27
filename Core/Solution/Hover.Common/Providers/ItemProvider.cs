using System;
using Hover.Common.Items;
using Hover.Common.Renderers;
using Hover.Common.State;
using Hover.Common.Styles;

namespace Hover.Common.Providers {

	/*================================================================================================*/
	public class ItemProvider : IItemProvider {

		public IBaseItem Item { get; private set; }
		public IBaseItemState State { get; private set; }
		
		private readonly Func<IBaseItem, IItemStyle> vGetDefaultStyle;
		private readonly Func<IItemStyle> vGetItemStyle;
		private readonly Func<IBaseItem, Type> vGetDefaultRendererType;
		private readonly Func<IHoverItemRenderer> vGetItemRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemProvider(IBaseItem pItem, IBaseItemState pState,
						Func<IBaseItem, IItemStyle> pGetDefaultStyle, Func<IItemStyle> pGetItemStyle,
						Func<IBaseItem, Type> pGetDefaultRendererType,
						Func<IHoverItemRenderer> pGetItemRenderer) {
			Item = pItem;
			State = pState;
			
			vGetDefaultStyle = pGetDefaultStyle;
			vGetItemStyle = pGetItemStyle;
			vGetDefaultRendererType = pGetDefaultRendererType;
			vGetItemRenderer = pGetItemRenderer;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemStyle Style {
			get {
				IItemStyle itemStyle = vGetItemStyle();
				return (itemStyle == null ? vGetDefaultStyle(Item) : itemStyle);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IHoverItemRenderer Renderer {
			get {
				return vGetItemRenderer();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Type RendererType {
			get {
				return (Renderer == null ? vGetDefaultRendererType(Item) : null);
			}
		}

	}

}
