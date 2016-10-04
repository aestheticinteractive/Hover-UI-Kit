using System;

namespace HoverDemos.CastCubes {

	/*================================================================================================*/
	public class DemoMotion {

		public float Position { get; private set; }
		public float Speed { get; private set; }
		public float MaxSpeed { get; private set; }
		public float AccelMs { get; private set; }
		public float GlobalSpeed { get; set; }

		private DateTime? vLastUpdate;
		private DateTime? vStart;
		private float vInitSpeed;
		private float vTargetSpeed;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoMotion(float pMaxSpeed, float pAccelMs) {
			Position = 0;
			Speed = 0;
			MaxSpeed = pMaxSpeed;
			AccelMs = pAccelMs;
			GlobalSpeed = 1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Enable(bool pEnable) {
			vInitSpeed = Speed;
			vTargetSpeed = (pEnable ? MaxSpeed : 0);
			vStart = DateTime.UtcNow;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateSpeed();
			UpdatePosition();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSpeed() {
			if ( vStart == null ) {
				return;
			}

			float percent = (float)(DateTime.UtcNow-(DateTime)vStart).TotalMilliseconds/AccelMs;
			float prog = (float)Math.Min(1, Math.Pow(percent, 2));

			Speed = vInitSpeed+(vTargetSpeed-vInitSpeed)*prog;

			if ( percent >= 1 ) {
				vStart = null;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdatePosition() {
			if ( vLastUpdate == null ) {
				vLastUpdate = DateTime.UtcNow;
				return;
			}

			float ms = (float)(DateTime.UtcNow-(DateTime)vLastUpdate).TotalSeconds;

			Position += Speed*GlobalSpeed*ms;
			vLastUpdate = DateTime.UtcNow;
		}

	}

}
