using Hover.Items;
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
		public IHoverCursorData GetCursorData() {
			return CursorDataProvider.GetCursorData(CursorType);
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

			IHoverCursorData cursor = GetCursorData();
			RaycastResult? raycast = cursor.BestRaycastResult;

			if ( FollowCursorActive ) {
				foreach ( GameObject go in ObjectsToActivate ) {
					go.SetActive(cursor.IsActive);
				}
			}

			if ( FollowCursorPosition ) {
				Controllers.Set("transform.position", this, 0);
				gameObject.transform.position = (raycast == null ? 
					cursor.WorldPosition : raycast.Value.WorldPosition);
			}

			if ( FollowCursorRotation ) {
				Controllers.Set("transform.rotation", this, 0);
				gameObject.transform.rotation = (raycast == null ? 
					cursor.WorldRotation : raycast.Value.WorldRotation);
				//TODO: "flatten" the cursor's rotation onto the raycast.WorldPlane
			}

			if ( ScaleUsingCursorSize ) {
				Controllers.Set("transform.localScale", this, 0);
				gameObject.transform.localScale = Vector3.one*(cursor.Size*CursorSizeMultiplier);
			}
		}

	}

}
