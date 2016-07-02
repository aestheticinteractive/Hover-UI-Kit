using System.Collections.Generic;
using Hover.Renderers.Elements.Sliders;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements.Shapes.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverFillSlider))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillSliderArcUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(RangeMin=0, DisplaySpecials=true)]
		public float InsetOuter = 0.01f;

		[DisableWhenControlled(RangeMin=0)]
		public float InsetInner = 0.01f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float TickRelativeSizeX = 0.5f;

		[DisableWhenControlled]
		public bool UseTrackUv = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverFillSliderArcUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateFillMeshes();
			UpdateTickMeshes();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateFillMeshes() {
			HoverFillSlider fillSlider = gameObject.GetComponent<HoverFillSlider>();
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			List<SliderUtil.SegmentInfo> segInfoList = fillSlider.SegmentInfo.SegmentInfoList;
			int segCount = fillSlider.GetChildMeshCount();
			int segIndex = 0;
			float startPos = segInfoList[0].StartPosition;
			float endPos = segInfoList[segInfoList.Count-1].EndPosition;

			for ( int i = 0 ; i < segCount ; i++ ) {
				ResetFillMesh(fillSlider.GetChildMesh(i), shapeArc);
			}

			for ( int i = 0 ; i < segInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = segInfoList[i];

				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				UpdateFillMesh(fillSlider.GetChildMesh(segIndex++), segInfo, startPos, endPos);
			}

			for ( int i = 0 ; i < segCount ; i++ ) {
				ActivateFillMesh(fillSlider.GetChildMesh(i));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void ResetFillMesh(HoverMesh pSegmentMesh, HoverShapeArc pShapeArc) {
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();

			meshShapeArc.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.ArcDegreesName, this);

			meshShapeArc.OuterRadius = pShapeArc.OuterRadius-InsetOuter;
			meshShapeArc.InnerRadius = pShapeArc.InnerRadius+InsetInner;
			meshShapeArc.ArcDegrees = 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateFillMesh(HoverMesh pSegmentMesh, SliderUtil.SegmentInfo pSegmentInfo,
																	float pStartPos, float pEndPos) {
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
		private void ActivateFillMesh(HoverMesh pSegmentMesh) {
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();

			pSegmentMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			RendererUtil.SetActiveWithUpdate(pSegmentMesh, (meshShapeArc.ArcDegrees > 0));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTickMeshes() {
			HoverFillSlider fillSlider = gameObject.GetComponent<HoverFillSlider>();
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			List<SliderUtil.SegmentInfo> tickInfoList = fillSlider.SegmentInfo.TickInfoList;
			float tickInset = (shapeArc.OuterRadius-shapeArc.InnerRadius-InsetOuter-InsetInner)*
				(1-TickRelativeSizeX)/2;
			float tickOuterRadius = shapeArc.OuterRadius-InsetOuter-tickInset;
			float tickInnerRadius = shapeArc.InnerRadius+InsetInner+tickInset;

			for ( int i = 0 ; i < tickInfoList.Count ; i++ ) {
				UpdateTickMesh(fillSlider.Ticks[i], tickInfoList[i], tickOuterRadius, tickInnerRadius);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTickMesh(HoverMesh pTickMesh, SliderUtil.SegmentInfo pTickInfo,
																float pOuterRadius, float pInnerRadius) {
			HoverShapeArc meshShapeArc = pTickMesh.GetComponent<HoverShapeArc>();

			pTickMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			pTickMesh.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.ArcDegreesName, this);

			meshShapeArc.OuterRadius = pOuterRadius;
			meshShapeArc.InnerRadius = pInnerRadius;
			meshShapeArc.ArcDegrees = pTickInfo.EndPosition-pTickInfo.StartPosition;
			
			pTickMesh.transform.localRotation = Quaternion.AngleAxis(
				(pTickInfo.StartPosition+pTickInfo.EndPosition)/2, Vector3.forward);

			RendererUtil.SetActiveWithUpdate(pTickMesh, !pTickInfo.IsHidden);
		}

	}

}
