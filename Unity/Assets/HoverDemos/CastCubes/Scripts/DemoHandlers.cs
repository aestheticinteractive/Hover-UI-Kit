using Hover.Core.Items;
using Hover.Core.Items.Types;
using UnityEngine;

namespace HoverDemos.CastCubes {

	/*================================================================================================*/
	[RequireComponent(typeof(DemoEnvironment))]
	public class DemoHandlers : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoEnvironment Enviro {
			get {
				return GetComponent<DemoEnvironment>();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetColorWhite() {
			Enviro.SetColorMode(DemoEnvironment.ColorMode.White);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetColorRandom() {
			Enviro.SetColorMode(DemoEnvironment.ColorMode.Random);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void InitColorHue(IItemData pItemData) {
			if ( IsItemAvailable(pItemData) ) {
				SetColorHue((IItemDataSlider)pItemData);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetColorHue(IItemDataSelectable<float> pItemData) {
			Enviro.SetColorMode(DemoEnvironment.ColorMode.Custom,
				((IItemDataSlider)pItemData).SnappedRangeValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetMotionOrbit(IItemDataSelectable<bool> pItemData) {
			Enviro.ToggleMotion(DemoEnvironment.MotionType.Orbit, pItemData.Value);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetMotionSpin(IItemDataSelectable<bool> pItemData) {
			Enviro.ToggleMotion(DemoEnvironment.MotionType.Spin, pItemData.Value);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetMotionBob(IItemDataSelectable<bool> pItemData) {
			Enviro.ToggleMotion(DemoEnvironment.MotionType.Bob, pItemData.Value);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetMotionGrow(IItemDataSelectable<bool> pItemData) {
			Enviro.ToggleMotion(DemoEnvironment.MotionType.Grow, pItemData.Value);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void InitMotionSpeed(IItemData pItemData) {
			if ( IsItemAvailable(pItemData) ) {
				SetMotionSpeed((IItemDataSlider)pItemData);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetMotionSpeed(IItemDataSelectable<float> pItemData) {
			Enviro.SetMotionSpeed(((IItemDataSlider)pItemData).SnappedRangeValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void InitLightingPos(IItemData pItemData) {
			if ( IsItemAvailable(pItemData) ) {
				SetLightingPos((IItemDataSlider)pItemData);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetLightingPos(IItemDataSelectable<float> pItemData) {
			Enviro.SetLightPos(((IItemDataSlider)pItemData).RangeValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void InitLightingPower(IItemData pItemData) {
			if ( IsItemAvailable(pItemData) ) {
				SetLightingPower((IItemDataSlider)pItemData);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetLightingPower(IItemDataSelectable<float> pItemData) {
			Enviro.SetLightIntensitiy(((IItemDataSlider)pItemData).RangeValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetLightingSpotlight(IItemDataSelectable pItemData) {
			Enviro.ShowSpotlight(pItemData.IsStickySelected);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool IsItemAvailable(IItemData pItemData) {
			return (pItemData.IsEnabled && pItemData.gameObject.activeInHierarchy);
		}

	}

}
