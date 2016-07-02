using System.Collections.Generic;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverFillSlider))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillSliderArcUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(RangeMin=0, DisplayMessage=true)]
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
			UpdateMeshes();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshes() {
			HoverFillSlider fillSlider = gameObject.GetComponent<HoverFillSlider>();
			List<SliderUtil.SegmentInfo> segInfoList = fillSlider.SegmentInfo.SegmentInfoList;
			int segCount = fillSlider.GetChildMeshCount();
			int segIndex = 0;
			float startPos = segInfoList[0].StartPosition;
			float endPos = segInfoList[segInfoList.Count-1].EndPosition;

			for ( int i = 0 ; i < segCount ; i++ ) {
				ResetMesh(fillSlider.GetChildMesh(i));
			}

			for ( int i = 0 ; i < segInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = segInfoList[i];

				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				UpdateMesh(fillSlider.GetChildMesh(segIndex++), segInfo, startPos, endPos);
			}

			for ( int i = 0 ; i < segCount ; i++ ) {
				ActivateMesh(fillSlider.GetChildMesh(i));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void ResetMesh(HoverMesh pSegmentMesh) {
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();
			HoverMeshArc meshArc = (HoverMeshArc)pSegmentMesh;

			pSegmentMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			pSegmentMesh.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);
			pSegmentMesh.Controllers.Set(HoverMesh.DisplayModeName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShapeArc.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshArc.Controllers.Set(HoverMeshArc.UvMinArcDegreeName, this);
			meshArc.Controllers.Set(HoverMeshArc.UvMaxArcDegreeName, this);

			meshShapeArc.OuterRadius = shapeArc.OuterRadius-InsetOuter;
			meshShapeArc.InnerRadius = shapeArc.InnerRadius+InsetInner;
			meshShapeArc.ArcDegrees = 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMesh(HoverMesh pSegmentMesh, SliderUtil.SegmentInfo pSegmentInfo,
																	float pStartPos, float pEndPos) {
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();
			HoverMeshArc meshArc = (HoverMeshArc)pSegmentMesh;

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
		private void ActivateMesh(HoverMesh pSegmentMesh) {
			HoverShapeArc meshShapeArc = pSegmentMesh.GetComponent<HoverShapeArc>();
			RendererUtil.SetActiveWithUpdate(pSegmentMesh, (meshShapeArc.ArcDegrees > 0));
		}

	}

}
