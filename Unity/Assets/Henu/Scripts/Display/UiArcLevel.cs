using System;
using System.Collections.Generic;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcLevel : MonoBehaviour {

		public const float AngleFull = (float)Math.PI/2f;
		public static readonly Vector3 PushFromHand = new Vector3(0, -0.2f, 0);

		private ArcState vArcState;
		private IList<GameObject> vSegmentObjList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, ISettings pSettings) {
			vArcState = pArcState;
			vSegmentObjList = new List<GameObject>();

			gameObject.transform.localPosition = PushFromHand;

			ArcSegmentState[] segStates = vArcState.GetSegments();
			int segCount = segStates.Length;
			bool isLeft = vArcState.IsLeft;

			const float pi = (float)Math.PI;
			float segAngleFull = AngleFull/segCount;
			float segAngleHalf = segAngleFull/2f;
			float degreeInc = segAngleFull/pi*180;
			float degrees = (isLeft ? 170 : 190)+degreeInc*(segCount-1)/2f;

			for ( int i = 0 ; i < segCount ; i++ ) {
				ArcSegmentState segState = segStates[isLeft ? i : segCount-i-1];

				var segObj = new GameObject("Segment"+vSegmentObjList.Count);
				segObj.transform.SetParent(gameObject.transform, false);
				segObj.transform.localRotation = Quaternion.AngleAxis(degrees, Vector3.up);
				vSegmentObjList.Add(segObj);

				UiArcSegment uiSeg = segObj.AddComponent<UiArcSegment>();
				uiSeg.Build(vArcState, segState, -segAngleHalf, segAngleHalf, pSettings);

				degrees -= degreeInc;
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
