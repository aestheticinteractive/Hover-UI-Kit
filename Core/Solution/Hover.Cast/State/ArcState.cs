using System.Collections.Generic;
using System.Linq;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class ArcState {

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChange;

		public bool IsInputAvailable { get; private set; }
		public bool IsLeft { get; private set; }
		public Vector3 Center { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }
		public float DisplayStrength { get; private set; }
		public float NavBackStrength { get; private set; }
		public SegmentState NearestSegment { get; private set; }

		private readonly IItemHierarchy vItemRoot;
		private readonly IList<SegmentState> vSegments;
		private readonly InteractionSettings vSettings;
		private bool vIsGrabbing;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArcState(IItemHierarchy pItemRoot, InteractionSettings pSettings) {
			vItemRoot = pItemRoot;
			vSegments = new List<SegmentState>();
			vSettings = pSettings;

			IsLeft = vSettings.IsMenuOnLeftSide;

			OnLevelChange += (d => {});

			vItemRoot.OnLevelChange += HandleLevelChange;
			HandleLevelChange(0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SegmentState[] GetSegments() {
			return vSegments.ToArray();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem GetLevelParentItem() {
			IItemGroup parGroup = vItemRoot.ParentLevel;
			return (parGroup == null ? null : parGroup.LastSelectedItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetLevelTitle() {
			return vItemRoot.CurrentLevelTitle;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputMenu pInputMenu) {
			IsInputAvailable = pInputMenu.IsAvailable;
			IsLeft = pInputMenu.IsLeft;
			Center = pInputMenu.Position;
			Rotation = pInputMenu.Rotation;
			Size = pInputMenu.Radius;
			DisplayStrength = pInputMenu.DisplayStrength;
			NavBackStrength = pInputMenu.NavigateBackStrength;

			CheckGrabGesture(pInputMenu);
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithCursor(CursorState pCursor) {
			bool allowSelect = (pCursor.IsInputAvailable && DisplayStrength > 0);
			Vector3? cursorPos = (allowSelect ? pCursor.Position : (Vector3?)null);

			NearestSegment = null;

			foreach ( SegmentState seg in vSegments ) {
				seg.UpdateWithCursor(cursorPos);

				if ( !allowSelect ) {
					continue;
				}

				if ( NearestSegment == null ) {
					NearestSegment = seg;
					continue;
				}

				if ( seg.HighlightDistance < NearestSegment.HighlightDistance ) {
					NearestSegment = seg;
				}
			}

			foreach ( SegmentState seg in vSegments ) {
				if ( seg.SetAsNearestSegment(seg == NearestSegment) ) {
					break; //stop loop upon actual selection because the segment list can change
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void CheckGrabGesture(IInputMenu pInputMenu) {
			if ( pInputMenu == null ) {
				vIsGrabbing = false;
				return;
			}

			if ( vIsGrabbing && pInputMenu.NavigateBackStrength <= 0 ) {
				vIsGrabbing = false;
				return;
			}

			if ( !vIsGrabbing && pInputMenu.NavigateBackStrength >= 1 ) {
				vIsGrabbing = true;
				vItemRoot.Back();
				return;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			vSegments.Clear();

			IBaseItem[] items = vItemRoot.CurrentLevel.Items;

			foreach ( IBaseItem item in items ) {
				var seg = new SegmentState(item, vSettings);
				vSegments.Add(seg);
			}

			OnLevelChange(pDirection);
		}

	}

}
