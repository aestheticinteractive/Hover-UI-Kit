using System;
using UnityEngine;

namespace Hover.Core.Layouts.Arc {

    /*================================================================================================*/
    [Serializable]
	public class HoverLayoutArcPaddingSettings {

		public float OuterRadius = 0;
		public float InnerRadius = 0;

		[Range(0, 180)]
		public float StartDegree = 0;

		[Range(0, 180)]
		public float EndDegree = 0;

		public float Between = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void ClampValues(HoverLayoutArcRow pArcRow) {
			if ( pArcRow.ChildCount <= 0 ) {
				return;
			}

			float radThick = Mathf.Abs(pArcRow.OuterRadius-pArcRow.InnerRadius);
			ClampValues(pArcRow.ArcDegrees, radThick);
			Between = Mathf.Clamp(Between, 0,
				(pArcRow.ArcDegrees-StartDegree-EndDegree)/(pArcRow.ChildCount-1));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ClampValues(HoverLayoutArcStack pArcStack) {
			if ( pArcStack.ChildCount <= 0 ) {
				return;
			}

			float radThick = Mathf.Abs(pArcStack.OuterRadius-pArcStack.InnerRadius);
			ClampValues(pArcStack.ArcDegrees, radThick);
			Between = Mathf.Clamp(Between, 0,
				(radThick-InnerRadius-OuterRadius)/(pArcStack.ChildCount-1));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ClampValues(float pArcDegrees, float pRadiusThickness) {
			OuterRadius = Mathf.Clamp(OuterRadius, 0, pRadiusThickness-InnerRadius);
			InnerRadius = Mathf.Clamp(InnerRadius, 0, pRadiusThickness);
			StartDegree = Mathf.Min(StartDegree, pArcDegrees);
			EndDegree = Mathf.Min(EndDegree, pArcDegrees-StartDegree);
		}

	}

}
