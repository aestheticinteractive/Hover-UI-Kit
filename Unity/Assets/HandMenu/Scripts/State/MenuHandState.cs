using System.Collections.Generic;
using HandMenu.Input;

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

		public bool IsActive { get; private set; }
		public bool IsLeft { get; private set; }
		public float PalmTowardEyes { get; private set; }

		private readonly HandProvider vHandProv;
		private readonly IDictionary<PointData.PointZone, MenuPointState> vPointStateMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuHandState(HandProvider pHandProv) {
			vHandProv = pHandProv;
			vPointStateMap = new Dictionary<PointData.PointZone, MenuPointState>();

			foreach ( PointData.PointZone zone in PointZones ) {
				vPointStateMap.Add(zone, 
					new MenuPointState(zone, vHandProv.GetPointProvider(zone)));
			}

			IsLeft = vHandProv.IsLeft;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			HandData data = vHandProv.Data;

			IsActive = (data != null);
			PalmTowardEyes = (data == null ? 0 : data.PalmTowardEyes);

			foreach ( MenuPointState point in vPointStateMap.Values ) {
				point.UpdateAfterInput();
			}
		}


		/*--------------------------------------------------------------------------------------------*/
		public MenuPointState GetPointState(PointData.PointZone pZone) {
			return vPointStateMap[pZone];
		}

	}

}
