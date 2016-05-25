using Hover.Common.Renderers.Contents;
using Hover.Common.Renderers.Shapes.Rect;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverAlphaRendererRectButton : MonoBehaviour, IRendererRectButton,
											IProximityProvider, ISettingsController, ITreeUpdateable {
	
		public const string SizeXName = "_SizeX";
		public const string SizeYName = "_SizeY";
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

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverFillRectButton Fill;

		[SerializeField]
		[DisableWhenControlled]
		public HoverCanvas Canvas;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _SizeX = 10;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _SizeY = 10;
		
		[SerializeField]
		[DisableWhenControlled]
		private bool _IsEnabled = true;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float EnabledAlpha = 1;

		[DisableWhenControlled(RangeMin=0.05f, RangeMax=0.9f)]
		public float DisabledAlpha = 0.35f;
		
		[DisableWhenControlled]
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[SerializeField]
		[DisableWhenControlled]
		private string _SortingLayer = "Default";
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaRendererRectButton() {
			Controllers = new SettingsControllerMap();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SizeX {
			get { return _SizeX; }
			set { _SizeX = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeY {
			get { return _SizeY; }
			set { _SizeY = value; }
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
			UpdateAnchorSettings();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererHelper.GetNearestWorldPositionOnRectangle(
				pFromWorldPosition, Fill.transform, _SizeX, _SizeY);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			Fill = BuildFill();
			Canvas = BuildCanvas();
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverFillRectButton BuildFill() {
			var rectGo = new GameObject("Fill");
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverFillRectButton>();
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
			Fill.Controllers.Set(HoverFillRectButton.SizeXName, this);
			Fill.Controllers.Set(HoverFillRectButton.SizeYName, this);
			Fill.Controllers.Set(HoverFillRectButton.AlphaName, this);
			Fill.Controllers.Set(HoverFill.SortingLayerName, this);
			
			Canvas.Controllers.Set(HoverCanvas.SizeXName, this);
			Canvas.Controllers.Set(HoverCanvas.SizeYName, this);
			Canvas.Controllers.Set(HoverCanvas.AlphaName, this);
			Canvas.Controllers.Set(HoverCanvas.RenderQueueName, this);
			Canvas.Controllers.Set("Canvas.sortingLayer", this);
			
			ISettingsController cont = RendererController;

			if ( cont == null ) {
				return;
			}
			
			Controllers.Set(SizeXName, cont);
			Controllers.Set(SizeYName, cont);
			Controllers.Set(IsEnabledName, cont);
			Controllers.Set(SortingLayerName, cont);

			Fill.Controllers.Set(HoverFillRectButton.HighlightProgressName, cont);
			Fill.Controllers.Set(HoverFillRectButton.SelectionProgressName, cont);
			Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);

			Canvas.Label.Controllers.Set("Text.text", cont);
			Canvas.IconOuter.Controllers.Set(HoverIcon.IconTypeName, cont);
			Canvas.IconInner.Controllers.Set(HoverIcon.IconTypeName, cont);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			float currAlpha = (IsEnabled ? EnabledAlpha : DisabledAlpha);

			Fill.SizeX = SizeX;
			Fill.SizeY = SizeY;
			Canvas.SizeX = SizeX-Fill.EdgeThickness*2;
			Canvas.SizeY = SizeY-Fill.EdgeThickness*2;
			
			Fill.Alpha = currAlpha;
			Canvas.Alpha = currAlpha;

			Fill.SortingLayer = SortingLayer;
			Canvas.CanvasComponent.sortingLayerName = SortingLayer;

			Canvas.Label.TextComponent.text = LabelText;

			Canvas.IconOuter.IconType = IconOuterType;
			Canvas.IconInner.IconType = IconInnerType;

			Fill.HighlightProgress = HighlightProgress;
			Fill.SelectionProgress = SelectionProgress;

			RendererHelper.SetActiveWithUpdate(Fill.Edge, ShowEdge);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorType.Custom ) {
				return;
			}
			
			Vector2 anchorPos = RendererHelper.GetRelativeAnchorPosition(Anchor);
			var localPos = new Vector3(SizeX*anchorPos.x, SizeY*anchorPos.y, 0);
			
			Fill.transform.localPosition = localPos;
			Canvas.transform.localPosition = localPos;
		}
		
	}

}
