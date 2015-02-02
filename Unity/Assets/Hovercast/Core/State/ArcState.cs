using System.Collections.Generic;
using System.Linq;
using Hovercast.Core.Input;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class ArcState {

		public delegate void LevelChangeHandler(int pDirection);
		public delegate void IsLeftChangeHandler();

		public event LevelChangeHandler OnLevelChange;
		public event IsLeftChangeHandler OnIsLeftChange;

		public bool IsActive { get; private set; }
		public bool IsLeft { get; private set; }
		public Vector3 Center { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }
		public float DisplayStrength { get; private set; }
		public float NavBackStrength { get; private set; }
		public ArcSegmentState NearestSegment { get; private set; }

		private readonly IInputProvider vInputProv;
		private readonly NavigationProvider vNavProv;
		private readonly IList<ArcSegmentState> vSegments;
		private readonly InteractionSettings vSettings;
		private bool vIsGrabbing;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArcState(IInputProvider pInputProv, NavigationProvider pNavProv, 
																		InteractionSettings pSettings) {
			vInputProv = pInputProv;
			vNavProv = pNavProv;
			vSegments = new List<ArcSegmentState>();
			vSettings = pSettings;

			IsLeft = vSettings.IsMenuOnLeftSide;

			OnLevelChange += (d => {});
			OnIsLeftChange += (() => {});

			vNavProv.OnLevelChange += HandleLevelChange;
			HandleLevelChange(0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArcSegmentState[] GetSegments() {
			return vSegments.ToArray();
		}

		/*--------------------------------------------------------------------------------------------*/
		public NavItem GetLevelParentItem() {
			NavLevel parNavLevel = vNavProv.GetParentLevel();
			return (parNavLevel == null ? null : parNavLevel.LastSelectedParentItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetLevelTitle() {
			return vNavProv.GetLevelTitle();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput() {
			if ( vSettings.IsMenuOnLeftSide != IsLeft ) {
				IsLeft = vSettings.IsMenuOnLeftSide;
				OnIsLeftChange();
			}

			IInputSide inputSide = vInputProv.GetSide(IsLeft);
			IsActive = inputSide.IsActive;

			if ( !IsActive ) {
				Center = Vector3.zero;
				Rotation = Quaternion.identity;
				Size = 0;
				DisplayStrength = 0;
				NavBackStrength = 0;
				return;
			}

			IInputMenu inputMenu = inputSide.Menu;

			Center = inputMenu.Position;
			Rotation = inputMenu.Rotation;
			Size = inputMenu.Radius;
			DisplayStrength = inputMenu.DisplayStrength;
			NavBackStrength = inputMenu.NavigateBackStrength;

			CheckGrabGesture(inputMenu);
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithCursor(CursorState pCursor) {
			bool allowSelect = (pCursor != null && DisplayStrength > 0);
			NearestSegment = null;

			foreach ( ArcSegmentState seg in vSegments ) {
				seg.UpdateWithCursor(pCursor != null ? pCursor.Position : null);

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

			foreach ( ArcSegmentState seg in vSegments ) {
				if ( seg.SetAsNearestSegment(seg == NearestSegment) ) {
					break;
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
				vNavProv.Back();
				return;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			vSegments.Clear();

			NavLevel navLevel = vNavProv.GetLevel();

			foreach ( NavItem navItem in navLevel.Items ) {
				var seg = new ArcSegmentState(navItem, vSettings);
				vSegments.Add(seg);
			}

			OnLevelChange(pDirection);
		}

	}

}
