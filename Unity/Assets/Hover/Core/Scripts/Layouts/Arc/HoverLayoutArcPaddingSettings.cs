using System;
using UnityEngine;

namespace Hover.Core.Layouts.Arc {

    /*================================================================================================*/
    [Serializable]
	public class HoverLayoutArcPaddingSettings {

		public float InnerRadius = 0;
		public float OuterRadius = 0;

		[Range(0, 180)]
		public float StartDegree = 0;

		[Range(0, 180)]
		public float EndDegree = 0;

		public float Between = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void ClampValues(HoverLayoutArcRow pArcRow) {
			float radThick = Mathf.Abs(pArcRow.OuterRadius-pArcRow.InnerRadius);
			ClampValues(pArcRow.ArcDegrees, radThick);
			Between = Mathf.Clamp(Between, 0,
				(pArcRow.ArcDegrees-StartDegree-EndDegree)/(pArcRow.ChildCount-1));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ClampValues(HoverLayoutArcStack pArcStack) {
			float radThick = Mathf.Abs(pArcStack.OuterRadius-pArcStack.InnerRadius);
			ClampValues(pArcStack.ArcDegrees, radThick);
			Between = Mathf.Clamp(Between, 0,
				(radThick-InnerRadius-OuterRadius)/(pArcStack.ChildCount-1));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ClampValues(float pArcDegrees, float pRadiusThickness) {
			InnerRadius = Mathf.Clamp(InnerRadius, 0, pRadiusThickness);
			OuterRadius = Mathf.Clamp(OuterRadius, 0, pRadiusThickness-InnerRadius);
			StartDegree = Mathf.Min(StartDegree, pArcDegrees);
			EndDegree = Mathf.Min(EndDegree, pArcDegrees-StartDegree);
		}

	}

}
