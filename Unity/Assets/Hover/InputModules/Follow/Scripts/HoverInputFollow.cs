using Hover.Core.Cursors;
using UnityEngine;

namespace Hover.InputModules.Follow {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputFollow : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;

		[Space(12)]

		public FollowCursor Look = new FollowCursor(CursorType.Look);

		[Space(12)]

		public FollowCursor LeftPalm = new FollowCursor(CursorType.LeftPalm);
		public FollowCursor LeftThumb = new FollowCursor(CursorType.LeftThumb);
		public FollowCursor LeftIndex = new FollowCursor(CursorType.LeftIndex);
		public FollowCursor LeftMiddle = new FollowCursor(CursorType.LeftMiddle);
		public FollowCursor LeftRing = new FollowCursor(CursorType.LeftRing);
		public FollowCursor LeftPinky = new FollowCursor(CursorType.LeftPinky);

		[Space(12)]

		public FollowCursor RightPalm = new FollowCursor(CursorType.RightPalm);
		public FollowCursor RightThumb = new FollowCursor(CursorType.RightThumb);
		public FollowCursor RightIndex = new FollowCursor(CursorType.RightIndex);
		public FollowCursor RightMiddle = new FollowCursor(CursorType.RightMiddle);
		public FollowCursor RightRing = new FollowCursor(CursorType.RightRing);
		public FollowCursor RightPinky = new FollowCursor(CursorType.RightPinky);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CursorUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( Look.FollowTransform == null ) {
				Look.FollowTransform = Camera.main.transform;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !CursorUtil.FindCursorReference(this, ref CursorDataProvider, true) ) {
				return;
			}

			if ( !Application.isPlaying ) {
				return;
			}

			CursorDataProvider.MarkAllCursorsUnused();

			Look.UpdateData(CursorDataProvider);

			LeftPalm.UpdateData(CursorDataProvider);
			LeftThumb.UpdateData(CursorDataProvider);
			LeftIndex.UpdateData(CursorDataProvider);
			LeftMiddle.UpdateData(CursorDataProvider);
			LeftRing.UpdateData(CursorDataProvider);
			LeftPinky.UpdateData(CursorDataProvider);

			RightPalm.UpdateData(CursorDataProvider);
			RightThumb.UpdateData(CursorDataProvider);
			RightIndex.UpdateData(CursorDataProvider);
			RightMiddle.UpdateData(CursorDataProvider);
			RightRing.UpdateData(CursorDataProvider);
			RightPinky.UpdateData(CursorDataProvider);

			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}

	}

}
