using Hover.Core.Renderers.Items.Sliders;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverFillSliderRectUpdater : HoverFillSliderUpdater {

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, DisplaySpecials=true)]
		[FormerlySerializedAs("InsetLeft")]
		private float _InsetLeft = 0.01f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("InsetRight")]
		private float _InsetRight = 0.01f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("TickRelativeSizeX")]
		private float _TickRelativeSizeX = 0.5f;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UseTrackUv")]
		private bool _UseTrackUv = false;

		private float vMeshSizeX;
		private float vTickSizeX;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float InsetLeft {
			get => _InsetLeft;
			set => this.UpdateValueWithTreeMessage(ref _InsetLeft, value, "InsetLeft");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InsetRight {
			get => _InsetRight;
			set => this.UpdateValueWithTreeMessage(ref _InsetRight, value, "InsetRight");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TickRelativeSizeX {
			get => _TickRelativeSizeX;
			set => this.UpdateValueWithTreeMessage(ref _TickRelativeSizeX, value, "TickRelativeSizeX");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool UseTrackUv {
			get => _UseTrackUv;
			set => this.UpdateValueWithTreeMessage(ref _UseTrackUv, value, "UseTrackUv");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateFillMeshes() {
			HoverShapeRect shapeRect = gameObject.GetComponent<HoverShapeRect>();

			vMeshSizeX = Mathf.Max(0, shapeRect.SizeX-InsetLeft-InsetRight);

			base.UpdateFillMeshes();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateUnusedFillMesh(HoverMesh pSegmentMesh) {
			HoverShapeRect meshShapeRect = pSegmentMesh.GetComponent<HoverShapeRect>();

			meshShapeRect.Controllers.Set(HoverShapeRect.SizeXName, this);
			meshShapeRect.Controllers.Set(HoverShapeRect.SizeYName, this);

			meshShapeRect.SizeX = vMeshSizeX;
			meshShapeRect.SizeY = 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateUsedFillMesh(HoverMesh pSegmentMesh, 
								SliderUtil.SegmentInfo pSegmentInfo, float pStartPos, float pEndPos) {
			HoverShapeRect meshShapeRect = pSegmentMesh.GetComponent<HoverShapeRect>();
			HoverMeshRect meshRect = (HoverMeshRect)pSegmentMesh;

			pSegmentMesh.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".x", this);
			pSegmentMesh.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".y", this);
			pSegmentMesh.Controllers.Set(HoverMesh.DisplayModeName, this);
			meshShapeRect.Controllers.Set(HoverShapeRect.SizeXName, this);
			meshShapeRect.Controllers.Set(HoverShapeRect.SizeYName, this);
			meshRect.Controllers.Set(HoverMeshRect.UvTopName, this);
			meshRect.Controllers.Set(HoverMeshRect.UvBottomName, this);

			meshShapeRect.SizeX = vMeshSizeX;
			meshShapeRect.SizeY = pSegmentInfo.EndPosition-pSegmentInfo.StartPosition;
			pSegmentMesh.DisplayMode = (pSegmentInfo.IsFill ?
				HoverMesh.DisplayModeType.SliderFill : HoverMesh.DisplayModeType.Standard);
			meshRect.UvTop = (UseTrackUv ?
				Mathf.InverseLerp(pStartPos, pEndPos, pSegmentInfo.StartPosition) : 0);
			meshRect.UvBottom = (UseTrackUv ?
				Mathf.InverseLerp(pStartPos, pEndPos, pSegmentInfo.EndPosition) : 1);

			Vector3 localPos = pSegmentMesh.transform.localPosition;
			localPos.x = (InsetLeft-InsetRight)/2;
			localPos.y = (pSegmentInfo.StartPosition+pSegmentInfo.EndPosition)/2;
			pSegmentMesh.transform.localPosition = localPos;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void ActivateFillMesh(HoverMesh pSegmentMesh) {
			HoverShapeRect meshShapeRect = pSegmentMesh.GetComponent<HoverShapeRect>();

			pSegmentMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			RendererUtil.SetActiveWithUpdate(pSegmentMesh, (meshShapeRect.SizeY > 0));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTickMeshes() {
			HoverShapeRect shapeRect = gameObject.GetComponent<HoverShapeRect>();

			vTickSizeX = Mathf.Max(0, shapeRect.SizeX-InsetLeft-InsetRight)*TickRelativeSizeX;

			base.UpdateTickMeshes();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTickMesh(HoverMesh pTickMesh, SliderUtil.SegmentInfo pTickInfo) {
			HoverShapeRect meshShapeRect = pTickMesh.GetComponent<HoverShapeRect>();

			pTickMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			pTickMesh.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".x", this);
			pTickMesh.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".y", this);
			meshShapeRect.Controllers.Set(HoverShapeRect.SizeXName, this);
			meshShapeRect.Controllers.Set(HoverShapeRect.SizeYName, this);

			meshShapeRect.SizeX = vTickSizeX;
			meshShapeRect.SizeY = pTickInfo.EndPosition-pTickInfo.StartPosition;
			
			Vector3 localPos = pTickMesh.transform.localPosition;
			localPos.x = (InsetLeft-InsetRight)/2;
			localPos.y = (pTickInfo.StartPosition+pTickInfo.EndPosition)/2;
			pTickMesh.transform.localPosition = localPos;

			RendererUtil.SetActiveWithUpdate(pTickMesh, !pTickInfo.IsHidden);
		}

	}

}
