using System;
using Hover.Board.Custom;
using Hover.Board.Custom.Standard;
using Hover.Board.State;
using Hover.Common.Display;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiProjectionRenderer : MonoBehaviour, IUiProjectionRenderer {

		protected ProjectionState vProjectionState;
		protected ProjectionVisualSettingsStandard vSettings;
		protected GameObject vSpotObj;
		protected GameObject vLineObj;
		protected MeshBuilder vSpotMeshBuilder;
		protected MeshBuilder vLineMeshBuilder;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ProjectionState pProjectionState,
																IProjectionVisualSettings pSettings) {
			vProjectionState = pProjectionState;
			vSettings = (ProjectionVisualSettingsStandard)pSettings;

			////

			vSpotObj = new GameObject("Spot");
			vSpotObj.transform.SetParent(gameObject.transform, false);
			vSpotObj.transform.localScale = Vector3.zero;

			MeshFilter spotFilt = vSpotObj.AddComponent<MeshFilter>();
			vSpotMeshBuilder = new MeshBuilder(spotFilt.mesh);
			MeshUtil.BuildCircleMesh(vSpotMeshBuilder, 0.5f, 32);
			vSpotMeshBuilder.Commit();

			MeshRenderer spotRend = vSpotObj.AddComponent<MeshRenderer>();
			spotRend.sharedMaterial = 
				Materials.GetLayer(Materials.Layer.AboveText, Materials.DepthHintMax);

			////

			vLineObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			vLineObj.name = "Line";
			vLineObj.transform.SetParent(gameObject.transform, false);
			vLineObj.transform.localScale = Vector3.zero;

			vLineMeshBuilder = new MeshBuilder(vLineObj.GetComponent<MeshFilter>().mesh);

			MeshRenderer lineRend = vLineObj.GetComponent<MeshRenderer>();
			lineRend.sharedMaterial = 
				Materials.GetLayer(Materials.Layer.AboveText, Materials.DepthHintMax);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
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

			vSpotMeshBuilder.CommitColors(colSpot);
			vSpotObj.transform.localScale = spotScale;

			vLineMeshBuilder.CommitColors(colLine);
			vLineObj.transform.localScale = new Vector3(lineThick, dist, lineThick);
			vLineObj.transform.localPosition = new Vector3(0, vLineObj.transform.localScale.y/2f, 0);
		}

	}

}
