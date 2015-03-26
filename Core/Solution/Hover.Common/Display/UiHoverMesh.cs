using System;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Common.Display {

	/*================================================================================================*/
	public abstract class UiHoverMesh {

		protected enum MeshType {
			Background,
			Edge,
			Highlight,
			Select
		}

		public GameObject Background { get; private set; }
		public GameObject Edge { get; private set; }
		public GameObject Highlight { get; private set; }
		public GameObject Select { get; private set; }

		public Mesh BackgroundMesh { get; private set; }
		public Mesh EdgeMesh { get; private set; }
		public Mesh HighlightMesh { get; private set; }
		public Mesh SelectMesh { get; private set; }

		protected GameObject vParent;
		protected Vector3[] vHoverPoints;
		private float vPrevHighAmount;
		private float vPrevSelAmount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void Build(GameObject pParent) {
			vParent = pParent;

			Shader shader = Shader.Find("Unlit/AlphaSelfIllum");

			Background = new GameObject("Background");
			Background.transform.SetParent(pParent.transform, false);
			Background.AddComponent<MeshRenderer>();
			Background.renderer.sharedMaterial = new Material(shader);
			Background.renderer.sharedMaterial.renderQueue -= 300;
			Background.renderer.sharedMaterial.color = Color.clear;
			BackgroundMesh = Background.AddComponent<MeshFilter>().mesh;

			Edge = new GameObject("Edge");
			Edge.transform.SetParent(pParent.transform, false);
			Edge.AddComponent<MeshRenderer>();
			Edge.renderer.sharedMaterial = new Material(shader);
			Edge.renderer.sharedMaterial.renderQueue -= 50;
			Edge.renderer.sharedMaterial.color = Color.clear;
			EdgeMesh = Edge.AddComponent<MeshFilter>().mesh;

			Highlight = new GameObject("Highlight");
			Highlight.transform.SetParent(pParent.transform, false);
			Highlight.AddComponent<MeshRenderer>();
			Highlight.renderer.sharedMaterial = new Material(shader);
			Highlight.renderer.sharedMaterial.renderQueue -= 200;
			Highlight.renderer.sharedMaterial.color = Color.clear;
			HighlightMesh = Highlight.AddComponent<MeshFilter>().mesh;

			Select = new GameObject("Select");
			Select.transform.SetParent(pParent.transform, false);
			Select.AddComponent<MeshRenderer>();
			Select.renderer.sharedMaterial = new Material(shader);
			Select.renderer.sharedMaterial.renderQueue -= 100;
			Select.renderer.sharedMaterial.color = Color.clear;
			SelectMesh = Select.AddComponent<MeshFilter>().mesh;

			UpdateAfterResize();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateAfterResize() {
			UpdateMesh(MeshType.Background, BackgroundMesh);
			UpdateMesh(MeshType.Edge, EdgeMesh);
			UpdateMesh(MeshType.Highlight, HighlightMesh, vPrevHighAmount);
			UpdateMesh(MeshType.Select, SelectMesh, vPrevSelAmount);

			vHoverPoints = CalcHoverLocalPoints();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateBackground(Color pColor) {
			Background.renderer.sharedMaterial.color = pColor;
			Background.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateEdge(Color pColor) {
			Edge.renderer.sharedMaterial.color = pColor;
			Edge.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateHighlight(Color pColor, float pAmount) {
			Highlight.renderer.sharedMaterial.color = pColor;
			Highlight.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevHighAmount) > 0.005f ) {
				UpdateMesh(MeshType.Highlight, HighlightMesh, pAmount);
				vPrevHighAmount = pAmount;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateSelect(Color pColor, float pAmount) {
			Select.renderer.sharedMaterial.color = pColor;
			Select.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevSelAmount) > 0.005f ) {
				UpdateMesh(MeshType.Select, SelectMesh, pAmount);
				vPrevSelAmount = pAmount;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateHoverPoints(IBaseItemPointsState pPointsState);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateMesh(MeshType pType, Mesh pMesh, float pAmount=1);
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Vector3[] CalcHoverLocalPoints();

	}

}
