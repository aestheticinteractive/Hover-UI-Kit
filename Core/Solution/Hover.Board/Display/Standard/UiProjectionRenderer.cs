using System;
using Hover.Board.Custom;
using Hover.Board.Custom.Standard;
using Hover.Board.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiProjectionRenderer : MonoBehaviour, IUiProjectionRenderer {

		private ProjectionState vProjectionState;
		private ProjectionVisualSettingsStandard vSettings;
		private GameObject vSpotObj;
		private GameObject vLineObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ProjectionState pProjectionState, IProjectionVisualSettings pSettings) {
			vProjectionState = pProjectionState;
			vSettings = (ProjectionVisualSettingsStandard)pSettings;

			////

			vSpotObj = new GameObject("Spot");
			vSpotObj.transform.SetParent(gameObject.transform, false);
			vSpotObj.transform.localScale = Vector3.zero;

			MeshFilter spotMeshFilt = vSpotObj.AddComponent<MeshFilter>();
			MeshUtil.BuildCircleMesh(spotMeshFilt.mesh, 0.5f, 32);

			MeshRenderer spotMeshRend = vSpotObj.AddComponent<MeshRenderer>();
			spotMeshRend.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			spotMeshRend.sharedMaterial.renderQueue += 100;

			////

			vLineObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			vLineObj.name = "Line";
			vLineObj.transform.SetParent(gameObject.transform, false);
			vLineObj.transform.localScale = Vector3.zero;
			vLineObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vLineObj.renderer.sharedMaterial.renderQueue += 200;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			float dist = vProjectionState.ProjectedPanelDistance;
			float prog = vProjectionState.ProjectedPanelProgress;
			float lineThick = 0.01f*vProjectionState.Cursor.Size;
			float spotSize = (1-prog)*60 + 5;
			float alphaMult = (float)Math.Pow(prog, 2);
			Color colSpot = vSettings.SpotlightColor;
			Color colLine = vSettings.LineColor;

			Vector3 spotScale = Vector3.one*lineThick*spotSize;
			spotScale.y *= (vProjectionState.ProjectedFromFront ? -1 : 1);

			colSpot.a *= alphaMult;
			colLine.a *= alphaMult;

			vSpotObj.renderer.sharedMaterial.color = colSpot;
			vSpotObj.transform.localScale = spotScale;

			vLineObj.renderer.sharedMaterial.color = colLine;
			vLineObj.transform.localScale = new Vector3(lineThick, dist, lineThick);
			vLineObj.transform.localPosition = new Vector3(0, vLineObj.transform.localScale.y/2f, 0);
		}

	}

}
