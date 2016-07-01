using Hover.Items;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverShape : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }
		public bool DidSettingsChange { get; protected set; }
		
		[DisableWhenControlled(DisplayMessage=true)]
		public bool ControlChildShapes = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverShape() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast);
		
		/*--------------------------------------------------------------------------------------------*/
		public abstract float GetSliderValueViaNearestWorldPosition(Vector3 pNearestWorldPosition, 
			Transform pSliderContainerTx, HoverShape pHandleButtonShape);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			DidSettingsChange = false;
			Controllers.TryExpireControllers();
		}

	}

}
