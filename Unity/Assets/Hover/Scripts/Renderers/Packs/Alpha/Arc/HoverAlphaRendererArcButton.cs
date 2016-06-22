using Hover.Renderers.Contents;
using Hover.Renderers.Shapes.Arc;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	public class HoverAlphaRendererArcButton : HoverAlphaRendererArc, IRendererArcButton {
	
		public ISettingsController SliderController { get; set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverAlphaFillArcButton Fill;

		[SerializeField]
		[DisableWhenControlled]
		public HoverCanvas Canvas;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			UpdateControl();
			UpdateGeneralSettings();

			RendererController = null;
			SliderController = null;
			Controllers.TryExpireControllers();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererUtil.GetNearestWorldPositionOnArc(
				pFromWorldPosition, Fill.transform, OuterRadius, InnerRadius, ArcAngle);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Fill = BuildFill();
			Canvas = BuildCanvas();

			Canvas.SizeY = 3; //TODO: SizeY based on ArcAngle?
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaFillArcButton BuildFill() {
			var arcGo = new GameObject("Fill");
			arcGo.transform.SetParent(gameObject.transform, false);
			return arcGo.AddComponent<HoverAlphaFillArcButton>();
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
