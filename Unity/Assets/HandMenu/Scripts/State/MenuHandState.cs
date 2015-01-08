using System;
using System.Collections.Generic;
using HandMenu.Input;
using HandMenu.Navigation;
using UnityEngine;

namespace HandMenu.State {

	/*================================================================================================*/
	public class MenuHandState {

		public static readonly List<PointData.PointZone> PointZones = new List<PointData.PointZone> {
			PointData.PointZone.Index,
			PointData.PointZone.IndexMiddle,
			PointData.PointZone.Middle,
			PointData.PointZone.MiddleRing,
			PointData.PointZone.Ring,
			PointData.PointZone.RingPinky,
			PointData.PointZone.Pinky
		};

		public delegate void LevelChangeHandler(int pDirection);

		public event LevelChangeHandler OnLevelChange;

		public static float BackGrabThreshold = 0.6f;
		public static float BackReleaseThreshold = 0.3f;

		public bool IsActive { get; private set; }
		public bool IsLeft { get; private set; }
		public float Strength { get; private set; }

		private readonly HandProvider vHandProv;
		private readonly NavigationProvider vNavProv;
		private readonly IDictionary<PointData.PointZone, MenuPointState> vPointStateMap;
		private bool vIsGrabbing;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuHandState(HandProvider pHandProv, NavigationProvider pNavProv) {
			vHandProv = pHandProv;
			vNavProv = pNavProv;
			vPointStateMap = new Dictionary<PointData.PointZone, MenuPointState>();

			foreach ( PointData.PointZone zone in PointZones ) {
				var pointState = new MenuPointState(zone, 
					vHandProv.GetPointProvider(zone), vNavProv.GetItemProvider(zone));
				vPointStateMap.Add(zone, pointState);
			}

			IsLeft = vHandProv.IsLeft;

			OnLevelChange = (d => {});
			vNavProv.OnLevelChange += (d => OnLevelChange(d));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			HandData data = vHandProv.Data;
			float palmTowardEyes = (data == null ? 0 : data.PalmTowardEyes);

			IsActive = (data != null);
			Strength = Math.Max(0, (palmTowardEyes-0.7f)/0.3f);
			CheckGrabGesture(data);

			foreach ( MenuPointState point in vPointStateMap.Values ) {
				point.UpdateAfterInput();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithCursor(Vector3? pCursorPosition) {
			bool allowSelect = (Strength > 0);
			MenuPointState selectPoint = null;

			foreach ( MenuPointState point in vPointStateMap.Values ) {
				point.UpdateWithCursor(pCursorPosition);

				if ( !allowSelect || point.HighlightProgress < 1 || point.Strength <= 0 ) {
					continue;
				}

				if ( selectPoint == null ) {
					selectPoint = point;
					continue;
				}

				if ( point.HighlightDistance < selectPoint.HighlightDistance ) {
					selectPoint = point;
				}
			}

			foreach ( MenuPointState point in vPointStateMap.Values ) {
				point.ContinueSelectionProgress(point == selectPoint);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public MenuPointState GetPointState(PointData.PointZone pZone) {
			return vPointStateMap[pZone];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void CheckGrabGesture(HandData pData) {
			if ( pData == null ) {
				vIsGrabbing = false;
				return;
			}

			if ( vIsGrabbing && pData.GrabStrength < BackReleaseThreshold ) {
				vIsGrabbing = false;
				return;
			}

			if ( !vIsGrabbing && pData.GrabStrength > BackGrabThreshold ) {
				vIsGrabbing = true;
				vNavProv.Back();
			}
		}

	}

}
