using Hover.Items;
using Hover.Renderers.Contents;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverAlphaRenderer : MonoBehaviour,
											IProximityProvider, ISettingsController, ITreeUpdateable {
	
		public const string IsEnabledName = "_IsEnabled";
		public const string EnabledAlphaName = "EnabledAlpha";
		public const string DisabledAlphaName = "DisabledAlpha";
		public const string SortingLayerName = "_SortingLayer";

		public ISettingsController RendererController { get; set; }
		public ISettingsControllerMap Controllers { get; private set; }
		public string LabelText { get; set; }
		public HoverIcon.IconOffset IconOuterType { get; set; }
		public HoverIcon.IconOffset IconInnerType { get; set; }
		public float HighlightProgress { get; set; }
		public float SelectionProgress { get; set; }
		public bool ShowEdge { get; set; }
		
		[SerializeField]
		[DisableWhenControlled]
		private bool _IsEnabled = true;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		private float _EnabledAlpha = 1;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		private float _DisabledAlpha = 0.35f;
		
		[SerializeField]
		[DisableWhenControlled]
		private string _SortingLayer = "Default";
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaRenderer() {
			Controllers = new SettingsControllerMap();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get { return _IsEnabled; }
			set { _IsEnabled = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float EnabledAlpha {
			get { return _EnabledAlpha; }
			set { _EnabledAlpha = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float DisabledAlpha {
			get { return _DisabledAlpha; }
			set { _DisabledAlpha = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public string SortingLayer {
			get { return _SortingLayer; }
			set { _SortingLayer = value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				BuildElements();
				_IsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public abstract void TreeUpdate();
		
		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast);
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void BuildElements();

	}

}
