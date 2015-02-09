using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public abstract class HovercastNavItemListener<T> : MonoBehaviour where T : NavItem {

		public HovercastNavItem Component { get; private set; }
		public T Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Component = gameObject.GetComponent<HovercastNavItem>();

			if ( Component == null ) {
				throw new Exception("There must be a "+typeof(HovercastNavItem).Name+" component "+
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
