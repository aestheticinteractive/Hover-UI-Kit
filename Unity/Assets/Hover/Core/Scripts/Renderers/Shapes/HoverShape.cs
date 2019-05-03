using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverShape : TreeUpdateableBehavior, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }
		public bool DidSettingsChange { get; protected set; }

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("ControlChildShapes")]
		private bool _ControlChildShapes = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverShape() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool ControlChildShapes {
			get => _ControlChildShapes;
			set => this.UpdateValueWithTreeMessage(ref _ControlChildShapes, value, "CtrlChildShapes");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetCenterWorldPosition();

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast);

		/*--------------------------------------------------------------------------------------------*/
		public abstract float GetSliderValueViaNearestWorldPosition(Vector3 pNearestWorldPosition, 
			Transform pSliderContainerTx, HoverShape pHandleButtonShape);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			DidSettingsChange = false;
			Controllers.TryExpireControllers();
		}

	}

}
