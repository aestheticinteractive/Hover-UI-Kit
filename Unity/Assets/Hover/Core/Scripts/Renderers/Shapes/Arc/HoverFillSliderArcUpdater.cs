using Hover.Core.Renderers.Items.Sliders;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillSliderArcUpdater : HoverFillSliderUpdater {

		[DisableWhenControlled(RangeMin=0, DisplaySpecials=true)]
		public float InsetOuter = 0.01f;

		[DisableWhenControlled(RangeMin=0)]
		public float InsetInner = 0.01f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float TickRelativeSizeX = 0.5f;

		[DisableWhenControlled]
		public bool UseTrackUv = false;

		private float vMeshOuterRadius;
		private float vMeshInnerRadius;
		private float vTickOuterRadius;
		private float vTickInnerRadius;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateFillMeshes() {
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();

			vMeshOuterRadius = shapeArc.OuterRadius-InsetOuter;
			vMeshInnerRadius = shapeArc.InnerRadius+InsetInner;

			base.UpdateFillMeshes();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void ResetFillMesh(HoverMesh pSegmentMesh) {
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();

			meshShapeArc.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.ArcDegreesName, this);

			meshShapeArc.OuterRadius = vMeshOuterRadius;
			meshShapeArc.InnerRadius = vMeshInnerRadius;
			meshShapeArc.ArcDegrees = 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateFillMesh(HoverMesh pSegmentMesh, 
								SliderUtil.SegmentInfo pSegmentInfo, float pStartPos, float pEndPos) {
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();
			HoverMeshArc meshArc = (HoverMeshArc)pSegmentMesh;
			
			pSegmentMesh.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);
			pSegmentMesh.Controllers.Set(HoverMesh.DisplayModeName, this);
			meshArc.Controllers.Set(HoverMeshArc.UvMinArcDegreeName, this);
			meshArc.Controllers.Set(HoverMeshArc.UvMaxArcDegreeName, this);

			meshShapeArc.ArcDegrees = pSegmentInfo.EndPosition-pSegmentInfo.StartPosition;
			pSegmentMesh.DisplayMode = (pSegmentInfo.IsFill ?
				HoverMesh.DisplayModeType.SliderFill : HoverMesh.DisplayModeType.Standard);
			meshArc.UvMinArcDegree = (UseTrackUv ?
				Mathf.InverseLerp(pStartPos, pEndPos, pSegmentInfo.StartPosition) : 0);
			meshArc.UvMaxArcDegree = (UseTrackUv ?
				Mathf.InverseLerp(pStartPos, pEndPos, pSegmentInfo.EndPosition) : 1);

			pSegmentMesh.transform.localRotation = Quaternion.AngleAxis(
				(pSegmentInfo.StartPosition+pSegmentInfo.EndPosition)/2, Vector3.forward);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void ActivateFillMesh(HoverMesh pSegmentMesh) {
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();

			pSegmentMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			RendererUtil.SetActiveWithUpdate(pSegmentMesh, (meshShapeArc.ArcDegrees > 0));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTickMeshes() {
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			float inset = (shapeArc.OuterRadius-shapeArc.InnerRadius-InsetOuter-InsetInner)*
				(1-TickRelativeSizeX)/2;
			
			vTickOuterRadius = shapeArc.OuterRadius-InsetOuter-inset;
			vTickInnerRadius = shapeArc.InnerRadius+InsetInner+inset;

			base.UpdateTickMeshes();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTickMesh(HoverMesh pTickMesh, SliderUtil.SegmentInfo pTickInfo) {
			HoverShapeArc meshShapeArc = pTickMesh.GetComponent<HoverShapeArc>();

			pTickMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			pTickMesh.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.ArcDegreesName, this);

			meshShapeArc.OuterRadius = vTickOuterRadius;
			meshShapeArc.InnerRadius = vTickInnerRadius;
			meshShapeArc.ArcDegrees = pTickInfo.EndPosition-pTickInfo.StartPosition;
			
			pTickMesh.transform.localRotation = Quaternion.AngleAxis(
				(pTickInfo.StartPosition+pTickInfo.EndPosition)/2, Vector3.forward);

			RendererUtil.SetActiveWithUpdate(pTickMesh, !pTickInfo.IsHidden);
		}

	}

}
