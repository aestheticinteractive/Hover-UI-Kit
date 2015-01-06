using System.Collections.Generic;
using HandMenu.Input;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandDisplay : MonoBehaviour {

		public bool IsLeft { get; set; }
		public HandProvider MenuHandProvider { get; set; }
		public HandProvider SelectHandProvider { get; set; }

		private List<PointData.PointZone> vZones;
		private IList<PointDisplay> vFingerDisplays;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vZones = new List<PointData.PointZone> {
				PointData.PointZone.Index,
				PointData.PointZone.IndexMiddle,
				PointData.PointZone.Middle,
				PointData.PointZone.MiddleRing,
				PointData.PointZone.Ring,
				PointData.PointZone.RingPinky,
				PointData.PointZone.Pinky
			};

			vFingerDisplays = new List<PointDisplay>();

			foreach ( PointData.PointZone zone in vZones ) {
				var pointObj = new GameObject("Point-"+zone);
				pointObj.transform.parent = gameObject.transform;

				PointDisplay pointDisp = pointObj.AddComponent<PointDisplay>();
				pointDisp.HandProvider = MenuHandProvider;
				pointDisp.PointProvider = MenuHandProvider.GetPointProvider(zone);
				vFingerDisplays.Add(pointDisp);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			HandData handData = MenuHandProvider.Data;
			bool isActive = (handData != null);
			
			foreach ( PointDisplay fingerDisp in vFingerDisplays ) {
				fingerDisp.gameObject.SetActive(isActive);
			}
		}

	}

}
