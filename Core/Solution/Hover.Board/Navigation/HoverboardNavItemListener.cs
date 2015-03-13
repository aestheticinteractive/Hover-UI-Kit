using System;
using UnityEngine;

namespace Hover.Board.Navigation {

	/*================================================================================================*/
	public abstract class HoverboardNavItemListener<T> : MonoBehaviour where T : NavItem {

		public HoverboardNavItem Component { get; private set; }
		public T Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Component = gameObject.GetComponent<HoverboardNavItem>();

			if ( Component == null ) {
				throw new Exception("There must be a "+typeof(HoverboardNavItem).Name+" component "+
					"attached to this GameObject.");
			}

			Item = (T)Component.GetItem();
			Setup();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			BroadcastInitialValue();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void Setup();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void BroadcastInitialValue();

	}

}
