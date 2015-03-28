using System;
using System.Collections.Generic;
using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiLevel : MonoBehaviour {

		public const float DegreeFull = 90;
		public const float ToDegrees = 180/(float)Math.PI;
		public const float AngleFull = DegreeFull/ToDegrees;

		private MenuState vMenuState;
		private IList<GameObject> vItemObjList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(MenuState pMenuState, IItemVisualSettingsProvider pVisualSettingsProv) {
			vMenuState = pMenuState;
			vItemObjList = new List<GameObject>();

			BaseItemState[] itemStates = vMenuState.GetItems();
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
				IItemVisualSettings visualSett = pVisualSettingsProv.GetSettings(itemState.Item);

				var itemObj = new GameObject("Item"+vItemObjList.Count);
				itemObj.transform.SetParent(gameObject.transform, false);
				vItemObjList.Add(itemObj);

				UiItem uiItem = itemObj.AddComponent<UiItem>();
				uiItem.Build(vMenuState, itemState, itemAngle, visualSett);

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
