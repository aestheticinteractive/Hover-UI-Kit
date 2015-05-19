using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public abstract class UiItemBaseIconRenderer : UiItemSelectRenderer {

		private GameObject vIcon;

		private int vPrevTextSize;
		private bool vIsSizeChanged;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Texture2D GetIconTexture();

		/*--------------------------------------------------------------------------------------------*/
		protected virtual Vector3 GetIconScale() {
			float s = vSettings.TextSize*0.75f*LabelCanvasScale;
			return new Vector3(s, s, 1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(IHoverboardPanelState pPanelState,
										IHoverboardLayoutState pLayoutState, IBaseItemState pItemState,
										IItemVisualSettings pSettings) {
			base.Build(pPanelState, pLayoutState, pItemState, pSettings);

			vLabel.AlignLeft = true;

			vIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vIcon.name = "Icon";
			vIcon.transform.SetParent(gameObject.transform, false);
			vIcon.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vIcon.renderer.sharedMaterial.color = Color.clear;
			vIcon.renderer.sharedMaterial.mainTexture = GetIconTexture();
			vIcon.transform.localRotation = 
				vLabel.gameObject.transform.localRotation*vLabel.CanvasLocalRotation;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void SetCustomSize(float pWidth, float pHeight, bool pCentered=true) {
			base.SetCustomSize(pWidth, pHeight, pCentered);
			vLabel.transform.localPosition = new Vector3(-vWidth/2, 0, 0);
			vIsSizeChanged = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ArrowIconColor;
			color.a *= (vItemState.MaxHighlightProgress*0.75f + 0.25f)*vMainAlpha;

			vIcon.renderer.sharedMaterial.color = color;

			if ( vSettings.TextSize != vPrevTextSize || vIsSizeChanged ) {
				vPrevTextSize = vSettings.TextSize;
				vIsSizeChanged = false;

				float inset = vSettings.TextSize;
				vLabel.SetInset(false, inset);

				vIcon.transform.localPosition = new Vector3(
					vWidth/UiItem.Size/2-inset*0.666f*LabelCanvasScale, 0, 0);
				vIcon.transform.localScale = GetIconScale();
			}
		}

	}

}
