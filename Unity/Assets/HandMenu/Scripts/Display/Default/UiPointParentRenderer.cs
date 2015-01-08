using System;
using HandMenu.State;
using HandMenu.Util;
using UnityEngine;

namespace HandMenu.Display.Default {

	/*================================================================================================*/
	public class UiPointParentRenderer : UiPointRenderer {

		private GameObject vArrow;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(MenuHandState pHand, MenuPointState pPoint) {
			base.Build(pHand, pPoint);

			vArrow = new GameObject("Arrow");
			vArrow.transform.parent = vHold.transform;
			vArrow.AddComponent<MeshRenderer>();
			vArrow.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vArrow.renderer.sharedMaterial.color = Color.clear;

			MeshFilter meshFilter = vArrow.AddComponent<MeshFilter>();
			MeshUtil.BuildArrow(meshFilter.mesh);

			////
			
			int mult = (vHand.IsLeft ? -1 : 1);

			vArrow.transform.localPosition = new Vector3(0, 0, (Width-Height/2f)*Scale*mult);
			vArrow.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.left);
			vArrow.transform.localScale = new Vector3(8*Scale*mult, -1, 8*Scale);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			if ( !vPoint.IsActive ) {
				return;
			}

			float high = (float)Math.Pow(vPoint.HighlightProgress, 4);
			float alpha = high*0.8f + 0.2f;

			vArrow.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha*vOverallAlpha);
		}

	}

}
