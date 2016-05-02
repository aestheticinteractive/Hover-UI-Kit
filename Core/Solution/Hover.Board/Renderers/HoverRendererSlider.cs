using System;
using Hover.Board.Renderers.Fills;
using Hover.Board.Renderers.Contents;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererSlider : MonoBehaviour {
	
		public GameObject Container;
		public HoverRendererHollowRectangle BackgroundA;
		public HoverRendererHollowRectangle BackgroundB;
		public HoverRendererHollowRectangle BackgroundC;
		public HoverRendererButton HandleButton;
		public HoverRendererButton JumpButton;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 1)]
		public float HandlePosition = 0.5f;
		
		[Range(0, 1)]
		public float JumpPosition = 0;
		
		public bool ShowJump = false;
		
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !vIsBuilt ) {
				BuildElements();
				vIsBuilt = true;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateGeneralSettings();
			//UpdateAnchorSettings();

			BackgroundA.UpdateAfterRenderer();
			BackgroundB.UpdateAfterRenderer();
			BackgroundC.UpdateAfterRenderer();
			HandleButton.UpdateAfterRenderer();
			JumpButton.UpdateAfterRenderer();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			Container = new GameObject("Container");
			Container.transform.SetParent(gameObject.transform, false);
			
			BackgroundA = BuildHollowRect("BackgroundA");
			BackgroundB = BuildHollowRect("BackgroundB");
			BackgroundC = BuildHollowRect("BackgroundC");
			HandleButton = BuildButton("Handle");
			JumpButton = BuildButton("Jump");
			
			BackgroundA.FillColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
			BackgroundB.FillColor = BackgroundA.FillColor;
			BackgroundC.FillColor = BackgroundA.FillColor;
			
			HandleButton.SizeY = 2;
			JumpButton.SizeY = 1;
			
			JumpButton.Canvas.gameObject.SetActive(false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererHollowRectangle BuildHollowRect(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(Container.transform, false);
			return rectGo.AddComponent<HoverRendererHollowRectangle>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererButton BuildButton(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(Container.transform, false);
			return rectGo.AddComponent<HoverRendererButton>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			BackgroundA.ControlledByRenderer = true;
			BackgroundB.ControlledByRenderer = true;
			BackgroundC.ControlledByRenderer = true;
			HandleButton.ControlledByRenderer = true;
			JumpButton.ControlledByRenderer = true;
			
			BackgroundA.SizeX = SizeX*0.8f;
			BackgroundB.SizeX = BackgroundA.SizeX;
			BackgroundC.SizeX = BackgroundA.SizeX;
			
			BackgroundA.InnerAmount = 0;
			BackgroundB.InnerAmount = 0;
			BackgroundC.InnerAmount = 0;
			
			float availHandleSizeY = SizeY-HandleButton.SizeY;
			
			BackgroundA.SizeY = Mathf.Max(0, availHandleSizeY*HandlePosition);
			BackgroundB.SizeY = 0; //SizeY-handleY-HandleButton.SizeY/2;
			BackgroundC.SizeY = Mathf.Max(0, availHandleSizeY*(1-HandlePosition));
			
			BackgroundA.transform.localPosition = new Vector3(0, (-SizeY+BackgroundA.SizeY)/2, 0);
			BackgroundC.transform.localPosition = new Vector3(0, ( SizeY-BackgroundC.SizeY)/2, 0);
			//BackgroundB.transform.localPosition = new Vector3(0, handleY+HandleButton.SizeY/2, 0);
			
			HandleButton.transform.localPosition = 
				new Vector3(0, availHandleSizeY*(HandlePosition-0.5f), 0);
			
			if ( ShowJump ) {
				float availJumpSizeY = SizeY-JumpButton.SizeY;
				float jumpY = availJumpSizeY*JumpPosition;
				
				JumpButton.transform.localPosition = new Vector3(0, jumpY, 0);
			}
			
			//JumpButton.gameObject.SetActive(ShowJump);
			//BackgroundB.gameObject.SetActive(false);
			//BackgroundC.gameObject.SetActive(ShowJump);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorType.Custom ) {
				return;
			}
			
			int ai = (int)Anchor;
			float x = (ai%3)/2f - 0.5f;
			float y = (ai/3)/2f - 0.5f;
			var localPos = new Vector3(-SizeX*x, SizeY*y, 0);
			
			Container.transform.localPosition = localPos;
		}
		
	}

}
