using Hover.Common.Renderers.Contents;
using Hover.Common.Renderers.Fills;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverRendererRectangleButton : MonoBehaviour, 
											IProximityProvider, ISettingsController, ITreeUpdateable {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverRendererFillRectangleFromCenter Fill;

		[DisableWhenControlled]
		public HoverRendererCanvas Canvas;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 10;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;
		
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
			UpdateGeneralSettings();
			UpdateAnchorSettings();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererHelper.GetNearestWorldPositionOnRectangle(
				pFromWorldPosition, Fill.transform, SizeX, SizeY);
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
		private void UpdateGeneralSettings() {
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.SizeXName, this);
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.SizeYName, this);
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.AlphaName, this);

			Canvas.Controllers.Set(HoverRendererCanvas.SizeXName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.SizeYName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.AlphaName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.RenderQueueName, this);

			Fill.SizeX = SizeX;
			Fill.SizeY = SizeY;
			Canvas.SizeX = SizeX-Fill.EdgeThickness*2;
			Canvas.SizeY = SizeY-Fill.EdgeThickness*2;
			
			Fill.Alpha = Alpha;
			Canvas.Alpha = Alpha;

			Canvas.RenderQueue = Fill.MaterialRenderQueue+1;
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
