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
		void AddOrUpdatePlane(string pId, Vector3 pPointWorld, Vector3 pNormalWorld);

		/*--------------------------------------------------------------------------------------------*/
		bool RemovePlane(string pId);

	}

}
