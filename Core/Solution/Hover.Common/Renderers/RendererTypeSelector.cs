using System;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Renderers {
	
	/*================================================================================================*/
	public class RendererTypeSelector {

		private static readonly Type CheckboxItemType = typeof(ICheckboxItem);
		private static readonly Type ParentItemType = typeof(IParentItem);
		private static readonly Type RadioItemType = typeof(IRadioItem);
		private static readonly Type SelectorItemType = typeof(ISelectorItem);
		private static readonly Type SliderItemType = typeof(ISliderItem);
		private static readonly Type StickyItemType = typeof(IStickyItem);
		private static readonly Type TextItemType = typeof(ITextItem);

		private readonly Type vCheckbox;
		private readonly Type vParent;
		private readonly Type vRadio;
		private readonly Type vSelector;
		private readonly Type vSlider;
		private readonly Type vSticky;
		private readonly Type vText;
			

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RendererTypeSelector(Type pCheckbox, Type pParent, Type pRadio, Type pSelector, 
															Type pSlider, Type pSticky, Type pText) {
			vCheckbox = ConfirmComponentType(pCheckbox);
			vParent = ConfirmComponentType(pParent);
			vRadio = ConfirmComponentType(pRadio);
			vSelector = ConfirmComponentType(pSelector);
			vSlider = ConfirmComponentType(pSlider);
			vSticky = ConfirmComponentType(pSticky);
			vText = ConfirmComponentType(pText);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetItemRendererType<T>() where T : IBaseItem {
			return GetItemRenderer(typeof(T));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Type GetItemRendererType(IBaseItem pItem) {
			return GetItemRenderer(pItem.GetType());
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Type ConfirmComponentType(Type pRendererType) {
			if ( typeof(Component).IsAssignableFrom(pRendererType) ) {
				return pRendererType;
			}

			throw new Exception("Renderer type '"+pRendererType.Name+"' is not a Unity component.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Type GetItemRenderer(Type pItemType) {
			if ( pItemType == CheckboxItemType ) {
				return vCheckbox;
			}
			
			if ( pItemType == ParentItemType ) {
				return vParent;
			}
			
			if ( pItemType == RadioItemType ) {
				return vRadio;
			}
			
			if ( pItemType == SelectorItemType ) {
				return vSelector;
			}
			
			if ( pItemType == SliderItemType ) {
				return vSlider;
			}
			
			if ( pItemType == StickyItemType ) {
				return vSticky;
			}
			
			if ( pItemType == TextItemType ) {
				return vText;
			}
			
			throw new Exception("Unhandled item type: '"+pItemType+"'.");
		}

	}

}
