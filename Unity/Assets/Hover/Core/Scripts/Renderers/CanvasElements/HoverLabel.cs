using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hover.Core.Renderers.CanvasElements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(Text))]
	public class HoverLabel : TreeUpdateableBehavior {

		public const string CanvasScaleName = "CanvasScale";
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(RangeMin=0.0001f, RangeMax=1, DisplaySpecials=true)]
		[FormerlySerializedAs("CanvasScale")]
		private float _CanvasScale = 0.0002f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("SizeX")]
		private float _SizeX = 0.1f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("SizeY")]
		private float _SizeY = 0.1f;

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float CanvasScale {
			get => _CanvasScale;
			set => this.UpdateValueWithTreeMessage(ref _CanvasScale, value, "CanvasScale");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeX {
			get => _SizeX;
			set => this.UpdateValueWithTreeMessage(ref _SizeX, value, "SizeX");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeY {
			get => _SizeY;
			set => this.UpdateValueWithTreeMessage(ref _SizeY, value, "SizeY");
		}


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
		public override void TreeUpdate() {
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
