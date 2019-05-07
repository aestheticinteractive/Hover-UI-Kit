using System;
using System.Collections.Generic;
using Hover.Core.Cursors;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsRaycastManager : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;

		private List<HoverItemHighlightState> vHighStates;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = HoverCursorDataProvider.Instance;
			}

			if ( CursorDataProvider == null ) {
				throw new ArgumentNullException("CursorDataProvider");
			}

			vHighStates = new List<HoverItemHighlightState>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			CalcNearestRaycastResults();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void CalcNearestRaycastResults() {
			List<ICursorData> cursors = CursorDataProvider.SelectableCursors;
			int cursorCount = cursors.Count;
			bool didFillList = false;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				ICursorData cursor = cursors[i];

				if ( !cursor.IsRaycast ) {
					continue;
				}

				if ( !didFillList ) {
					GetComponent<HoverItemsManager>().FillListWithActiveItemComponents(vHighStates);
					didFillList = true;
				}

				cursor.BestRaycastResult = CalcNearestRaycastResult(cursor);

				/*if ( cursor.BestRaycastResult != null ) {
					Color col = (cursor.BestRaycastResult.Value.IsHit ? Color.green : Color.red);
					Debug.DrawLine(cursor.BestRaycastResult.Value.WorldPosition, 
						cursor.WorldPosition, col);
				}*/
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private RaycastResult? CalcNearestRaycastResult(ICursorData pCursor) {
			float minHighSqrDist = float.MaxValue;
			float minCastSqrDist = float.MaxValue;
			var worldRay = new Ray(pCursor.WorldPosition,
				pCursor.WorldRotation*pCursor.RaycastLocalDirection);

			RaycastResult result = new RaycastResult();
			result.WorldPosition = worldRay.GetPoint(10000);
			result.WorldRotation = pCursor.WorldRotation;

			for ( int i = 0 ; i < vHighStates.Count ; i++ ) {
				HoverItemHighlightState item = vHighStates[i];

				if ( !item.isActiveAndEnabled || item.IsHighlightPreventedViaAnyDisplay() ) {
					continue;
				}

				RaycastResult raycast;
				Vector3 nearHighWorldPos = item.ProximityProvider
					.GetNearestWorldPosition(worldRay, out raycast);

				if ( !raycast.IsHit ) {
					continue;
				}

				float highSqrDist = (raycast.WorldPosition-nearHighWorldPos).sqrMagnitude;
				float castSqrDist = (raycast.WorldPosition-pCursor.WorldPosition).sqrMagnitude;
				//float hitSqrDist = Mathf.Pow(item.InteractionSettings.HighlightDistanceMin, 2);
				float hitSqrDist = 0.0000001f;

				highSqrDist = Mathf.Max(highSqrDist, hitSqrDist);

				bool isHighlightWorse = (highSqrDist > minHighSqrDist);
				bool isComparingHits = (highSqrDist <= hitSqrDist && minHighSqrDist <= hitSqrDist);
				bool isRaycastNearer = (castSqrDist < minCastSqrDist);

				if ( isHighlightWorse || (isComparingHits && !isRaycastNearer) ) {
					continue;
				}

				minHighSqrDist = highSqrDist;
				minCastSqrDist = castSqrDist;
				result = raycast;
			}

			return result;
		}

	}

}
