using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using System;

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

		[Range(0, 180)]
		public float Between = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void ClampValues(HoverLayoutArcRow pArcRow) {
			float radThick = Mathf.Abs(pArcRow.OuterRadius-pArcRow.InnerRadius);
			ClampValues(pArcRow.ArcDegrees, radThick);
			Between = Mathf.Min(Between, (radThick-InnerRadius-OuterRadius)/pArcRow.ChildCount);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ClampValues(HoverLayoutArcStack pArcStack) {
			float arcDeg = pArcStack.ArcDegrees;
			ClampValues(arcDeg, Mathf.Abs(pArcStack.OuterRadius-pArcStack.InnerRadius));
			Between = Mathf.Min(Between, (arcDeg-StartDegree-EndDegree)/pArcStack.ChildCount);
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
