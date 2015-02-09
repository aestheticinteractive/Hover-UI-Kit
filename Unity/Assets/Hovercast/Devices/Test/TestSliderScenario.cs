using Hovercast.Demo;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestSliderScenario : MonoBehaviour {

		//TODO: revise scenario to match the latest Demo menu

		private enum ScenarioStage {
			SelectSliderParent,
			SelectSlider,
			DragSliderUpA,
			DragSliderUpB,
			DragSliderDown,
			DeselectSlider,
			Done
		}

		private HovercastTestInputProvider vInputProv;
		private TestInputCursor vCursor;
		private DemoAnimVector3 vAnimPos;
		private ScenarioStage vStage;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vInputProv = gameObject.GetComponent<HovercastTestInputProvider>();
			vCursor = (TestInputCursor)vInputProv.GetSide(false).Cursor;
			vAnimPos = new DemoAnimVector3(2000);
			vStage = ScenarioStage.SelectSliderParent;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vAnimPos.Start(vCursor.Position, vCursor.Position+new Vector3(0.02f, 0, 0));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vStage == ScenarioStage.Done ) {
				return;
			}

			vCursor.gameObject.transform.position = vAnimPos.GetValue();

			if ( vAnimPos.GetProgress() >= 1 ) {
				SetupNextStage();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void SetupNextStage() {
			Vector3 pos = vCursor.Position;

			switch ( vStage ) {
				case ScenarioStage.SelectSliderParent:
					vStage = ScenarioStage.SelectSlider;
					vAnimPos.Start(pos, pos+new Vector3(0.03f, 0, -0.1f));
					break;

				case ScenarioStage.SelectSlider:
					vStage = ScenarioStage.DragSliderUpA;
					vAnimPos.Start(pos, pos+new Vector3(-0.03f, 0, 0.1f));
					break;

				case ScenarioStage.DragSliderUpA:
					vStage = ScenarioStage.DragSliderUpB;
					vAnimPos.Start(pos, pos+new Vector3(0.08f, 0, 0.18f));
					break;

				case ScenarioStage.DragSliderUpB:
					vStage = ScenarioStage.DragSliderDown;
					vAnimPos.Start(pos, pos+new Vector3(-0.02f, 0, -0.04f));
					break;

				case ScenarioStage.DragSliderDown:
					vStage = ScenarioStage.DeselectSlider;
					vAnimPos.Start(pos, pos+new Vector3(-0.2f, 0.4f, -0.5f));
					break;

				case ScenarioStage.DeselectSlider:
					vStage = ScenarioStage.Done;
					break;
			}
		}

	}

}
