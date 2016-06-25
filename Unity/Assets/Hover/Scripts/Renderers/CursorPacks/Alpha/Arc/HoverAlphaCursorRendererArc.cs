using Hover.Renderers.Packs.Alpha.Arc;
using Hover.Renderers.Shapes.Arc;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.CursorPacks.Alpha.Arc {

	/*================================================================================================*/
	public class HoverAlphaCursorRendererArc : HoverAlphaCursorRenderer, ICursorRendererArc {
	
		public const string OuterRadiusName = "_OuterRadius";
		public const string InnerRadiusName = "_InnerRadius";
		public const string ArcAngleName = "_ArcAngle";

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _OuterRadius = 0.75f;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _InnerRadius = 0.55f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		private float _ArcAngle = 270;

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverAlphaFillArcButton Fill;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float OuterRadius {
			get { return _OuterRadius; }
			set { _OuterRadius = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InnerRadius {
			get { return _InnerRadius; }
			set { _InnerRadius = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float ArcAngle {
			get { return _ArcAngle; }
			set { _ArcAngle = value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			UpdateControl();
			UpdateGeneralSettings();

			RendererController = null;
			Controllers.TryExpireControllers();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Fill = BuildFill();
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaFillArcButton BuildFill() {
			var arcGo = new GameObject("Fill");
			arcGo.transform.SetParent(gameObject.transform, false);
			arcGo.transform.localRotation = Quaternion.Euler(0, 0, 90);
			return arcGo.AddComponent<HoverAlphaFillArcButton>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateControl() {
			Fill.Controllers.Set(HoverFillArcButton.OuterRadiusName, this);
			Fill.Controllers.Set(HoverFillArcButton.InnerRadiusName, this);
			Fill.Controllers.Set(HoverFillArcButton.ArcAngleName, this);
			Fill.Controllers.Set(HoverAlphaFillArcButton.AlphaName, this);
			Fill.Controllers.Set(HoverFill.SortingLayerName, this);
			
			ISettingsController cont = RendererController;

			if ( cont == null ) {
				return;
			}
			
			Controllers.Set(OuterRadiusName, cont);
			Controllers.Set(InnerRadiusName, cont);
			Controllers.Set(ArcAngleName, cont);
			Controllers.Set(IsEnabledName, cont);
			Controllers.Set(SortingLayerName, cont);

			Fill.Controllers.Set(HoverFillArcButton.HighlightProgressName, cont);
			Fill.Controllers.Set(HoverFillArcButton.SelectionProgressName, cont);
			Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			float currAlpha = (IsEnabled ? EnabledAlpha : DisabledAlpha);

			Fill.OuterRadius = OuterRadius;
			Fill.InnerRadius = InnerRadius;
			Fill.ArcAngle = ArcAngle;
			Fill.Alpha = currAlpha;
			Fill.SortingLayer = SortingLayer;

			if ( RendererController == null ) {
				return;
			}

			Fill.HighlightProgress = HighlightProgress;
			Fill.SelectionProgress = SelectionProgress;
		}
		
	}

}
