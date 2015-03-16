using System;
using UnityEngine;

namespace Hover.Engines.Unity {

	/*================================================================================================*/
	public class ContainerWrap : IContainer {

		public GameObject UnityGameObject { get; private set; }

		public ITransform Transform { get; private set; }
		public IRenderer Renderer { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ContainerWrap(GameObject pUnityGameObject) {
			UnityGameObject = pUnityGameObject;
			Initialize();
		}

		/*--------------------------------------------------------------------------------------------*/
		public ContainerWrap(string pName, ContainerType pType, IContainer pParent=null) {
			switch ( pType ) {
				case ContainerType.Empty:
					UnityGameObject = new GameObject();
					break;

				case ContainerType.Cube:
					UnityGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
					break;

				case ContainerType.Quad:
					UnityGameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
					break;

				case ContainerType.Sphere:
					UnityGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					break;

				case ContainerType.CustomMesh:
					UnityGameObject = new GameObject();
					UnityGameObject.AddComponent<MeshRenderer>();
					UnityGameObject.AddComponent<MeshFilter>();
					break;
			}

			UnityGameObject.name = pName;
			Initialize();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void Initialize() {
			Transform = new TransformWrap(UnityGameObject.transform);
			Renderer = new RendererWrap(UnityGameObject.renderer);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		public IContainer[] GetChildren() {
			return null;
		}

		/*--------------------------------------------------------------------------------------------* /
		public IContainer FindChildByName(string pName, bool pRecursive=true) {
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddChildContainer(IContainer pChild) {
			TransformWrap txWrap = (pChild.Transform as TransformWrap);

			if ( txWrap == null ) {
				throw new Exception("This method requires a Unity-related implementation.");
			}

			txWrap.UnityTransform.SetParent(UnityGameObject.transform, false);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddComponent<T>(T pComponent) where T : IComponent, new() {
			UnityGameObject.AddComponent<ComponentHarness<T>>();
		}

	}

}
