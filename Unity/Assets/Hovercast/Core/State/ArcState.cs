using System;
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
		public float Strength { get; private set; }
		public float GrabStrength { get; private set; }
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

			OnLevelChange = (d => {});
			OnIsLeftChange = (() => {});

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
			IInputCenter inputCenter = inputSide.Center;

			if ( inputCenter == null ) {
				IsActive = false;
				Center = Vector3.zero;
				Rotation = Quaternion.identity;
				Strength = 0;
				GrabStrength = 0;
				return;
			}

			IsActive = true;
			Center = inputCenter.Position;
			Size = 0;
			Rotation = inputCenter.Rotation;

			foreach ( IInputPoint inputPoint in inputSide.Points ) {
				if ( inputPoint == null ) {
					continue;
				}

				Rotation = Quaternion.Slerp(Rotation, inputPoint.Rotation, 0.1f);
				Size = Math.Max(Size, (inputPoint.Position-Center).sqrMagnitude);
			}

			Size = (float)Math.Sqrt(Size);
			Strength = Math.Max(0, (inputCenter.PalmTowardEyes-0.7f)/0.3f);
			GrabStrength = Math.Min(1, inputCenter.GrabStrength/vSettings.NavBackGrabThreshold);
			CheckGrabGesture(inputCenter);
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithCursor(CursorState pCursor) {
			bool allowSelect = (pCursor != null && Strength > 0);
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
		private void CheckGrabGesture(IInputCenter pInputCenter) {
			if ( pInputCenter == null ) {
				vIsGrabbing = false;
				return;
			}

			if ( vIsGrabbing && pInputCenter.GrabStrength < vSettings.NavBackUngrabThreshold ) {
				vIsGrabbing = false;
				return;
			}

			if ( !vIsGrabbing && pInputCenter.GrabStrength > vSettings.NavBackGrabThreshold ) {
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
