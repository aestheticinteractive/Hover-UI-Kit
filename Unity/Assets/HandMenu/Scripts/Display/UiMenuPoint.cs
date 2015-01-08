using System;
using HandMenu.Navigation;
using HandMenu.State;
using UnityEngine;

namespace HandMenu.Display {

	/*================================================================================================*/
	public class UiMenuPoint : MonoBehaviour {

		public static float DataChangeMilliseconds = 1000;
		public static float DataChangeDistance = 0.08f;

		private MenuHandState vHand;
		private MenuPointState vPoint;
		private Renderers vRenderers;

		private GameObject vPrevRendererObj;
		private GameObject vCurrRendererObj;
		private IUiMenuPointRenderer vPrevRenderer;
		private IUiMenuPointRenderer vCurrRenderer;

		private int vRendererCount;
		private DateTime? vChangeTime;
		private int vChangeDir;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(MenuHandState pHand, MenuPointState pPoint, Renderers pRenderers) {
			vHand = pHand;
			vPoint = pPoint;
			vRenderers = pRenderers;

			vPoint.OnDataChange += HandleDataChange;
			HandleDataChange(0);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vPoint.IsActive ) {
				return;
			}

			Transform tx = gameObject.transform;
			tx.localPosition = vPoint.Position;
			tx.localRotation = vPoint.Rotation;

			if ( !vHand.IsLeft ) {
				tx.localRotation *= Quaternion.FromToRotation(Vector3.left, Vector3.right);
			}

			UpdateChangeAnimation();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsActive() {
			return (vPoint != null && vPoint.IsActive);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleDataChange(int pDirection) {
			DestroyPrevRenderer();
			vPrevRendererObj = vCurrRendererObj;
			vPrevRenderer = vCurrRenderer;

			if ( vPoint.Data == null ) {
				vCurrRendererObj = null;
				vCurrRenderer = null;
			}
			else {
				BuildCurrRenderer();
			}

			vChangeTime = DateTime.UtcNow;
			vChangeDir = pDirection;
			UpdateChangeAnimation();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyPrevRenderer() {
			if ( vPrevRendererObj == null ) {
				return;
			}

			vPrevRendererObj.SetActive(false);
			Destroy(vPrevRendererObj);

			vPrevRendererObj = null;
			vPrevRenderer = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCurrRenderer() {
			vCurrRendererObj = new GameObject("Renderer"+vRendererCount);
			vRendererCount++;

			Type rendererType;

			switch ( vPoint.Data.Type ) {
				case NavItemData.ItemType.Parent:
					rendererType = vRenderers.PointParent;
					break;

				case NavItemData.ItemType.Checkbox:
					rendererType = vRenderers.PointCheckbox;
					break;

				case NavItemData.ItemType.Radio:
					rendererType = vRenderers.PointRadio;
					break;

				default:
					rendererType = vRenderers.PointSelection;
					break;
			}

			vCurrRenderer = (IUiMenuPointRenderer)vCurrRendererObj.AddComponent(rendererType);
			vCurrRenderer.Build(vHand, vPoint);
			vCurrRenderer.Update();

			vCurrRendererObj.transform.parent = gameObject.transform;
			vCurrRendererObj.transform.localPosition = Vector3.zero;
			vCurrRendererObj.transform.localRotation = Quaternion.identity;
			vCurrRendererObj.transform.localScale = Vector3.one;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateChangeAnimation() {
			if ( vChangeTime == null ) {
				return;
			}

			float ms = (float)(DateTime.UtcNow-(DateTime)vChangeTime).TotalMilliseconds;
			float prog = Math.Min(1, ms/DataChangeMilliseconds);
			float push = 1-(float)Math.Pow(1-prog, 3);
			float dist = -DataChangeDistance*vChangeDir;

			if ( vPrevRenderer != null ) {
				vPrevRenderer.HandleChangeAnimation(false, vChangeDir, prog);
				vPrevRendererObj.transform.localPosition = new Vector3(0, 0, -dist*push);
			}

			if ( vCurrRenderer != null ) {
				vCurrRenderer.HandleChangeAnimation(true, vChangeDir, prog);
				vCurrRendererObj.transform.localPosition = new Vector3(0, 0, dist*(1-push));
			}

			if ( prog >= 1 ) {
				vChangeTime = null;
				DestroyPrevRenderer();
			}

			vPoint.SetIsAnimating(vChangeTime != null);
		}

	}

}
