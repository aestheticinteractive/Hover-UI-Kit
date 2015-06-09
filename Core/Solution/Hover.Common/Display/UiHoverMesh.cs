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

		public Color BackgroundColor { get; private set; }
		public Color EdgeColor { get; private set; }
		public Color HighlightColor { get; private set; }
		public Color SelectColor { get; private set; }

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
			BackgroundMesh.MarkDynamic();

			Edge = new GameObject("Edge");
			Edge.transform.SetParent(pParent.transform, false);
			Edge.AddComponent<MeshRenderer>();
			EdgeMesh = Edge.AddComponent<MeshFilter>().mesh;
			EdgeMesh.MarkDynamic();

			Highlight = new GameObject("Highlight");
			Highlight.transform.SetParent(pParent.transform, false);
			Highlight.AddComponent<MeshRenderer>();
			HighlightMesh = Highlight.AddComponent<MeshFilter>().mesh;
			HighlightMesh.MarkDynamic();

			Select = new GameObject("Select");
			Select.transform.SetParent(pParent.transform, false);
			Select.AddComponent<MeshRenderer>();
			SelectMesh = Select.AddComponent<MeshFilter>().mesh;
			SelectMesh.MarkDynamic();

			BackgroundColor = Color.clear;
			EdgeColor = Color.clear;
			SelectColor = Color.clear;
			HighlightColor = Color.clear;

			UpdateAfterResize();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateAfterResize() {
			if ( Background != null ) {
				UpdateMesh(MeshType.Background, BackgroundMesh);
				Materials.SetMeshColor(BackgroundMesh, BackgroundColor);
			}

			if ( Edge != null ) {
				UpdateMesh(MeshType.Edge, EdgeMesh);
				Materials.SetMeshColor(EdgeMesh, EdgeColor);
			}
			
			if ( Highlight != null ) {
				UpdateMesh(MeshType.Highlight, HighlightMesh, vPrevHighAmount);
				Materials.SetMeshColor(HighlightMesh, HighlightColor);
			}

			if ( Select != null ) {
				UpdateMesh(MeshType.Select, SelectMesh, vPrevSelAmount);
				Materials.SetMeshColor(SelectMesh, SelectColor);
			}

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
			Background.SetActive(pColor.a > 0);

			if ( pColor != BackgroundColor ) {
				Materials.SetMeshColor(BackgroundMesh, pColor);
			}

			BackgroundColor = pColor;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateEdge(Color pColor) {
			Edge.SetActive(pColor.a > 0);

			if ( pColor != EdgeColor ) {
				Materials.SetMeshColor(EdgeMesh, pColor);
			}

			EdgeColor = pColor;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateHighlight(Color pColor, float pAmount) {
			Highlight.SetActive(pAmount > 0 && pColor.a > 0);

			bool isNewAmount = (Math.Abs(pAmount-vPrevHighAmount) > 0.005f);

			if ( isNewAmount ) {
				UpdateMesh(MeshType.Highlight, HighlightMesh, pAmount);
				vPrevHighAmount = pAmount;
			}

			if ( pColor != HighlightColor || isNewAmount ) {
				Materials.SetMeshColor(HighlightMesh, pColor);
			}

			HighlightColor = pColor;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateSelect(Color pColor, float pAmount) {
			Select.SetActive(pAmount > 0 && pColor.a > 0);

			bool isNewAmount = (Math.Abs(pAmount-vPrevSelAmount) > 0.005f);

			if ( isNewAmount ) {
				UpdateMesh(MeshType.Select, SelectMesh, pAmount);
				vPrevSelAmount = pAmount;
			}

			if ( pColor != SelectColor || isNewAmount ) {
				Materials.SetMeshColor(SelectMesh, pColor);
			}

			SelectColor = pColor;
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
