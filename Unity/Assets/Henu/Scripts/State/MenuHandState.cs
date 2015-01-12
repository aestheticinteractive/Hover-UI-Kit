using System;
using System.Collections.Generic;
using Henu.Input;
using Henu.Navigation;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class MenuHandState {

		public static readonly List<InputPointZone> PointZones = 
			new List<InputPointZone> {
				InputPointZone.Index,
				InputPointZone.IndexMiddle,
				InputPointZone.Middle,
				InputPointZone.MiddleRing,
				InputPointZone.Ring,
				InputPointZone.RingPinky,
				InputPointZone.Pinky
			};

		public delegate void LevelChangeHandler(int pDirection);

		public event LevelChangeHandler OnLevelChange;

		public static float BackGrabThreshold = 0.6f;
		public static float BackReleaseThreshold = 0.3f;

		public bool IsActive { get; private set; }
		public bool IsLeft { get; private set; }
		public Vector3 Center { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Strength { get; private set; }
		public float GrabStrength { get; private set; }

		private readonly InputHandProvider vInputHandProv;
		private readonly NavigationProvider vNavProv;
		private readonly IDictionary<InputPointZone, MenuPointState> vPointStateMap;
		private bool vIsGrabbing;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuHandState(InputHandProvider pInputHandProv, NavigationProvider pNavProv) {
			vInputHandProv = pInputHandProv;
			vNavProv = pNavProv;
			vPointStateMap = new Dictionary<InputPointZone, MenuPointState>();

			foreach ( InputPointZone zone in PointZones ) {
				var pointState = new MenuPointState(zone, 
					vInputHandProv.GetPointProvider(zone), vNavProv.GetItemProvider(zone));
				vPointStateMap.Add(zone, pointState);
			}

			IsLeft = vInputHandProv.IsLeft;

			OnLevelChange = (d => {});
			vNavProv.OnLevelChange += (d => OnLevelChange(d));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			InputHand inputHand = vInputHandProv.Hand;
			float palmTowardEyes = (inputHand == null ? 0 : inputHand.PalmTowardEyes);
			float grabStrength = (inputHand == null ? 0 : inputHand.GrabStrength);

			IsActive = (inputHand != null);
			Center = (inputHand == null ? Vector3.zero : inputHand.Center);
			Rotation = (inputHand == null ? Quaternion.identity : inputHand.Rotation);
			Strength = Math.Max(0, (palmTowardEyes-0.7f)/0.3f);
			GrabStrength = Math.Min(1, grabStrength/BackGrabThreshold);
			CheckGrabGesture(inputHand);

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
		public MenuPointState GetPointState(InputPointZone pZone) {
			return vPointStateMap[pZone];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void CheckGrabGesture(InputHand pInputHand) {
			if ( pInputHand == null ) {
				vIsGrabbing = false;
				return;
			}

			if ( vIsGrabbing && pInputHand.GrabStrength < BackReleaseThreshold ) {
				vIsGrabbing = false;
				return;
			}

			if ( !vIsGrabbing && pInputHand.GrabStrength > BackGrabThreshold ) {
				vIsGrabbing = true;
				vNavProv.Back();
				return;
			}
		}

	}

}
