using System;
using UnityEngine;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public abstract class HoverboardPanelListener : MonoBehaviour {

		public HoverboardPanel Component { get; private set; }
		public ItemPanel Panel { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Component = gameObject.GetComponent<HoverboardPanel>();

			if ( Component == null ) {
				throw new Exception("There must be a "+typeof(HoverboardPanel).Name+
					" component attached to this GameObject.");
			}

			Panel = Component.GetPanel();
			Setup();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			BroadcastInitialValues();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void Setup();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void BroadcastInitialValues();

	}

}
