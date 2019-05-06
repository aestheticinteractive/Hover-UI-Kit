using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastActiveDirection : TreeUpdateableBehavior, ISettingsController {

		public const string ActiveWhenFacingTransformName = "ActiveWhenFacingTransform";

		public ISettingsControllerMap Controllers { get; private set; }
		public float CurrentDegree { get; private set; }

		[SerializeField]
		[FormerlySerializedAs("ActiveWhenFacingMainCamera")]
		private bool _ActiveWhenFacingMainCamera = true;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ActiveWhenFacingTransform")]
		private Transform _ActiveWhenFacingTransform;

		[SerializeField]
		[FormerlySerializedAs("ChildForActivation")]
		private GameObject _ChildForActivation;

		[SerializeField]
		[FormerlySerializedAs("LocalFacingDirection")]
		private Vector3 _LocalFacingDirection = Vector3.forward;

		[SerializeField]
		[Range(10, 180)]
		[FormerlySerializedAs("FullyActiveWithinDegree")]
		private float _FullyActiveWithinDegree = 30;

		[SerializeField]
		[Range(10, 180)]
		[FormerlySerializedAs("InactiveOutsideDegree")]
		private float _InactiveOutsideDegree = 55;

		//TODO: events for active, inactive, fully-active


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastActiveDirection() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool ActiveWhenFacingMainCamera {
			get => _ActiveWhenFacingMainCamera;
			set => this.UpdateValueWithTreeMessage(ref _ActiveWhenFacingMainCamera, value, "ActFaceMc");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Transform ActiveWhenFacingTransform {
			get => _ActiveWhenFacingTransform;
			set => this.UpdateValueWithTreeMessage(ref _ActiveWhenFacingTransform, value, "ActFaceTx");
		}

		/*--------------------------------------------------------------------------------------------*/
		public GameObject ChildForActivation {
			get => _ChildForActivation;
			set => this.UpdateValueWithTreeMessage(ref _ChildForActivation, value, "ChildForActiv");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 LocalFacingDirection {
			get => _LocalFacingDirection;
			set => this.UpdateValueWithTreeMessage(ref _LocalFacingDirection, value, "LocalFacingDir");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float FullyActiveWithinDegree {
			get => _FullyActiveWithinDegree;
			set => this.UpdateValueWithTreeMessage(ref _FullyActiveWithinDegree, value, "FullyActDeg");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InactiveOutsideDegree {
			get => _InactiveOutsideDegree;
			set => this.UpdateValueWithTreeMessage(ref _InactiveOutsideDegree, value, "InactOutDeg");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ChildForActivation == null ) {
				ChildForActivation = gameObject.transform.GetChild(0).gameObject;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			UpdateFacingTransform();
			UpdateDegree();
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateFacingTransform() {
			if ( ActiveWhenFacingMainCamera ) {
				Controllers.Set(ActiveWhenFacingTransformName, this);
			}

			if ( ActiveWhenFacingMainCamera || ActiveWhenFacingTransform == null ) {
				ActiveWhenFacingTransform = (Camera.main == null ? transform : Camera.main.transform);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDegree() {
			HovercastInterface cast = GetComponent<HovercastInterface>();

			Vector3 castWorldDir = cast.transform.TransformDirection(LocalFacingDirection.normalized);
			Vector3 castToTxWorldVec = (ActiveWhenFacingTransform.position-cast.transform.position);
			Vector3 castToTxWorldDir = castToTxWorldVec.normalized;
			float dotBetweenDirs = Vector3.Dot(castWorldDir, castToTxWorldDir);

			if ( dotBetweenDirs >= 1 ) {
				CurrentDegree = 0;
			}
			else {
				CurrentDegree = Mathf.Acos(dotBetweenDirs)/Mathf.PI*180;
			}

			ChildForActivation.SetActive(CurrentDegree <= InactiveOutsideDegree);

			//Vector3 castPos = cast.transform.position;
			//Debug.DrawLine(castPos, castPos+castWorldDir, Color.red);
			//Debug.DrawLine(castPos, castPos+castToTxWorldDir, Color.cyan);
		}

	}

}
