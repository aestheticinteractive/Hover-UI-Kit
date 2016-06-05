using Hover.Common.Renderers.Contents;
using Hover.Common.Renderers.Shapes.Arc;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverAlphaRendererArcButton : MonoBehaviour, IRendererArcButton,
											IProximityProvider, ISettingsController, ITreeUpdateable {
	
		public const string OuterRadiusName = "_OuterRadius";
		public const string InnerRadiusName = "_InnerRadius";
		public const string ArcAngleName = "_ArcAngle";
		public const string IsEnabledName = "_IsEnabled";
		public const string EnabledAlphaName = "EnabledAlpha";
		public const string DisabledAlphaName = "DisabledAlpha";
		public const string SortingLayerName = "_SortingLayer";

		public ISettingsController RendererController { get; set; }
		public ISettingsController SliderController { get; set; }
		public ISettingsControllerMap Controllers { get; private set; }
		public string LabelText { get; set; }
		public HoverIcon.IconOffset IconOuterType { get; set; }
		public HoverIcon.IconOffset IconInnerType { get; set; }
		public float HighlightProgress { get; set; }
		public float SelectionProgress { get; set; }
		public bool ShowEdge { get; set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverAlphaFillArcButton Fill;

		[SerializeField]
		[DisableWhenControlled]
		public HoverCanvas Canvas;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _OuterRadius = 10;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _InnerRadius = 4;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		private float _ArcAngle = 60;
		
		[SerializeField]
		[DisableWhenControlled]
		private bool _IsEnabled = true;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float EnabledAlpha = 1;

		[DisableWhenControlled(RangeMin=0.05f, RangeMax=0.9f)]
		public float DisabledAlpha = 0.35f;
		
		[SerializeField]
		[DisableWhenControlled]
		private string _SortingLayer = "Default";
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaRendererArcButton() {
			Controllers = new SettingsControllerMap();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float OuterRadius {
			get { return _OuterRadius; }
			set { _OuterRadius = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InnerRadius {
			get { return _InnerRadius; }
			set { _InnerRadius = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float ArcAngle {
			get { return _ArcAngle; }
			set { _ArcAngle = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get { return _IsEnabled; }
			set { _IsEnabled = value; }
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
		public void TreeUpdate() {
			UpdateControl();
			UpdateGeneralSettings();

			RendererController = null;
			SliderController = null;
			Controllers.TryExpireControllers();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererUtil.GetNearestWorldPositionOnArc(
				pFromWorldPosition, Fill.transform, _OuterRadius, _InnerRadius, _ArcAngle);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			Fill = BuildFill();
			Canvas = BuildCanvas();

			Canvas.SizeY = 3; //TODO: SizeY based on ArcAngle?
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaFillArcButton BuildFill() {
			var ArcGo = new GameObject("Fill");
			ArcGo.transform.SetParent(gameObject.transform, false);
			return ArcGo.AddComponent<HoverAlphaFillArcButton>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverCanvas BuildCanvas() {
			var canvasGo = new GameObject("Canvas");
			canvasGo.transform.SetParent(gameObject.transform, false);
			return canvasGo.AddComponent<HoverCanvas>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateControl() {
			Fill.Controllers.Set(HoverFillArcButton.OuterRadiusName, this);
			Fill.Controllers.Set(HoverFillArcButton.InnerRadiusName, this);
			Fill.Controllers.Set(HoverFillArcButton.ArcAngleName, this);
			Fill.Controllers.Set(HoverAlphaFillArcButton.AlphaName, this);
			Fill.Controllers.Set(HoverFill.SortingLayerName, this);
			
			Canvas.Controllers.Set(HoverCanvas.SizeXName, this);
			Canvas.Controllers.Set(HoverCanvas.AlphaName, this);
			Canvas.Controllers.Set(HoverCanvas.RenderQueueName, this);
			Canvas.Controllers.Set("Canvas.sortingLayer", this);
			Canvas.Controllers.Set("Transform.localPosition.x", this);
			
			ISettingsController cont = RendererController;

			if ( cont == null ) {
				return;
			}
			
			Controllers.Set(OuterRadiusName, cont);
			Controllers.Set(InnerRadiusName, cont);
			Controllers.Set(ArcAngleName, cont);
			Controllers.Set(IsEnabledName, cont);
			Controllers.Set(SortingLayerName, cont);

			Fill.Controllers.Set(HoverFillArcButton.HighlightProgressName, cont);
			Fill.Controllers.Set(HoverFillArcButton.SelectionProgressName, cont);
			Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);

			Canvas.Label.Controllers.Set("Text.text", cont);
			Canvas.IconOuter.Controllers.Set(HoverIcon.IconTypeName, cont);
			Canvas.IconInner.Controllers.Set(HoverIcon.IconTypeName, cont);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			float currAlpha = (IsEnabled ? EnabledAlpha : DisabledAlpha);

			Fill.OuterRadius = OuterRadius;
			Fill.InnerRadius = InnerRadius;
			Fill.ArcAngle = ArcAngle;

			Canvas.SizeX = OuterRadius-InnerRadius-Fill.EdgeThickness*2;

			Vector3 canvasLocalPos = Canvas.transform.localPosition;
			canvasLocalPos.x = InnerRadius+Fill.EdgeThickness+Canvas.SizeX/2;
			Canvas.transform.localPosition = canvasLocalPos;
			
			Fill.Alpha = currAlpha;
			Canvas.Alpha = currAlpha;

			Fill.SortingLayer = SortingLayer;
			Canvas.CanvasComponent.sortingLayerName = SortingLayer;

			if ( RendererController == null && SliderController == null ) {
				return;
			}

			Canvas.Label.TextComponent.text = LabelText;
			
			Canvas.IconOuter.IconType = IconOuterType;
			Canvas.IconInner.IconType = IconInnerType;

			Fill.HighlightProgress = HighlightProgress;
			Fill.SelectionProgress = SelectionProgress;

			RendererUtil.SetActiveWithUpdate(Fill.Edge, ShowEdge);
		}
		
	}

}
