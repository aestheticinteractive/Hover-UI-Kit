using System;
using HandMenu.State;
using UnityEngine;

namespace HandMenu.Display.Default {

	/*================================================================================================*/
	public class UiPointParentRenderer : UiPointRenderer {

		public static float ArrowSize = 16;
		public static Texture2D ArrowTexture = Resources.Load<Texture2D>("Arrow");

		private GameObject vArrow;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(MenuHandState pHand, MenuPointState pPoint) {
			base.Build(pHand, pPoint);

			vArrow = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vArrow.transform.parent = vHold.transform;
			vArrow.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vArrow.renderer.sharedMaterial.color = Color.clear;
			vArrow.renderer.sharedMaterial.mainTexture = ArrowTexture;

			////
			
			int mult = (vHand.IsLeft ? -1 : 1);

			vArrow.transform.localPosition = new Vector3(0, 0, (Width-Height/2f)*Scale*mult);
			vArrow.transform.localRotation = vBackground.transform.localRotation;
			vArrow.transform.localScale = new Vector3(-ArrowSize*Scale*mult, ArrowSize*Scale, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			if ( !vPoint.IsActive ) {
				return;
			}

			float alpha = vPoint.HighlightProgress*0.8f + 0.2f;
			vArrow.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha*vOverallAlpha);
		}

	}

}
