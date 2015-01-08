using System.Collections.Generic;
using HandMenu.State;
using UnityEngine;

namespace HandMenu.Display {

	/*================================================================================================*/
	public class MenuHandDisplay : MonoBehaviour {

		private MenuHandState vMenuHand;
		private IList<MenuPointDisplay> vPointDisplays;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(MenuHandState pMenuHand, Renderers pRenderers) {
			vMenuHand = pMenuHand;

			vPointDisplays = new List<MenuPointDisplay>();

			foreach ( PointData.PointZone zone in MenuHandState.PointZones ) {
				var pointObj = new GameObject("Point-"+zone);
				pointObj.transform.parent = gameObject.transform;

				MenuPointDisplay pointDisp = pointObj.AddComponent<MenuPointDisplay>();
				pointDisp.Build(vMenuHand, vMenuHand.GetPointState(zone), pRenderers);
				vPointDisplays.Add(pointDisp);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			foreach ( MenuPointDisplay pointDisp in vPointDisplays ) {
				pointDisp.gameObject.SetActive(vMenuHand.IsActive && pointDisp.IsActive());
			}
		}

	}

}
