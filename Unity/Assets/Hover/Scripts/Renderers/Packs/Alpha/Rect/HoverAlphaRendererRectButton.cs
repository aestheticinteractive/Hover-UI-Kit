using Hover.Renderers.Contents;
using Hover.Renderers.Shapes.Rect;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Rect {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverAlphaRendererRectButton : HoverAlphaRendererRect, IRendererRectButton {
	
		public ISettingsController SliderController { get; set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverAlphaFillRectButton Fill;

		[DisableWhenControlled]
		public HoverCanvas Canvas;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			UpdateControl();
			UpdateGeneralSettings();
			UpdateAnchorSettings();

			RendererController = null;
			SliderController = null;
			Controllers.TryExpireControllers();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererUtil.GetNearestWorldPositionOnRectangle(
				pFromWorldPosition, Fill.transform, SizeX, SizeY);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Fill = BuildFill();
			Canvas = BuildCanvas();
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaFillRectButton BuildFill() {
			var rectGo = new GameObject("Fill");
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverAlphaFillRectButton>();
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
			Fill.Controllers.Set(HoverAlphaFillRectButton.AlphaName, this);
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
			float currAlpha = MasterAlpha*(IsEnabled ? EnabledAlpha : DisabledAlpha);

			Fill.SizeX = SizeX;
			Fill.SizeY = SizeY;
			Canvas.SizeX = SizeX-Fill.EdgeThickness*2;
			Canvas.SizeY = SizeY-Fill.EdgeThickness*2;
			
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
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorTypeWithCustom.Custom ) {
				return;
			}
			
			Vector2 anchorPos = RendererUtil.GetRelativeAnchorPosition(Anchor);
			var localPos = new Vector3(SizeX*anchorPos.x, SizeY*anchorPos.y, 0);
			
			Fill.transform.localPosition = localPos;
			Canvas.transform.localPosition = localPos;
		}
		
	}

}
