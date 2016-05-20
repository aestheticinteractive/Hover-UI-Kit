using Hover.Common.Renderers.Contents;
using Hover.Common.Renderers.Fills;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverRendererRectangleButton : MonoBehaviour, IHoverRendererRectangleButton,
											IProximityProvider, ISettingsController, ITreeUpdateable {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";

		public ISettingsController RendererController { get; set; }
		public ISettingsControllerMap Controllers { get; private set; }
		public string LabelText { get; set; }
		public HoverRendererIcon.IconOffset IconOuterType { get; set; }
		public HoverRendererIcon.IconOffset IconInnerType { get; set; }
		public float HighlightProgress { get; set; }
		public float SelectionProgress { get; set; }
		public bool ShowEdge { get; set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverRendererFillRectangleFromCenter Fill;

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
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererRectangleButton() {
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
		private HoverRendererFillRectangleFromCenter BuildFill() {
			var rectGo = new GameObject("Fill");
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererFillRectangleFromCenter>();
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
			ISettingsController cont = RendererController;

			Controllers.Set(SizeXName, cont);
			Controllers.Set(SizeYName, cont);
			Controllers.Set(AlphaName, cont);

			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.HighlightProgressName, cont);
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.SelectionProgressName, cont);
			Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);

			Canvas.Label.Controllers.Set("Text.text", cont);
			Canvas.IconOuter.Controllers.Set(HoverRendererIcon.IconTypeName, cont);
			Canvas.IconInner.Controllers.Set(HoverRendererIcon.IconTypeName, cont);
			
			////

			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.SizeXName, this);
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.SizeYName, this);
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.AlphaName, this);

			Canvas.Controllers.Set(HoverRendererCanvas.SizeXName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.SizeYName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.AlphaName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.RenderQueueName, this);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			Fill.SizeX = _SizeX;
			Fill.SizeY = _SizeY;
			Canvas.SizeX = _SizeX-Fill.EdgeThickness*2;
			Canvas.SizeY = _SizeY-Fill.EdgeThickness*2;
			
			Fill.Alpha = _Alpha;
			Canvas.Alpha = _Alpha;

			Canvas.RenderQueue = Fill.MaterialRenderQueue+1;

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
			var localPos = new Vector3(_SizeX*anchorPos.x, _SizeY*anchorPos.y, 0);
			
			Fill.transform.localPosition = localPos;
			Canvas.transform.localPosition = localPos;
		}
		
	}

}
