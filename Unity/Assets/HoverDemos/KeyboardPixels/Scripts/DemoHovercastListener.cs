namespace HoverDemos.KeyboardPixels {

	/*================================================================================================* /
	public class DemoHovercastListener : MonoBehaviour {

		private GameObject vTextField;
		private HoverboardSetup vHoverboardSetup;
		private HovercastSetup vHovercastSetup;
		private ItemPanel[] vKeyboardItemPanels;
		private bool vPrevEnableKey;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		public void Awake() {
			vTextField = GameObject.Find("DemoTextField");
			vHoverboardSetup = GameObject.Find("Hoverboard").GetComponent<HoverboardSetup>();
			vHovercastSetup = GameObject.Find("Hovercast").GetComponent<HovercastSetup>();

			vPrevEnableKey = true;
		}

		/*--------------------------------------------------------------------------------------------* /
		public void Start() {
			vKeyboardItemPanels = vHoverboardSetup.Panels
				.Select(x => x.GetPanel())
				.ToArray();
		}

		/*--------------------------------------------------------------------------------------------* /
		public void Update() {
			IHovercastState state = vHovercastSetup.State;
			bool enableKey = (state.Menu.DisplayStrength <= 0);

			if ( vPrevEnableKey == enableKey ) {
				return;
			}

			vTextField.SetActive(enableKey);

			foreach ( ItemPanel itemPanel in vKeyboardItemPanels ) {
				itemPanel.IsEnabled = enableKey;
			}

			vPrevEnableKey = enableKey;
		}

	}*/

}
