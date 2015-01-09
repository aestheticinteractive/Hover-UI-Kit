using System;
using Henu.Input;
using Henu.Navigation;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class MenuPointState {

		public static float HighlightDistanceMin = 0.025f;
		public static float HighlightDistanceMax = 0.12f;
		public static float SelectionMilliseconds = 1000;

		public delegate void NavItemChangeHandler(int pDirection);

		public event NavItemChangeHandler OnNavItemChange;

		public InputPointZone Zone { get; set; }
		public Vector3 Position { get; private set; }
		public Vector3 Direction { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Strength { get; private set; }

		public Vector3 SelectionPosition { get; private set; }
		public float HighlightDistance { get; private set; }
		public float HighlightProgress { get; private set; }

		private readonly InputPointProvider vInputPointProv;
		private readonly NavItemProvider vNavItemProv;
		private bool vIsActive;
		private float vSelectionExtension;
		private DateTime? vSelectionStart;
		private bool vIsAnimating;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuPointState(InputPointZone pZone, InputPointProvider pInputPointProv,
																		NavItemProvider pNavItemProv) {
			Zone = pZone;
			vInputPointProv = pInputPointProv;
			vNavItemProv = pNavItemProv;

			OnNavItemChange = (d => {});
			vNavItemProv.OnItemChange += (d => OnNavItemChange(d));
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsActive {
			get {
				return (vIsActive && vNavItemProv.Item != null);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public NavItem NavItem {
			get {
				return vNavItemProv.Item;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				if ( vSelectionStart == null ) {
					return 0;
				}

				float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
				return Math.Min(1, ms/SelectionMilliseconds);
			}
		}



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			InputPoint inputPoint = vInputPointProv.Point;

			if ( inputPoint == null ) {
				Reset();
			}
			else {
				UpdateWithInputPoint(inputPoint);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithCursor(Vector3? pCursorPosition) {
			if ( pCursorPosition == null || !IsActive || vIsAnimating ) {
				HighlightProgress = 0;
				vSelectionStart = null;
				return;
			}

			float dist = (SelectionPosition-(Vector3)pCursorPosition).magnitude;
			float prog = 1-(dist-HighlightDistanceMin)/(HighlightDistanceMax-HighlightDistanceMin);

			HighlightDistance = dist;
			HighlightProgress = Math.Max(0, Math.Min(1, prog));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetSelectionExtension(float pExtension) {
			vSelectionExtension = pExtension;
			CalcHighlightPosition();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ContinueSelectionProgress(bool pContinue) {
			if ( !pContinue ) {
				vSelectionStart = null;
				return;
			}

			NavItem item = vNavItemProv.Item;

			if ( item.Selected && item.Type == NavItem.ItemType.Radio ) {
				vSelectionStart = null;
				return;
			}

			if ( vSelectionStart == null ) {
				vSelectionStart = DateTime.UtcNow;
				return;
			}

			if ( SelectionProgress < 1 ) {
				return;
			}

			vNavItemProv.Select();
			vSelectionStart = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetIsAnimating(bool pIsAnimating) {
			vIsAnimating = pIsAnimating;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void Reset() {
			vIsActive = false;

			Position = Vector3.zero;
			Direction = Vector3.zero;
			Rotation = Quaternion.identity;
			Strength = 0;

			SelectionPosition = Vector3.zero;
			HighlightDistance = float.MaxValue;
			HighlightProgress = 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithInputPoint(InputPoint pInputPoint) {
			vIsActive = true;

			Position = pInputPoint.Position;
			Direction = pInputPoint.Direction;
			Rotation = pInputPoint.Rotation;
			Strength = pInputPoint.Extension;

			CalcHighlightPosition();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void CalcHighlightPosition() {
			SelectionPosition = Position - Direction*vSelectionExtension;
		}

	}

}
