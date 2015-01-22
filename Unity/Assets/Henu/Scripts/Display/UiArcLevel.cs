using System;
using System.Collections.Generic;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcLevel : MonoBehaviour {

		public const float DegreeFull = 90;
		public const float ToDegrees = 180/(float)Math.PI;
		public const float AngleFull = DegreeFull/ToDegrees;
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
			float degree = 180+10*(vArcState.IsLeft ? -1 : 1) + DegreeFull/2f;
			float sizeSum = 0;

			for ( int i = 0 ; i < segCount ; i++ ) {
				sizeSum += segStates[i].NavItem.RelativeSize;
			}

			for ( int i = 0 ; i < segCount ; i++ ) {
				ArcSegmentState segState = segStates[isLeft ? i : segCount-i-1];
				float segPerc = segState.NavItem.RelativeSize/sizeSum;
				float segAngle = AngleFull*segPerc;
				float segDegHalf = segAngle*ToDegrees/2f;

				var segObj = new GameObject("Segment"+vSegmentObjList.Count);
				segObj.transform.SetParent(gameObject.transform, false);
				vSegmentObjList.Add(segObj);

				UiArcSegment uiSeg = segObj.AddComponent<UiArcSegment>();
				uiSeg.Build(vArcState, segState, segAngle, pSettings);

				degree -= segDegHalf;
				segObj.transform.localRotation = Quaternion.AngleAxis(degree, Vector3.up);
				degree -= segDegHalf;
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

		/*--------------------------------------------------------------------------------------------* /
		private void HandleIsLeftChange() {
			int segCount = vSegmentObjList.Count;
			bool isLeft = vArcState.IsLeft;
			float degree = 180+10*(isLeft ? -1 : 1) + DegreeFull/2f;

			for ( int i = 0 ; i < segCount ; i++ ) {
				GameObject segObj = vSegmentObjList[isLeft ? i : segCount-i-1];
				UiArcSegment uiSeg = segObj.GetComponent<UiArcSegment>();
				float segDegHalf = uiSeg.ArcAngle*ToDegrees/2f;

				degree -= segDegHalf;
				segObj.transform.localRotation = Quaternion.AngleAxis(degree, Vector3.up);
				degree -= segDegHalf;
			}
		}*/

	}

}
