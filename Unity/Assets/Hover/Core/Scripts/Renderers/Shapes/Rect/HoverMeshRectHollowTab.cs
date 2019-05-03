using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverMeshRectHollowTab : HoverMeshRect {

		public const string TabOutwardName = "TabOutward";
		public const string TabThicknessName = "TabThickness";
		public const string ShowTabNName = "ShowTabN";
		public const string ShowTabEName = "ShowTabE";
		public const string ShowTabSName = "ShowTabS";
		public const string ShowTabWName = "ShowTabW";

		[SerializeField]
		[FormerlySerializedAs("InnerSizeType")]
		private SizeType _InnerSizeType = SizeType.Min;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("TabOutward")]
		private float _TabOutward = 0.01f;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("TabThickness")]
		private float _TabThickness = 0.025f;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ShowTabN")]
		private bool _ShowTabN = true;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ShowTabE")]
		private bool _ShowTabE = false;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ShowTabS")]
		private bool _ShowTabS = false;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ShowTabW")]
		private bool _ShowTabW = false;

		private SizeType vPrevInnerType;
		private float vPrevTabOut;
		private float vPrevTabThick;
		private bool vPrevTabN;
		private bool vPrevTabE;
		private bool vPrevTabS;
		private bool vPrevTabW;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SizeType InnerSizeType {
			get => _InnerSizeType;
			set => this.UpdateValueWithTreeMessage(ref _InnerSizeType, value, "InnerSizeType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TabOutward {
			get => _TabOutward;
			set => this.UpdateValueWithTreeMessage(ref _TabOutward, value, "TabOutward");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TabThickness {
			get => _TabThickness;
			set => this.UpdateValueWithTreeMessage(ref _TabThickness, value, "TabThickness");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool ShowTabN {
			get => _ShowTabN;
			set => this.UpdateValueWithTreeMessage(ref _ShowTabN, value, "ShowTabN");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool ShowTabE {
			get => _ShowTabE;
			set => this.UpdateValueWithTreeMessage(ref _ShowTabE, value, "ShowTabE");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool ShowTabS {
			get => _ShowTabS;
			set => this.UpdateValueWithTreeMessage(ref _ShowTabS, value, "ShowTabS");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool ShowTabW {
			get => _ShowTabW;
			set => this.UpdateValueWithTreeMessage(ref _ShowTabW, value, "ShowTabW");
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
				InnerSizeType != vPrevInnerType ||
				TabOutward != vPrevTabOut ||
				TabThickness != vPrevTabThick ||
				vPrevTabN != ShowTabN ||
				vPrevTabE != ShowTabE ||
				vPrevTabS != ShowTabS ||
				vPrevTabW != ShowTabW
			);

			vPrevInnerType = InnerSizeType;
			vPrevTabOut = TabOutward;
			vPrevTabThick = TabThickness;
			vPrevTabN = ShowTabN;
			vPrevTabE = ShowTabE;
			vPrevTabS = ShowTabS;
			vPrevTabW = ShowTabW;

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

			MeshUtil.BuildHollowRectangleTabMesh(vMeshBuild, outerW, outerH, innerW, innerH,
				TabOutward*outerProg, TabThickness*outerProg, innerProg/outerProg,
				ShowTabN, ShowTabE, ShowTabS, ShowTabW);

			UpdateAutoUv(shape, outerW, outerH);
			UpdateMeshUvAndColors();
			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

	}

}
