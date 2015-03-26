using Hover.Common.Input;
using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IHovercursorState {

		HovercursorVisualSettings VisualSettings { get; }
		IHovercursorInput Input { get; }
		Transform CameraTransform { get; }
		CursorType[] InitializedCursorTypes { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ICursorState GetCursorState(CursorType pType);

		/*--------------------------------------------------------------------------------------------*/
		Transform GetCursorTransform(CursorType pType);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//TODO: allow optional boundaries on the plane. Possibly do this via a custom "hit test" ...
		// ... function within InteractionPlaneState.
		IInteractionPlaneState AddPlane(string pId, Transform pTransform, Vector3 pLocalNormal);

		/*--------------------------------------------------------------------------------------------*/
		IInteractionPlaneState GetPlane(string pId);

		/*--------------------------------------------------------------------------------------------*/
		bool RemovePlane(string pId);

	}

}
