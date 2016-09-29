using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hover.Core.Renderers.CanvasElements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(Text))]
	public class HoverLabel : MonoBehaviour, ITreeUpdateable {
		
		public const string CanvasScaleName = "CanvasScale";
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(RangeMin=0.0001f, RangeMax=1, DisplaySpecials=true)]
		public float CanvasScale = 0.0002f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeX = 0.1f;

		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverLabel() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Text TextComponent {
			get { return GetComponent<Text>(); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				BuildText();
				_IsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			RectTransform rectTx = TextComponent.rectTransform;
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SizeX/CanvasScale);
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SizeY/CanvasScale);
			Controllers.TryExpireControllers();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildText() {
			Text text = TextComponent;
			text.text = "Label";
			text.font = Resources.Load<Font>("Fonts/Tahoma");
			text.fontSize = 40;
			text.lineSpacing = 0.75f;
			text.color = Color.white;
			text.alignment = TextAnchor.MiddleCenter;
			text.raycastTarget = false;
			text.horizontalOverflow = HorizontalWrapMode.Wrap;
			text.verticalOverflow = VerticalWrapMode.Overflow;
		}

	}

}
