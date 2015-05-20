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

		public GameObject Background { get; protected set; }
		public GameObject Edge { get; protected set; }
		public GameObject Highlight { get; protected set; }
		public GameObject Select { get; protected set; }

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

			Background = new GameObject("Background");
			Background.transform.SetParent(pParent.transform, false);
			Background.AddComponent<MeshRenderer>();
			BackgroundMesh = Background.AddComponent<MeshFilter>().mesh;

			Edge = new GameObject("Edge");
			Edge.transform.SetParent(pParent.transform, false);
			Edge.AddComponent<MeshRenderer>();
			EdgeMesh = Edge.AddComponent<MeshFilter>().mesh;

			Highlight = new GameObject("Highlight");
			Highlight.transform.SetParent(pParent.transform, false);
			Highlight.AddComponent<MeshRenderer>();
			HighlightMesh = Highlight.AddComponent<MeshFilter>().mesh;

			Select = new GameObject("Select");
			Select.transform.SetParent(pParent.transform, false);
			Select.AddComponent<MeshRenderer>();
			SelectMesh = Select.AddComponent<MeshFilter>().mesh;

			UpdateAfterResize();

			UpdateBackground(Color.clear);
			UpdateEdge(Color.clear);
			UpdateHighlight(Color.clear, vPrevHighAmount);
			UpdateSelect(Color.clear, vPrevSelAmount);
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
		public virtual void SetDepthHint(int pDepthHint) {
			if ( Background != null ) {
				Background.GetComponent<MeshRenderer>().sharedMaterial = 
					Materials.GetLayer(Materials.Layer.Background, pDepthHint);
			}

			if ( Edge != null ) {
				Edge.GetComponent<MeshRenderer>().sharedMaterial = 
					Materials.GetLayer(Materials.Layer.SelectAndEdge, pDepthHint);
			}

			if ( Highlight != null ) {
				Highlight.GetComponent<MeshRenderer>().sharedMaterial = 
					Materials.GetLayer(Materials.Layer.Highlight, pDepthHint);
			}

			if ( Select != null ) {
				Select.GetComponent<MeshRenderer>().sharedMaterial = 
					Materials.GetLayer(Materials.Layer.SelectAndEdge, pDepthHint);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateBackground(Color pColor) {
			Materials.SetMeshColor(BackgroundMesh, pColor);
			Background.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateEdge(Color pColor) {
			Materials.SetMeshColor(EdgeMesh, pColor);
			Edge.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateHighlight(Color pColor, float pAmount) {
			Materials.SetMeshColor(HighlightMesh, pColor);
			Highlight.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevHighAmount) > 0.005f ) {
				UpdateMesh(MeshType.Highlight, HighlightMesh, pAmount);
				vPrevHighAmount = pAmount;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateSelect(Color pColor, float pAmount) {
			Materials.SetMeshColor(SelectMesh, pColor);
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
