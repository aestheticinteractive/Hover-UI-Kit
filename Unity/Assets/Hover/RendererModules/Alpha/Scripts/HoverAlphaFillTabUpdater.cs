using Hover.Core.Renderers;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverShapeRect))]
	[RequireComponent(typeof(HoverFillButton))]
	[RequireComponent(typeof(HoverFillButtonRectUpdater))]
	public class HoverAlphaFillTabUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public HoverCanvasDataUpdater CanvasUpdater;

		public float TabOutward = 0.01f;
		public float TabThickness = 0.025f;
		public bool UseItemSelectionState = true;

		public bool ShowTabN = true;
		public bool ShowTabE = false;
		public bool ShowTabS = false;
		public bool ShowTabW = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
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
