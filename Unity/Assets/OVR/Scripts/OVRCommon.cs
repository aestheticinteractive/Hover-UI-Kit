/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Ovr;

/// <summary>
/// Miscellaneous extension methods that any script can use.
/// </summary>
public static class OVRExtensions
{
	/// <summary>
	/// Converts a plain C# matrix to a Unity matrix.
	/// </summary>
	/// <returns>The matrix as a Unity Matrix4x4.</returns>
	/// <param name="ovrMat">The matrix as a Matrix4f.</param>
	public static Matrix4x4 ToMatrix4x4(this Matrix4f ovrMat)
	{
        Matrix4x4 mat = new Matrix4x4();

		mat[0, 0] = ovrMat.m[0, 0];
		mat[0, 1] = ovrMat.m[0, 1];
		mat[0, 2] = ovrMat.m[0, 2];
		mat[0, 3] = ovrMat.m[0, 3];
		mat[1, 0] = ovrMat.m[1, 0];
		mat[1, 1] = ovrMat.m[1, 1];
		mat[1, 2] = ovrMat.m[1, 2];
		mat[1, 3] = ovrMat.m[1, 3];
		mat[2, 0] = ovrMat.m[2, 0];
		mat[2, 1] = ovrMat.m[2, 1];
		mat[2, 2] = ovrMat.m[2, 2];
		mat[2, 3] = ovrMat.m[2, 3];
		mat[3, 0] = ovrMat.m[3, 0];
		mat[3, 1] = ovrMat.m[3, 1];
		mat[3, 2] = ovrMat.m[3, 2];
		mat[3, 3] = ovrMat.m[3, 3];

		return mat;
	}

	/// <summary>
	/// Converts a plain C# Sizei to a Unity Vector2.
	/// </summary>
	/// <returns>The size as a Unity Vector2.</returns>
	/// <param name="size">The size as a C# Sizei.</param>
	public static Vector2 ToVector2(this Sizei size)
	{
		return new Vector2(size.w, size.h);
	}

	/// <summary>
	/// Converts a plain C# Vector2i to a Unity Vector2.
	/// </summary>
	/// <returns>The vector as a Unity Vector2.</returns>
	/// <param name="size">The vector as a C# Vector2i.</param>
	public static Vector2 ToVector2(this Vector2i vec)
	{
		return new Vector2(vec.x, vec.y);
	}

	/// <summary>
	/// Converts a plain C# Vector2 to a Unity Vector2.
	/// </summary>
	/// <returns>The vector as a Unity Vector2.</returns>
	/// <param name="size">The vector as a C# Vector2.</param>
	public static Vector2 ToVector2(this Vector2f vec)
	{
		return new Vector2(vec.x, vec.y);
	}

	/// <summary>
	/// Converts a plain C# Vector3 to a Unity Vector3.
	/// </summary>
	/// <returns>The vector as a Unity Vector3.</returns>
	/// <param name="size">The vector as a C# Vector3.</param>
	public static Vector3 ToVector3(this Vector3f vec, bool rhToLh = true)
	{
		Vector3 v = new Vector3(vec.x, vec.y, vec.z);

		if (rhToLh)
			v.z = -v.z;

		return v;
	}
	
	/// <summary>
	/// Converts a plain C# Quatf to a Unity Quaternion.
	/// </summary>
	/// <returns>The quaternion as a Unity Quaternion.</returns>
	/// <param name="size">The quaternion as a C# Quatf.</param>
	public static Quaternion ToQuaternion(this Quatf quat, bool rhToLh = true)
	{
		Quaternion q = new Quaternion(quat.x, quat.y, quat.z, quat.w);

		if (rhToLh)
		{
			q.x = -q.x;
			q.y = -q.y;
		}

		return q;
	}

	/// <summary>
	/// Converts a plain C# Posef to a Unity OVRPose.
	/// </summary>
	/// <returns>The pose as a Unity OVRPose.</returns>
	/// <param name="size">The pose as a C# Posef.</param>
	public static OVRPose ToPose(this Posef pose, bool rhToLh = true)
	{
		return new OVRPose
		{
			position = pose.Position.ToVector3(rhToLh),
			orientation = pose.Orientation.ToQuaternion(rhToLh)
		};
	}
}

/// <summary>
/// An affine transformation built from a Unity position and orientation.
/// </summary>
public struct OVRPose
{
	/// <summary>
	/// The position.
	/// </summary>
	public Vector3 position;

	/// <summary>
	/// The orientation.
	/// </summary>
	public Quaternion orientation;
}

/// <summary>
/// Selects a human eye.
/// </summary>
public enum OVREye
{
	Center = -1,
	Left  = Ovr.Eye.Left,
	Right = Ovr.Eye.Right,
	Count = Ovr.Eye.Count,
}
