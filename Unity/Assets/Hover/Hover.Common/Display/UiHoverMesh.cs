using System;
using Hover.Common.State;
using Hover.Common.Util;
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

		public MeshBuilder BackgroundMeshBuilder { get; private set; }
		public MeshBuilder EdgeMeshBuilder { get; private set; }
		public MeshBuilder HighlightMeshBuilder { get; private set; }
		public MeshBuilder SelectMeshBuilder { get; private set; }

		public Color BackgroundColor { get; private set; }
		public Color EdgeColor { get; private set; }
		public Color HighlightColor { get; private set; }
		public Color SelectColor { get; private set; }

		protected GameObject vParent;
		protected ReadList<Vector3> vHoverPoints;
		private float vPrevHighAmount;
		private float vPrevSelAmount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void Build(GameObject pParent) {
			vParent = pParent;
			vHoverPoints = new ReadList<Vector3>();

			Background = new GameObject("Background");
			Background.transform.SetParent(pParent.transform, false);
			Background.AddComponent<MeshRenderer>();
			Background.AddComponent<MeshFilter>();

			Edge = new GameObject("Edge");
			Edge.transform.SetParent(pParent.transform, false);
			Edge.AddComponent<MeshRenderer>();
			Edge.AddComponent<MeshFilter>();

			Highlight = new GameObject("Highlight");
			Highlight.transform.SetParent(pParent.transform, false);
			Highlight.AddComponent<MeshRenderer>();
			Highlight.AddComponent<MeshFilter>();

			Select = new GameObject("Select");
			Select.transform.SetParent(pParent.transform, false);
			Select.AddComponent<MeshRenderer>();
			Select.AddComponent<MeshFilter>();

			BackgroundMeshBuilder = new MeshBuilder();
			EdgeMeshBuilder = new MeshBuilder();
			HighlightMeshBuilder = new MeshBuilder();
			SelectMeshBuilder = new MeshBuilder();

			Background.GetComponent<MeshFilter>().sharedMesh = BackgroundMeshBuilder.Mesh;
			Edge.GetComponent<MeshFilter>().sharedMesh = EdgeMeshBuilder.Mesh;
			Highlight.GetComponent<MeshFilter>().sharedMesh = HighlightMeshBuilder.Mesh;
			Select.GetComponent<MeshFilter>().sharedMesh = SelectMeshBuilder.Mesh;

			BackgroundColor = Color.clear;
			EdgeColor = Color.clear;
			SelectColor = Color.clear;
			HighlightColor = Color.clear;

			UpdateAfterResize();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateAfterResize() {
			if ( Background != null ) {
				UpdateMesh(MeshType.Background, BackgroundMeshBuilder);
				BackgroundMeshBuilder.CommitColors(BackgroundColor);
			}

			if ( Edge != null ) {
				UpdateMesh(MeshType.Edge, EdgeMeshBuilder);
				EdgeMeshBuilder.CommitColors(EdgeColor);
			}
			
			if ( Highlight != null ) {
				UpdateMesh(MeshType.Highlight, HighlightMeshBuilder, vPrevHighAmount);
				HighlightMeshBuilder.CommitColors(HighlightColor);
			}

			if ( Select != null ) {
				UpdateMesh(MeshType.Select, SelectMeshBuilder, vPrevSelAmount);
				SelectMeshBuilder.CommitColors(SelectColor);
			}

			UpdateHoverLocalPoints();
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
				BackgroundMeshBuilder.CommitColors(pColor);
			}

			BackgroundColor = pColor;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateEdge(Color pColor) {
			Edge.SetActive(pColor.a > 0);

			if ( pColor != EdgeColor ) {
				EdgeMeshBuilder.CommitColors(pColor);
			}

			EdgeColor = pColor;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateHighlight(Color pColor, float pAmount) {
			Highlight.SetActive(pAmount > 0 && pColor.a > 0);

			bool isNewAmount = (Math.Abs(pAmount-vPrevHighAmount) > 0.005f);

			if ( isNewAmount ) {
				UpdateMesh(MeshType.Highlight, HighlightMeshBuilder, pAmount);
				vPrevHighAmount = pAmount;
			}

			if ( pColor != HighlightColor || isNewAmount ) {
				HighlightMeshBuilder.CommitColors(pColor);
			}

			HighlightColor = pColor;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateSelect(Color pColor, float pAmount) {
			Select.SetActive(pAmount > 0 && pColor.a > 0);

			bool isNewAmount = (Math.Abs(pAmount-vPrevSelAmount) > 0.005f);

			if ( isNewAmount ) {
				UpdateMesh(MeshType.Select, SelectMeshBuilder, pAmount);
				vPrevSelAmount = pAmount;
			}

			if ( pColor != SelectColor || isNewAmount ) {
				SelectMeshBuilder.CommitColors(pColor);
			}

			SelectColor = pColor;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateHoverPoints(IBaseItemPointsState pPointsState);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateMesh(MeshType pType, MeshBuilder pMeshBuild, float pAmount=1);
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateHoverLocalPoints();

	}

}
