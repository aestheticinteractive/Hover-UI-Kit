using Hover.Utils;
using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorFollower : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverCursorDataProvider CursorDataProvider;

		[DisableWhenControlled]
		public CursorType CursorType = CursorType.LeftPalm;

		[DisableWhenControlled]
		public bool FollowCursorActive = true;
		
		public GameObject[] ObjectsToActivate; //should not include self or parent

		[DisableWhenControlled]
		public bool FollowCursorPosition = true;

		[DisableWhenControlled]
		public bool FollowCursorRotation = true;

		[DisableWhenControlled]
		public bool ScaleUsingCursorSize = false;

		[DisableWhenControlled]
		public float CursorSizeMultiplier = 1;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCursorFollower() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Controllers.TryExpireControllers();

			if ( CursorDataProvider == null  ) {
				Debug.LogError("Reference to "+typeof(HoverCursorDataProvider).Name+
					" must be set.", this);
				return;
			}

			IHoverCursorData cursor = CursorDataProvider.GetCursorData(CursorType);

			if ( FollowCursorActive ) {
				foreach ( GameObject go in ObjectsToActivate ) {
					go.SetActive(cursor.IsActive);
				}
			}

			if ( FollowCursorPosition ) {
				Controllers.Set("transform.position", this, 0);
				gameObject.transform.position = cursor.WorldPosition;
			}

			if ( FollowCursorRotation ) {
				Controllers.Set("transform.rotation", this, 0);
				gameObject.transform.rotation = cursor.WorldRotation;
			}

			if ( ScaleUsingCursorSize ) {
				Controllers.Set("transform.localScale", this, 0);
				gameObject.transform.localScale = Vector3.one*(cursor.Size*CursorSizeMultiplier);
			}
		}

	}

}
