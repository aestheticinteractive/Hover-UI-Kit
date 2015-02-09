using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {
	
	/*================================================================================================*/
	public abstract class HovercastNavItem : MonoBehaviour {
		
		public string Label = "";
		public float RelativeSize = 1;
		public bool NavigateBackUponSelect;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal abstract NavItem GetGenericItem();

	}


	/*================================================================================================*/
	public abstract class HovercastNavItem<N> : HovercastNavItem where N : NavItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override NavItem GetGenericItem() {
			return GetItem();
		}

		/*--------------------------------------------------------------------------------------------*/
		internal abstract N GetItem();
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void FillItem(N pItem) {
			pItem.Label = (string.IsNullOrEmpty(Label) ? gameObject.name : Label);
			pItem.RelativeSize = RelativeSize;
			pItem.NavigateBackUponSelect = NavigateBackUponSelect;
		}

	}


	/*================================================================================================*/
	public abstract class HovercastNavItem<N, T> : HovercastNavItem<N> 
														where N : NavItem<T> where T : IComparable  {

		public T Value;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void FillItem(N pItem) {
			base.FillItem(pItem);
			pItem.Value = Value;
		}

	}

}
