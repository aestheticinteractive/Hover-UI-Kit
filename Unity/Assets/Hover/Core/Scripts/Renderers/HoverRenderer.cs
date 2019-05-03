using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public abstract class HoverRenderer : TreeUpdateableBehavior,
															ISettingsController, IGameObjectProvider {

		public const string IsEnabledName = "IsEnabled";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("IsEnabled")]
		private bool _IsEnabled = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRenderer() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get => _IsEnabled;
			set => this.UpdateValueWithTreeMessage(ref _IsEnabled, value, "IsEnabled");
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
		public abstract int GetChildRendererCount();

		/*--------------------------------------------------------------------------------------------*/
		public abstract HoverRenderer GetChildRenderer(int pIndex);
		
		/*--------------------------------------------------------------------------------------------*/
		public abstract HoverCanvas GetCanvas();

		/*--------------------------------------------------------------------------------------------*/
		public abstract HoverCanvasDataUpdater GetCanvasDataUpdater();

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetCenterWorldPosition();

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			Controllers.TryExpireControllers();
		}

	}

}
