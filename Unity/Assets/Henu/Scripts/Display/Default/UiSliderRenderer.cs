using System;
using Henu.Navigation;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiSliderRenderer : MonoBehaviour, IUiArcSegmentRenderer {

		protected ArcState vArcState;
		protected ArcSegmentState vSegState;
		protected float vAngle0;
		protected float vAngle1;
		protected ArcSegmentSettings vSettings;
		protected NavItemSlider vNavSlider;
		protected int vMeshSteps;

		protected float vSliderAngleHalf;
		protected float vSlideDegree0;
		protected float vSlideDegrees;
		protected Vector3 vSlideDir0;
		protected Vector3 vCursorWorldPos;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected GameObject vBackground;
		protected GameObject vFill;
		protected GameObject vGrabHold;
		protected UiSliderGrabRenderer vGrab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, ArcSegmentState pSegState,																									float pAngle0, float pAngle1, ArcSegmentSettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;
			vAngle0 = pAngle0+0.001f;
			vAngle1 = pAngle1-0.001f;
			vSettings = pSettings;
			vNavSlider = (NavItemSlider)vSegState.NavItem;
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));

			const float pi = (float)Math.PI;

			vSliderAngleHalf = pi/80f;
			vSlideDegree0 = (vAngle0+vSliderAngleHalf)/pi*180;
			vSlideDegrees = (vAngle1-vAngle0-vSliderAngleHalf*2)/pi*180;

			////

			vBackground = new GameObject("Background");
			vBackground.transform.SetParent(gameObject.transform, false);
			vBackground.AddComponent<MeshFilter>();
			vBackground.AddComponent<MeshRenderer>();
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			Mesh mesh = vBackground.GetComponent<MeshFilter>().mesh;
			BuildMesh(mesh, 1);
			vSlideDir0 = mesh.vertices[0].normalized;

			vFill = new GameObject("Fill");
			vFill.transform.SetParent(gameObject.transform, false);
			vFill.AddComponent<MeshFilter>();
			vFill.AddComponent<MeshRenderer>();
			vFill.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vFill.renderer.sharedMaterial.renderQueue -= 200;
			vFill.renderer.sharedMaterial.color = Color.clear;

			////

			vGrabHold = new GameObject("GrabHold");
			vGrabHold.transform.SetParent(gameObject.transform, false);

			var grabObj = new GameObject("Grab");
			grabObj.transform.SetParent(vGrabHold.transform, false);

			vGrab = grabObj.AddComponent<UiSliderGrabRenderer>();
			vGrab.Build(vArcState, vSegState, -vSliderAngleHalf, vSliderAngleHalf, pSettings);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			UpdateCurrentValue();

			vMainAlpha = GetArcAlpha(vArcState)*vAnimAlpha;

			Color colBg = vSettings.BackgroundColor;
			Color colFill = vSettings.HighlightColor;

			colBg.a *= vMainAlpha;
			colFill.a *= vMainAlpha;

			BuildMesh(vFill.GetComponent<MeshFilter>().mesh, vNavSlider.CurrentValue);

			vBackground.renderer.sharedMaterial.color = colBg;
			vFill.renderer.sharedMaterial.color = colFill;

			float slideDeg = vSlideDegree0 + vSlideDegrees*vNavSlider.CurrentValue;
			vGrabHold.transform.localRotation = Quaternion.AngleAxis(slideDeg, Vector3.up);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
			vGrab.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float CalculateCursorDistance(Vector3 pCursorWorldPosition) {
			vCursorWorldPos = pCursorWorldPosition;
			return vGrab.CalculateCursorDistance(pCursorWorldPosition);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateCurrentValue() {
			if ( !vSegState.NavItem.Selected ) {
				return;
			}

			Vector3 cursorRel = gameObject.transform.InverseTransformPoint(vCursorWorldPos);
			cursorRel.y = 0;
			Vector3 cursorDir = cursorRel.normalized;
			Quaternion diff = Quaternion.FromToRotation(vSlideDir0, cursorDir);

			float cursorDeg;
			Vector3 cursorAxis;
			diff.ToAngleAxis(out cursorDeg, out cursorAxis);

			vNavSlider.CurrentValue = cursorDeg/vSlideDegrees;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh, float pFillAmount) {
			float fillAngle1 = vAngle0 + (vAngle1-vAngle0)*pFillAmount;
			int fillSteps = (int)Math.Round((vMeshSteps-2)*pFillAmount)+2;
			MeshUtil.BuildRingMesh(pMesh, 1, 1+0.4f, vAngle0, fillAngle1, fillSteps);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static float GetArcAlpha(ArcState pArcState) {
			float alpha = 1-(float)Math.Pow(1-pArcState.Strength, 2);
			alpha -= (float)Math.Pow(pArcState.GrabStrength, 2);
			return Math.Max(0, alpha);
		}

	}

}
