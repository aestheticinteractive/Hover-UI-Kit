using System;
using System.Collections.Generic;
using Hover.Cast.Renderers.Standard;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Renderers {
	
	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastItemRenderers : MonoBehaviour {

		//TODO: update these with the correct renderer classes
		private static Type DefaultCheckboxType = typeof(HovercastStandardSelectItemRenderer);
		private static Type DefaultParentType = typeof(HovercastStandardSelectItemRenderer);
		private static Type DefaultRadioType = typeof(HovercastStandardSelectItemRenderer);
		private static Type DefaultSelectorType = typeof(HovercastStandardSelectItemRenderer);
		private static Type DefaultSliderType = typeof(HovercastStandardSelectItemRenderer);
		private static Type DefaultStickyType = typeof(HovercastStandardSelectItemRenderer);
		private static Type DefaultTextType = typeof(HovercastStandardSelectItemRenderer);
		
		private static Type CheckboxItemType = typeof(ICheckboxItem);
		private static Type ParentItemType = typeof(IParentItem);
		private static Type RadioItemType = typeof(IRadioItem);
		private static Type SelectorItemType = typeof(ISelectorItem);
		private static Type SliderItemType = typeof(ISliderItem);
		private static Type StickyItemType = typeof(IStickyItem);
		private static Type TextItemType = typeof(ITextItem);

		public Component Checkbox;
		public Component Parent;
		public Component Radio;
		public Component Selector;
		public Component Slider;
		public Component Sticky;
		public Component Text;

		private readonly List<MonoBehaviour> vAllComponents;	
			

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastItemRenderers() {
			vAllComponents = new List<MonoBehaviour>();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.GetComponents(vAllComponents);
			
			Checkbox = null;
			Parent = null;
			Radio = null;
			Selector = null;
			Slider = null;
			Sticky = null;
			Text = null;
			
			foreach ( MonoBehaviour comp in vAllComponents ) {
				TryToFillRendererProperty(comp);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Type GetItemRenderer<T>() where T : IBaseItem {
			return GetItemRenderer(typeof(T));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Type GetItemRenderer(IBaseItem pItem) {
			return GetItemRenderer(pItem.GetType());
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Type GetItemRenderer(Type pItemType) {
			if ( pItemType == CheckboxItemType ) {
				return (Checkbox == null ? DefaultCheckboxType : Checkbox.GetType());
			}
			
			if ( pItemType == ParentItemType ) {
				return (Parent == null ? DefaultParentType : Checkbox.GetType());
			}
			
			if ( pItemType == RadioItemType ) {
				return (Radio == null ? DefaultRadioType : Radio.GetType());
			}
			
			if ( pItemType == SelectorItemType ) {
				return (Selector == null ? DefaultSelectorType : Selector.GetType());
			}
			
			if ( pItemType == SliderItemType ) {
				return (Slider == null ? DefaultSliderType : Slider.GetType());
			}
			
			if ( pItemType == StickyItemType ) {
				return (Sticky == null ? DefaultStickyType : Sticky.GetType());
			}
			
			if ( pItemType == TextItemType ) {
				return (Text == null ? DefaultTextType : Text.GetType());
			}
			
			throw new Exception("Unhandled item type: '"+pItemType+"'.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void TryToFillRendererProperty(MonoBehaviour pComp) {
			if ( !pComp.isActiveAndEnabled ) {
				return;
			}
			
			if ( pComp is IHovercastItemRenderer<ICheckboxItem> ) {
				Checkbox = pComp;
			}
			else if ( pComp is IHovercastItemRenderer<IParentItem> ) {
				Parent = pComp;
			}
			else if ( pComp is IHovercastItemRenderer<IRadioItem> ) {
				Radio = pComp;
			}
			else if ( pComp is IHovercastItemRenderer<ISelectorItem> ) {
				Selector = pComp;
			}
			else if ( pComp is IHovercastItemRenderer<ISliderItem> ) {
				Slider = pComp;
			}
			else if ( pComp is IHovercastItemRenderer<IStickyItem> ) {
				Sticky = pComp;
			}
			else if ( pComp is IHovercastItemRenderer<ITextItem> ) {
				Text = pComp;
			}
		}

	}

}
