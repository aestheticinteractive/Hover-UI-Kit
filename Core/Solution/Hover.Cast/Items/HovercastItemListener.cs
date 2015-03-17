using System;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Items {

	/*================================================================================================*/
	public abstract class HovercastItemListener<T> : MonoBehaviour where T : IBaseItem {

		public HovercastItem Component { get; private set; }
		public T Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Component = gameObject.GetComponent<HovercastItem>();

			if ( Component == null ) {
				throw new Exception("There must be a "+typeof(HovercastItem).Name+" component "+
					"attached to this GameObject.");
			}

			Item = (T)Component.GetItemData().Item;
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
