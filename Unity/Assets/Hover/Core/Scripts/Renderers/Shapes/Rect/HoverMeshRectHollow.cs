using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverMeshRectHollow : HoverMeshRect {

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("InnerSizeType")]
		private SizeType _InnerSizeType = SizeType.Min;

		private SizeType vPrevInnerType;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SizeType InnerSizeType {
			get => _InnerSizeType;
			set => this.UpdateValueWithTreeMessage(ref _InnerSizeType, value, "InnerSizeType");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsMeshVisible {
			get {
				HoverShapeRect shape = GetComponent<HoverShapeRect>();
				float innerProg = GetDimensionProgress(InnerSizeType);
				float outerProg = GetDimensionProgress(OuterSizeType);
				return (shape.SizeX != 0 && shape.SizeY != 0 && outerProg != innerProg);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				InnerSizeType != vPrevInnerType
			);

			vPrevInnerType = InnerSizeType;

			return shouldUpdate;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
			HoverShapeRect shape = GetComponent<HoverShapeRect>();
			float innerProg = GetDimensionProgress(InnerSizeType);
			float outerProg = GetDimensionProgress(OuterSizeType);
			float outerW;
			float outerH;
			float innerW;
			float innerH;

			if ( shape.SizeX >= shape.SizeY ) {
				outerH = shape.SizeY*outerProg;
				innerH = shape.SizeY*innerProg;
				outerW = shape.SizeX-(shape.SizeY-outerH);
				innerW = shape.SizeX-(shape.SizeY-innerH);
			}
			else {
				outerW = shape.SizeX*outerProg;
				innerW = shape.SizeX*innerProg;
				outerH = shape.SizeY-(shape.SizeX-outerW);
				innerH = shape.SizeY-(shape.SizeX-innerW);
			}

			MeshUtil.BuildHollowRectangleMesh(vMeshBuild, outerW, outerH, innerW, innerH);

			UpdateAutoUv(shape, outerW, outerH);
			UpdateMeshUvAndColors();
			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

	}

}
