using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public abstract class UiItemBaseIconRenderer : UiItemSelectRenderer {

		private GameObject vIcon;

		private int vPrevTextSize;


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
		public override void Build(IBaseItemState pItemState, IItemVisualSettings pSettings) {
			base.Build(pItemState, pSettings);

			vLabel.AlignLeft = true;
			vLabel.transform.localPosition = new Vector3(-vItemState.Item.Width/2, 0, 0);

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
		public override void Update() {
			base.Update();

			Color color = vSettings.ArrowIconColor;
			color.a *= (vItemState.MaxHighlightProgress*0.75f + 0.25f)*vMainAlpha;

			vIcon.renderer.sharedMaterial.color = color;

			if ( vSettings.TextSize != vPrevTextSize ) {
				vPrevTextSize = vSettings.TextSize;

				float inset = vSettings.TextSize;
				vLabel.SetInset(false, inset);

				vIcon.transform.localPosition = new Vector3(
					vItemState.Item.Width/2-inset*0.666f*LabelCanvasScale, 0, 0);
				vIcon.transform.localScale = GetIconScale();
			}
		}

	}

}
