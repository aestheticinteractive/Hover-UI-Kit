using Hover.Core.Renderers;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverShapeRect))]
	[RequireComponent(typeof(HoverFillButton))]
	[RequireComponent(typeof(HoverFillButtonRectUpdater))]
	public class HoverAlphaFillTabUpdater : TreeUpdateableBehavior, ISettingsController {

		[SerializeField]
		[FormerlySerializedAs("CanvasUpdater")]
		private HoverCanvasDataUpdater _CanvasUpdater;

		[SerializeField]
		[FormerlySerializedAs("TabOutward")]
		private float _TabOutward = 0.01f;

		[SerializeField]
		[FormerlySerializedAs("TabThickness")]
		private float _TabThickness = 0.025f;
		
		[SerializeField]
		[FormerlySerializedAs("UseItemSelectionState")]
		private bool _UseItemSelectionState = true;

		[SerializeField]
		[FormerlySerializedAs("ShowTabN")]
		private bool _ShowTabN = true;

		[SerializeField]
		[FormerlySerializedAs("ShowTabE")]
		private bool _ShowTabE = false;

		[SerializeField]
		[FormerlySerializedAs("ShowTabS")]
		private bool _ShowTabS = false;

		[SerializeField]
		[FormerlySerializedAs("ShowTabW")]
		private bool _ShowTabW = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCanvasDataUpdater CanvasUpdater {
			get => _CanvasUpdater;
			set => this.UpdateValueWithTreeMessage(ref _CanvasUpdater, value, "CanvasUpdater");
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
		public bool UseItemSelectionState {
			get => _UseItemSelectionState;
			set => this.UpdateValueWithTreeMessage(ref _UseItemSelectionState, value, "UseItemSel");
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
		public override void TreeUpdate() {
			bool isSelected = (
				!UseItemSelectionState ||
				CanvasUpdater.IconType == HoverCanvasDataUpdater.IconPairType.RadioOn ||
				CanvasUpdater.IconType == HoverCanvasDataUpdater.IconPairType.CheckboxOn
			);

			HoverMesh.DisplayModeType dispMode = (isSelected ?
				HoverMesh.DisplayModeType.SliderFill : HoverMesh.DisplayModeType.Standard);

			////

			HoverShapeRect shapeRect = GetComponent<HoverShapeRect>();
			float minOutward = -Mathf.Min(shapeRect.SizeX, shapeRect.SizeY)/2;

			TabOutward = Mathf.Max(TabOutward, minOutward);
			TabThickness = Mathf.Max(TabThickness, 0);

			////

			HoverFillButton hoverFill = GetComponent<HoverFillButton>();
			int meshCount = hoverFill.GetChildMeshCount();

			for ( int i = 0 ; i < meshCount ; i++ ) {
				UpdateChildMesh((HoverMeshRectHollowTab)hoverFill.GetChildMesh(i), dispMode);
			}

			if ( isSelected ) {
				hoverFill.Controllers.Set(HoverFillButton.ShowEdgeName, this);
				hoverFill.ShowEdge = true;
				RendererUtil.SetActiveWithUpdate(hoverFill.Edge, true);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateChildMesh(HoverMeshRectHollowTab pChildMesh,
																HoverMesh.DisplayModeType pDispMode) {
			float highProg = pChildMesh.GetComponent<HoverIndicator>().HighlightProgress;

			if ( pDispMode == HoverMesh.DisplayModeType.SliderFill ) {
				highProg = 1;
			}

			pChildMesh.Controllers.Set(HoverMesh.DisplayModeName, this);
			pChildMesh.Controllers.Set(HoverMeshRectHollowTab.TabOutwardName, this);
			pChildMesh.Controllers.Set(HoverMeshRectHollowTab.TabThicknessName, this);
			pChildMesh.Controllers.Set(HoverMeshRectHollowTab.ShowTabNName, this);
			pChildMesh.Controllers.Set(HoverMeshRectHollowTab.ShowTabEName, this);
			pChildMesh.Controllers.Set(HoverMeshRectHollowTab.ShowTabSName, this);
			pChildMesh.Controllers.Set(HoverMeshRectHollowTab.ShowTabWName, this);

			pChildMesh.DisplayMode = pDispMode;
			pChildMesh.TabOutward = TabOutward*highProg;
			pChildMesh.TabThickness = TabThickness;
			pChildMesh.ShowTabN = ShowTabN;
			pChildMesh.ShowTabE = ShowTabE;
			pChildMesh.ShowTabS = ShowTabS;
			pChildMesh.ShowTabW = ShowTabW;
		}

	}

}
