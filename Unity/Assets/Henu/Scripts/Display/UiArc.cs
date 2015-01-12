using System;
using System.Collections.Generic;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArc : MonoBehaviour {

		private ArcState vArcState;
		private Renderers vRenderers;
		private IList<GameObject> vSegmentObjList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ArcState pArc, Renderers pRenderers) {
			vArcState = pArc;
			vRenderers = pRenderers;
			vSegmentObjList = new List<GameObject>();

			vArcState.OnLevelChange += HandleLevelChange;
			HandleLevelChange(0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.transform.localPosition = vArcState.Center;
			gameObject.transform.localRotation = vArcState.Rotation;
			gameObject.transform.localScale = Vector3.one*(vArcState.Size*1.1f);

			foreach ( GameObject segObj in vSegmentObjList ) {
				segObj.SetActive(vArcState.IsActive);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			const float angleFull = (float)Math.PI/2f;

			ArcSegmentState[] segStates = vArcState.GetSegments();
			int segCount = segStates.Length;
			float angleInc = angleFull/segCount;
			float angle = (float)Math.PI-angleFull*0.6f;

			while ( vSegmentObjList.Count > segCount ) {
				GameObject segObj = vSegmentObjList[0];
				vSegmentObjList.RemoveAt(0);
				Destroy(segObj);
			}

			while ( vSegmentObjList.Count < segCount ) {
				var segObj = new GameObject("Segment"+vSegmentObjList.Count);
				segObj.transform.SetParent(gameObject.transform, false);
				segObj.AddComponent<UiArcSegment>();
				vSegmentObjList.Add(segObj);
			}

			for ( int i = 0 ; i < segCount ; i++ ) {
				ArcSegmentState segState = segStates[i];
				GameObject segObj = vSegmentObjList[i];

				UiArcSegment uiSeg = segObj.GetComponent<UiArcSegment>();
				uiSeg.Build(vArcState, segState, angle, angle+angleInc, vRenderers);

				angle += angleInc;
			}

			Update();
		}
		
	}

}
