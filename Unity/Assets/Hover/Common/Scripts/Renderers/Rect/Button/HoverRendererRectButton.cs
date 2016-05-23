using Hover.Common.Renderers.Shared.Bases;
using Hover.Common.Renderers.Shared.Contents;
using Hover.Common.Renderers.Shared.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Rect.Button {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverRendererRectButton : MonoBehaviour, IHoverRendererRectButton,
											IProximityProvider, ISettingsController, ITreeUpdateable {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";
		public const string SortingLayerName = "SortingLayer";

		public ISettingsController RendererController { get; set; }
		public ISettingsControllerMap Controllers { get; private set; }
		public string LabelText { get; set; }
		public HoverRendererIcon.IconOffset IconOuterType { get; set; }
		public HoverRendererIcon.IconOffset IconInnerType { get; set; }
		public float HighlightProgress { get; set; }
		public float SelectionProgress { get; set; }
		public bool ShowEdge { get; set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverRendererRectFillFromCenter Fill;

		[SerializeField]
		[DisableWhenControlled]
		public HoverRendererCanvas Canvas;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _SizeX = 10;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _SizeY = 10;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		private float _Alpha = 1;
		
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
		public HoverRendererRectButton() {
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
		public float Alpha {
			get { return _Alpha; }
			set { _Alpha = value; }
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
		private HoverRendererRectFillFromCenter BuildFill() {
			var rectGo = new GameObject("Fill");
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererRectFillFromCenter>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererCanvas BuildCanvas() {
			var canvasGo = new GameObject("Canvas");
			canvasGo.transform.SetParent(gameObject.transform, false);
			return canvasGo.AddComponent<HoverRendererCanvas>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateControl() {
			Fill.Controllers.Set(HoverRendererRectFillFromCenter.SizeXName, this);
			Fill.Controllers.Set(HoverRendererRectFillFromCenter.SizeYName, this);
			Fill.Controllers.Set(HoverRendererRectFillFromCenter.AlphaName, this);
			Fill.Controllers.Set(HoverRendererFill.SortingLayerName, this);
			
			Canvas.Controllers.Set(HoverRendererCanvas.SizeXName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.SizeYName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.AlphaName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.RenderQueueName, this);
			Canvas.Controllers.Set("Canvas.sortingLayer", this);
			
			ISettingsController cont = RendererController;

			if ( cont == null ) {
				return;
			}
			
			Controllers.Set(SizeXName, cont);
			Controllers.Set(SizeYName, cont);
			Controllers.Set(AlphaName, cont);
			Controllers.Set(SortingLayerName, cont);

			Fill.Controllers.Set(HoverRendererRectFillFromCenter.HighlightProgressName, cont);
			Fill.Controllers.Set(HoverRendererRectFillFromCenter.SelectionProgressName, cont);
			Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);

			Canvas.Label.Controllers.Set("Text.text", cont);
			Canvas.IconOuter.Controllers.Set(HoverRendererIcon.IconTypeName, cont);
			Canvas.IconInner.Controllers.Set(HoverRendererIcon.IconTypeName, cont);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			Fill.SizeX = SizeX;
			Fill.SizeY = SizeY;
			Canvas.SizeX = SizeX-Fill.EdgeThickness*2;
			Canvas.SizeY = SizeY-Fill.EdgeThickness*2;
			
			Fill.Alpha = Alpha;
			Canvas.Alpha = Alpha;

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
