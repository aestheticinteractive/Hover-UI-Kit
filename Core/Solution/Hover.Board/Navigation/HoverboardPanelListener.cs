using System;
using UnityEngine;

namespace Hover.Board.Navigation {

	/*================================================================================================*/
	public abstract class HoverboardPanelListener : MonoBehaviour {

		public HoverboardPanelProvider Component { get; private set; }
		public NavPanel Panel { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Component = gameObject.GetComponent<HoverboardPanelProvider>();

			if ( Component == null ) {
				throw new Exception("There must be a "+typeof(HoverboardPanelProvider).Name+
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
