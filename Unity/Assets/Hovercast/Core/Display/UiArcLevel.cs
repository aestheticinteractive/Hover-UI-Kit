using System;
using System.Collections.Generic;
using Hovercast.Core.Settings;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display {

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
			float degree = 170 + DegreeFull/2f;
			float sizeSum = 0;

			for ( int i = 0 ; i < segCount ; i++ ) {
				sizeSum += segStates[i].NavItem.RelativeSize;
			}

			for ( int i = 0 ; i < segCount ; i++ ) {
				ArcSegmentState segState = segStates[i];
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

	}

}
