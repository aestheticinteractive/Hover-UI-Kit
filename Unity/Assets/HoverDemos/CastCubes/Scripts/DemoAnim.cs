using System;
using System.Diagnostics;
using UnityEngine;

namespace HoverDemos.CastCubes {

	/*================================================================================================*/
	public abstract class DemoAnim<T> {

		public float DurationMs { get; private set; }
		public T BaseValue { get; private set; }
		public T TargetValue { get; private set; }

		private readonly Stopwatch vStopwatch;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected DemoAnim(float pDurationMs) {
			DurationMs = pDurationMs;
			vStopwatch = new Stopwatch();
		}

		/*--------------------------------------------------------------------------------------------*/
		public abstract T GetValue();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start(T pBase, T pTarget) {
			BaseValue = pBase;
			TargetValue = pTarget;

			vStopwatch.Reset();
			vStopwatch.Start();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float GetProgress() {
			return (float)Math.Min(1, vStopwatch.Elapsed.TotalMilliseconds/DurationMs);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetEasedProgress() {
			float prog = GetProgress();

			if ( prog < 0.5 ) {
				prog *= 2;
				prog = (float)Math.Pow(prog, 2);
				prog /= 2f;
			}
			else {
				prog = (prog-0.5f)*2;
				prog = 1-(float)Math.Pow(1-prog, 2);
				prog = prog/2f+0.5f;
			}

			return prog;
		}

	}


	/*================================================================================================*/
	public class DemoAnimVector3 : DemoAnim<Vector3> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoAnimVector3(float pDurationMs) : base(pDurationMs) {
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetValue() {
			return Vector3.Lerp(BaseValue, TargetValue, GetEasedProgress());
		}

	}


	/*================================================================================================*/
	public class DemoAnimFloat : DemoAnim<float> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoAnimFloat(float pDurationMs) : base(pDurationMs) {
		}

		/*--------------------------------------------------------------------------------------------*/
		public override float GetValue() {
			return Mathf.Lerp(BaseValue, TargetValue, GetEasedProgress());
		}

	}


	/*================================================================================================*/
	public class DemoAnimQuaternion : DemoAnim<Quaternion> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoAnimQuaternion(float pDurationMs) : base(pDurationMs) {
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Quaternion GetValue() {
			return Quaternion.Slerp(BaseValue, TargetValue, GetEasedProgress());
		}

	}

}
