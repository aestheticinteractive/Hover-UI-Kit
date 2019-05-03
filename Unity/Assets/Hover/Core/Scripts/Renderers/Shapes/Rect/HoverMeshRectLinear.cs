using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverMeshRectLinear : HoverMeshRect {

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("InnerSizeType")]
		private SizeType _InnerSizeType = SizeType.Min;

		public bool IsVertical = false;

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
			float originProg = (Mathf.Lerp(innerProg, outerProg, 0.5f)-0.5f);
			float sizeX = shape.SizeX;
			float sizeY = shape.SizeY;
			float originX = 0;
			float originY = 0;

			if ( IsVertical ) {
				sizeY = shape.SizeY*(outerProg-innerProg);
				originY = shape.SizeY*originProg;
			}
			else {
				sizeX = shape.SizeX*(outerProg-innerProg);
				originX = shape.SizeX*originProg;
			}

			MeshUtil.BuildQuadMesh(vMeshBuild, sizeX, sizeY, originX, originY);

			UpdateAutoUv(shape, sizeX, sizeY);
			UpdateMeshUvAndColors();
			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

	}

}
