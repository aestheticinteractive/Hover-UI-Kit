using System;
using Henu.Navigation;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcSegment : MonoBehaviour {

		private ArcState vArcState;
		private ArcSegmentState vSegState;
		private GameObject vRendererObj;
		private IUiArcSegmentRenderer vRenderer;
		private Transform vCursorBaseTx;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, ArcSegmentState pSegState, float pAngle0, float pAngle1, 
																				Renderers pRenderers) {
			vArcState = pArcState;
			vSegState = pSegState;

			BuildRenderer(pRenderers, pAngle0, pAngle1);

			vCursorBaseTx = GameObject.Find("HandController").transform;
			vSegState.SetCursorDistanceFunction(CalcCursorDistance);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildRenderer(Renderers pRenderers, float pAngle0, float pAngle1) {
			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);

			Type rendererType;

			switch ( vSegState.NavItem.Type ) {
				case NavItem.ItemType.Parent:
					rendererType = pRenderers.ArcSegmentParent;
					break;

				case NavItem.ItemType.Checkbox:
					rendererType = pRenderers.ArcSegmentCheckbox;
					break;

				case NavItem.ItemType.Radio:
					rendererType = pRenderers.ArcSegmentRadio;
					break;

				default:
					rendererType = pRenderers.ArcSegmentSelection;
					break;
			}

			vRenderer = (IUiArcSegmentRenderer)vRendererObj.AddComponent(rendererType);
			vRenderer.Build(vArcState, vSegState, pAngle0, pAngle1);
		}

		/*--------------------------------------------------------------------------------------------*/
		private float CalcCursorDistance(Vector3 pCursorPos) {
			Vector3 worldCursor = vCursorBaseTx.TransformPoint(pCursorPos);
			Vector3 relCursor = gameObject.transform.InverseTransformPoint(worldCursor);
			float dist = vRenderer.CalculateCursorDistance(relCursor);
			return gameObject.transform.TransformVector(Vector3.up*dist).magnitude;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			vSegState.SetIsAnimating(pProgress < 1);
			vRenderer.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

	}

}
