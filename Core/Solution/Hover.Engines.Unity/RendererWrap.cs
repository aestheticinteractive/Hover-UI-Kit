using UnityEngine;

namespace Hover.Engines.Unity {

	/*================================================================================================*/
	public class RendererWrap : IRenderer {

		public Renderer UnityRenderer { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RendererWrap(Renderer pUnityRenderer) {
			UnityRenderer = pUnityRenderer;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Color MaterialColor {
			get {
				return UnityRenderer.material.color.ToHover();
			}
			set {
				UnityRenderer.material.color = value.ToUnity();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public int MaterialRenderQueue {
			get {
				return UnityRenderer.material.renderQueue;
			}
			set {
				UnityRenderer.material.renderQueue = value;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetMaterialByShaderName(string pShaderName) {
			UnityRenderer.material = new Material(Shader.Find(pShaderName));
		}

	}

}
