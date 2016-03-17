using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public abstract class UiItemBaseIconRenderer : UiItemSelectRenderer {

		protected GameObject vIcon;
		protected MeshBuilder vIconMeshBuilder;

		private int vPrevTextSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Materials.IconOffset GetIconOffset();

		/*--------------------------------------------------------------------------------------------*/
		protected virtual Vector3 GetIconScale() {
			float s = vSettings.TextSize*0.75f*ArcCanvasScale;
			return new Vector3(s, s, 1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(IHovercastMenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			base.Build(pMenuState, pItemState, pArcAngle, pSettings);

			vIcon = new GameObject("Icon");
			vIcon.transform.SetParent(gameObject.transform, false);
			vIcon.transform.localRotation = 
				vLabel.gameObject.transform.localRotation*vLabel.CanvasLocalRotation;
			vIcon.transform.localScale = GetIconScale();
			vIcon.AddComponent<MeshRenderer>();

			MeshFilter iconFilt = vIcon.AddComponent<MeshFilter>();
			vIconMeshBuilder = new MeshBuilder();
			MeshUtil.BuildQuadMesh(vIconMeshBuilder);
			Materials.SetMeshIconCoords(vIconMeshBuilder, GetIconOffset());
			vIconMeshBuilder.Commit();
			vIconMeshBuilder.CommitColors(Color.clear);
			iconFilt.sharedMesh = vIconMeshBuilder.Mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void SetDepthHint(int pDepthHint) {
			base.SetDepthHint(pDepthHint);

			vIcon.GetComponent<MeshRenderer>().sharedMaterial = 
				Materials.GetLayer(Materials.Layer.Icon, pDepthHint, "StandardIcons");
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ArrowIconColor;
			color.a *= (vItemState.MaxHighlightProgress*0.75f + 0.25f)*vMainAlpha;

			vIconMeshBuilder.CommitColors(color);

			if ( vSettings.TextSize != vPrevTextSize ) {
				vPrevTextSize = vSettings.TextSize;

				float inset = vLabel.TextH;

				vLabel.SetInset(!vMenuState.IsOnLeftSide, inset);

				vIcon.transform.localPosition = 
					new Vector3(0, 0, 1+(vLabel.CanvasW-inset*0.666f)*ArcCanvasScale);
			}
		}

	}

}
