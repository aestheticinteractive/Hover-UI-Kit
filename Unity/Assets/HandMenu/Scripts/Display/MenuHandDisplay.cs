using System.Collections.Generic;
using HandMenu.State;
using UnityEngine;

namespace HandMenu.Display {

	/*================================================================================================*/
	public class MenuHandDisplay : MonoBehaviour {

		public MenuHandState MenuHand { get; set; }
		public SelectHandState SelectHand { get; set; }

		private IList<MenuPointDisplay> vPointDisplays;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vPointDisplays = new List<MenuPointDisplay>();

			foreach ( PointData.PointZone zone in MenuHandState.PointZones ) {
				var pointObj = new GameObject("Point-"+zone);
				pointObj.transform.parent = gameObject.transform;

				MenuPointDisplay pointDisp = pointObj.AddComponent<MenuPointDisplay>();
				pointDisp.Hand = MenuHand;
				pointDisp.Point = MenuHand.GetPointState(zone);
				vPointDisplays.Add(pointDisp);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			foreach ( MenuPointDisplay pointDisp in vPointDisplays ) {
				pointDisp.gameObject.SetActive(MenuHand.IsActive && pointDisp.Point.IsActive);
			}
		}

	}

}
