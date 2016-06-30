using Hover.Items;
using Hover.Renderers.Contents;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public abstract class HoverRenderer : MonoBehaviour, ITreeUpdateable, 
															ISettingsController, IGameObjectProvider {

		public const string IsEnabledName = "IsEnabled";

		public ISettingsControllerMap Controllers { get; private set; }
		public bool IsNearestToCursor { get; set; }
		
		[DisableWhenControlled(DisplayMessage=true)]
		public bool IsEnabled = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRenderer() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverIndicator GetIndicator() {
			return gameObject.GetComponent<HoverIndicator>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverShape GetShape() {
			return gameObject.GetComponent<HoverShape>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract int GetChildFillCount();

		/*--------------------------------------------------------------------------------------------*/
		public abstract HoverFill GetChildFill(int pIndex);
		
		/*--------------------------------------------------------------------------------------------*/
		public abstract HoverCanvas GetCanvas();

		/*--------------------------------------------------------------------------------------------*/
		public abstract HoverCanvasDataUpdater GetCanvasDataUpdater();

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			Controllers.TryExpireControllers();
		}

	}

}
