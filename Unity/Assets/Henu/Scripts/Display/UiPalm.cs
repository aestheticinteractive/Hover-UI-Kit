using Henu.Display.Default;
using Henu.Navigation;
using Henu.Settings;
using Henu.State;
using UnityEngine;
using UnityEngine.UI;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiPalm : MonoBehaviour {

		private ArcState vArcState;
		private ISettings vSettings;
		private float vDiameter;
		private float vTextScale;

		private GameObject vRendererObj;
		private GameObject vBackground;
		private GameObject vCanvasGroupObj;
		private GameObject vCanvasObj;
		private GameObject vTextObj;
		private ArcSegmentSettings vArcSegSett;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArc, ISettings pSettings) {
			vArcState = pArc;
			vSettings = pSettings;

			vDiameter = 250;
			vTextScale = 0.002f;

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);
			vRendererObj.transform.localPosition = new Vector3(0, -0.3f, 0);

			vBackground = new GameObject("Background");
			vBackground.transform.SetParent(vRendererObj.transform, false);
			vBackground.AddComponent<MeshFilter>();
			vBackground.AddComponent<MeshRenderer>();
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;
			vBackground.transform.localScale = Vector3.one*vTextScale;

			MeshUtil.BuildCircleMesh(vBackground.GetComponent<MeshFilter>().mesh, vDiameter/2f, 36);

			////
			
			vCanvasGroupObj = new GameObject("CanvasGroup");
			vCanvasGroupObj.transform.SetParent(vRendererObj.transform, false);
			vCanvasGroupObj.AddComponent<CanvasGroup>();
			vCanvasGroupObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.down, Vector3.right);
			vCanvasGroupObj.transform.localScale = Vector3.one*vTextScale;

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.SetParent(vCanvasGroupObj.transform, false);
			
			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vDiameter);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vDiameter);
			rect.pivot = new Vector2(0.5f, 0.5f);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.SetParent(vCanvasObj.transform, false);

			Text text = vTextObj.AddComponent<Text>();
			text.font = Resources.Load<Font>("Tahoma");
			text.alignment = TextAnchor.MiddleCenter;

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vDiameter);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vDiameter);

			////

			vArcState.OnLevelChange += HandleLevelChange;
			HandleLevelChange(0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.transform.localPosition = vArcState.Center;
			gameObject.transform.localRotation = vArcState.Rotation;
			gameObject.transform.localScale = Vector3.one*(vArcState.Size*1.1f);

			float mainAlpha = UiArcSegmentRenderer.GetArcAlpha(vArcState);
			Color colBg = vArcSegSett.BackgroundColor;
			colBg.a *= mainAlpha;

			vBackground.renderer.sharedMaterial.color = colBg;
			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = mainAlpha;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			NavItem parNavItem = vArcState.GetLevelParentItem();
			vArcSegSett = vSettings.GetArcSegmentSettings(parNavItem);

			Text text = vTextObj.GetComponent<Text>();
			text.fontSize = vArcSegSett.TextSize;
			text.color = vArcSegSett.TextColor;
			text.text = vArcState.GetLevelTitle();
		}

	}

}
