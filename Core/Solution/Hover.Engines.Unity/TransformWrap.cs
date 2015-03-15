namespace Hover.Engines.Unity {

	/*================================================================================================*/
	public class TransformWrap : ITransform {

		public UnityEngine.Transform UnityTransform { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TransformWrap(UnityEngine.Transform pUnityTransform) {
			UnityTransform = pUnityTransform;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 LocalPosition {
			get {
				return UnityTransform.localPosition.ToHover();
			}
			set {
				UnityTransform.localPosition = value.ToUnity();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 WorldPosition {
			get {
				return UnityTransform.position.ToHover();
			}
			set {
				UnityTransform.position = value.ToUnity();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Quaternion LocalRotation {
			get {
				return UnityTransform.localRotation.ToHover();
			}
			set {
				UnityTransform.localRotation = value.ToUnity();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Quaternion WorldRotation {
			get {
				return UnityTransform.rotation.ToHover();
			}
			set {
				UnityTransform.rotation = value.ToUnity();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetWorldPoint(Vector3 pLocalPoint) {
			return UnityTransform.TransformPoint(pLocalPoint.ToUnity()).ToHover();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 CalculateLocalPoint(ITransform pOtherTransform, Vector3 pOtherLocalPoint) {
			Vector3 worldPoint = pOtherTransform.GetWorldPoint(pOtherLocalPoint);
			return UnityTransform.InverseTransformPoint(worldPoint.ToUnity()).ToHover();
		}

	}

}
