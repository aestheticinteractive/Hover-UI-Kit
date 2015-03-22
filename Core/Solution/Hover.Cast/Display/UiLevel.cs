using System;
using System.Collections.Generic;
using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;
using Hover.Common.State;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiLevel : MonoBehaviour {

		public const float DegreeFull = 90;
		public const float ToDegrees = 180/(float)Math.PI;
		public const float AngleFull = DegreeFull/ToDegrees;
		public static readonly Vector3 PushFromHand = new Vector3(0, -0.2f, 0);

		private ArcState vArcState;
		private IList<GameObject> vItemObjList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, ICustomItem pCustom) {
			vArcState = pArcState;
			vItemObjList = new List<GameObject>();

			gameObject.transform.localPosition = PushFromHand;

			BaseItemState[] itemStates = vArcState.GetItems();
			int itemCount = itemStates.Length;
			float degree = 170 + DegreeFull/2f;
			float sizeSum = 0;

			for ( int i = 0 ; i < itemCount ; i++ ) {
				sizeSum += itemStates[i].Item.Height;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				BaseItemState itemState = itemStates[i];
				float itemPerc = itemState.Item.Height/sizeSum;
				float itemAngle = AngleFull*itemPerc;
				float itemDegHalf = itemAngle*ToDegrees/2f;

				var itemObj = new GameObject("Segment"+vItemObjList.Count);
				itemObj.transform.SetParent(gameObject.transform, false);
				vItemObjList.Add(itemObj);

				UiItem uiItem = itemObj.AddComponent<UiItem>();
				uiItem.Build(vArcState, itemState, itemAngle, pCustom);

				degree -= itemDegHalf;
				itemObj.transform.localRotation = Quaternion.AngleAxis(degree, Vector3.up);
				degree -= itemDegHalf;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			foreach ( GameObject segObj in vItemObjList ) {
				segObj.GetComponent<UiItem>()
					.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
			}
		}

	}

}
