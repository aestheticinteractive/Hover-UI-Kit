using System;
using System.Collections.Generic;
using Hovercast.Core.Settings;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display.Default {

	/*================================================================================================*/
	public class UiSelectRenderer : MonoBehaviour, IUiArcSegmentRenderer {

		public const float ArcCanvasThickness = 250;
		public const float ArcCanvasScale = 0.002f;
		public const float AngleInset = 0.0012f;

		protected ArcState vArcState;
		protected ArcSegmentState vSegState;
		protected float vAngle0;
		protected float vAngle1;
		protected ArcSegmentSettings vSettings;
		protected int vMeshSteps;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected GameObject vBackground;
		protected GameObject vEdge;
		protected GameObject vHighlight;
		protected GameObject vSelect;
		protected UiLabel vLabel;

		private List<Vector3> vSelectionPoints;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, ArcSegmentState pSegState,
														float pArcAngle, ArcSegmentSettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;
			vAngle0 = -pArcAngle/2f+AngleInset;
			vAngle1 = pArcAngle/2f-AngleInset;
			vSettings = pSettings;
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));

			////

			vBackground = new GameObject("Background");
			vBackground.transform.SetParent(gameObject.transform, false);
			vBackground.AddComponent<MeshFilter>();
			vBackground.AddComponent<MeshRenderer>();
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			vEdge = new GameObject("Edge");
			vEdge.transform.SetParent(gameObject.transform, false);
			vEdge.AddComponent<MeshFilter>();
			vEdge.AddComponent<MeshRenderer>();
			vEdge.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vEdge.renderer.sharedMaterial.renderQueue -= 300;
			vEdge.renderer.sharedMaterial.color = Color.clear;

			vHighlight = new GameObject("Highlight");
			vHighlight.transform.SetParent(gameObject.transform, false);
			vHighlight.AddComponent<MeshFilter>();
			vHighlight.AddComponent<MeshRenderer>();
			vHighlight.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vHighlight.renderer.sharedMaterial.renderQueue -= 200;
			vHighlight.renderer.sharedMaterial.color = Color.clear;

			vSelect = new GameObject("Select");
			vSelect.transform.SetParent(gameObject.transform, false);
			vSelect.AddComponent<MeshFilter>();
			vSelect.AddComponent<MeshRenderer>();
			vSelect.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelect.renderer.sharedMaterial.renderQueue -= 10;
			vSelect.renderer.sharedMaterial.color = Color.clear;

			////

			Mesh bgMesh = vBackground.GetComponent<MeshFilter>().mesh;
			BuildMesh(bgMesh, 1);

			vSelectionPoints = new List<Vector3>();

			for ( int i = 1 ; i < bgMesh.vertices.Length ; i += 2 ) {
				vSelectionPoints.Add(bgMesh.vertices[i]);
			}

			MeshUtil.BuildRingMesh(vEdge.GetComponent<MeshFilter>().mesh,
				0.99f, 1.0f, vAngle0, vAngle1, vMeshSteps);

			////

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			labelObj.transform.localPosition = new Vector3(0, 0, 1);
			labelObj.transform.localScale = new Vector3(1, 1, (vArcState.IsLeft ? 1 : -1));
			
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.IsLeft = vArcState.IsLeft;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = GetArcAlpha(vArcState)*vAnimAlpha;

			if ( !vSegState.NavItem.IsEnabled ) {
				vMainAlpha *= 0.333f;
			}

			float high = vSegState.HighlightProgress;
			float edge = (vSegState.IsNearestHighlight && !vSegState.IsSelectionPrevented && 
				vSegState.NavItem.AllowSelection ? high : 0);
			float select = 1-(float)Math.Pow(1-vSegState.SelectionProgress, 1.5f);
			float selectAlpha = select;

			if ( vSegState.NavItem.IsStickySelected ) {
				selectAlpha = 1;
			}

			Color colBg = vSettings.BackgroundColor;
			Color colEdge = vSettings.EdgeColor;
			Color colHigh = vSettings.HighlightColor;
			Color colSel = vSettings.SelectionColor;

			colBg.a *= vMainAlpha;
			colEdge.a *= edge*vMainAlpha;
			colHigh.a *= high*vMainAlpha;
			colSel.a *= selectAlpha*vMainAlpha;

			BuildMesh(vHighlight.GetComponent<MeshFilter>().mesh, high);
			BuildMesh(vSelect.GetComponent<MeshFilter>().mesh, select);

			vBackground.renderer.sharedMaterial.color = colBg;
			vEdge.renderer.sharedMaterial.color = colEdge;
			vHighlight.renderer.sharedMaterial.color = colHigh;
			vSelect.renderer.sharedMaterial.color = colSel;

			vHighlight.SetActive(high > 0);
			vSelect.SetActive(select > 0);

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vSegState.NavItem.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float CalculateCursorDistance(Vector3 pCursorWorldPosition) {
			float sqrMagMin = float.MaxValue;
			Vector3 relCursor = gameObject.transform.InverseTransformPoint(pCursorWorldPosition);

			foreach ( Vector3 v in vSelectionPoints ) {
				float sqrMag = (v-relCursor).sqrMagnitude;
				sqrMagMin = Math.Min(sqrMagMin, sqrMag);
			}

			return (float)Math.Sqrt(sqrMagMin);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh, float pThickness) {
			MeshUtil.BuildRingMesh(pMesh, 1, 1+0.5f*pThickness, vAngle0, vAngle1, vMeshSteps);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static float GetArcAlpha(ArcState pArcState) {
			float alpha = 1-(float)Math.Pow(1-pArcState.DisplayStrength, 2);
			alpha -= (float)Math.Pow(pArcState.NavBackStrength, 2);
			return Math.Max(0, alpha);
		}

	}

}
