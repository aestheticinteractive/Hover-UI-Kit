using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputSide : IInputSide {

		public bool IsLeft { get; private set; }
		public IInputCenter Center { get; private set; }

		public IInputPoint[] Points { get; private set; }
		public IInputPoint IndexPoint { get; private set; }
		public IInputPoint MiddlePoint { get; private set; }
		public IInputPoint RingPoint { get; private set; }
		public IInputPoint PinkyPoint { get; private set; }

		/*
		han: (0.121, 0.118, -0.020) / (0.100, 0.826, -0.030, 0.554)
		ind: (0.060, 0.134, 0.062) / (0.047, 0.919, 0.082, 0.383)
		mid: (0.031, 0.149, 0.025) / (-0.015, 0.848, 0.011, 0.530)
		rin: (0.029, 0.154, -0.013) / (-0.024, 0.791, -0.043, 0.610)
		pin: (0.040, 0.138, -0.053) / (-0.116, 0.703, -0.073, 0.698)
		*/

		/*
		(-0.077, 0.147, -0.066) / (-0.112, 0.827, -0.254, -0.490)
		(-0.013, 0.174, 0.008) / (-0.026, 0.897, -0.026, -0.441)
		(0.017, 0.171, -0.027) / (-0.103, -0.814, 0.097, 0.563)
		(0.022, 0.165, -0.057) / (-0.149, -0.777, 0.132, 0.598)
		(0.011, 0.155, -0.095) / (-0.232, -0.675, 0.221, 0.664)
		*/


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestInputSide(bool pIsLeft, TestInputCenter pCenter) {
			IsLeft = pIsLeft;
			Center = pCenter;

			GameObject indexObj = pCenter.gameObject.transform.FindChild("IndexPoint").gameObject;
			GameObject midObj = pCenter.gameObject.transform.FindChild("MiddlePoint").gameObject;
			GameObject ringObj = pCenter.gameObject.transform.FindChild("RingPoint").gameObject;
			GameObject pinkyObj = pCenter.gameObject.transform.FindChild("PinkyPoint").gameObject;

			IndexPoint = indexObj.gameObject.GetComponent<TestInputPoint>();
			MiddlePoint = midObj.gameObject.GetComponent<TestInputPoint>();
			RingPoint = ringObj.gameObject.GetComponent<TestInputPoint>();
			PinkyPoint = pinkyObj.gameObject.GetComponent<TestInputPoint>();

			Points = new[] { IndexPoint, MiddlePoint, RingPoint, PinkyPoint };
		}

	}

}
