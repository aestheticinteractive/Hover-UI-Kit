using System;
using System.Collections.Generic;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcLevel : MonoBehaviour {

		private ArcState vArcState;
		private IList<GameObject> vSegmentObjList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, Renderers pRenderers) {
			vArcState = pArcState;
			vSegmentObjList = new List<GameObject>();

			ArcSegmentState[] segStates = vArcState.GetSegments();
			int segCount = segStates.Length;

			const float angleFull = (float)Math.PI/2f;
			float angleInc = angleFull/segCount;
			float angle = (float)Math.PI-angleFull*0.6f;

			for ( int i = 0 ; i < segCount ; i++ ) {
				ArcSegmentState segState = segStates[i];

				var segObj = new GameObject("Segment"+vSegmentObjList.Count);
				segObj.transform.SetParent(gameObject.transform, false);
				vSegmentObjList.Add(segObj);

				UiArcSegment uiSeg = segObj.AddComponent<UiArcSegment>();
				uiSeg.Build(vArcState, segState, angle, angle+angleInc, pRenderers);

				angle += angleInc;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			foreach ( GameObject segObj in vSegmentObjList ) {
				segObj.SetActive(vArcState.IsActive);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			foreach ( GameObject segObj in vSegmentObjList ) {
				segObj.GetComponent<UiArcSegment>()
					.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
			}
		}
	}

}
