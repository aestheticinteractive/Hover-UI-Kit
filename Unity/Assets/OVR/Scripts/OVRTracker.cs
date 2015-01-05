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

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Ovr;

/// <summary>
/// An infrared camera that tracks the position of a head-mounted display.
/// </summary>
public class OVRTracker
{
	/// <summary>
	/// The (symmetric) visible area in front of the tracker.
	/// </summary>
	public struct Frustum
	{
		/// <summary>
		/// The tracker cannot track the HMD unless it is at least this far away.
		/// </summary>
		public float nearZ;
		/// <summary>
		/// The tracker cannot track the HMD unless it is at least this close.
		/// </summary>
		public float farZ;
		/// <summary>
		/// The tracker's horizontal and vertical fields of view in degrees.
		/// </summary>
		public Vector2 fov;
	}

	/// <summary>
	/// If true, a tracker is attached to the system.
	/// </summary>
	public bool isPresent
	{
	    get {
#if !UNITY_ANDROID || UNITY_EDITOR
			return (OVRManager.capiHmd.GetTrackingState().StatusFlags & (uint)StatusBits.PositionConnected) != 0;
#else
			return false;
#endif
		}
	}

	/// <summary>
	/// If true, the tracker can see and track the HMD. Otherwise the HMD may be occluded or the system may be malfunctioning.
	/// </summary>
	public bool isPositionTracked
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			return (OVRManager.capiHmd.GetTrackingState().StatusFlags & (uint)StatusBits.PositionTracked) != 0;
#else
			return false;
#endif
		}
	}

	/// <summary>
	/// If this is true and a tracker is available, the system will use position tracking when isPositionTracked is also true.
	/// </summary>
	public bool isEnabled
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			uint trackingCaps = OVRManager.capiHmd.GetDesc().TrackingCaps;
			return (trackingCaps & (uint)TrackingCaps.Position) != 0;
#else
			return false;
#endif
		}

		set {
#if !UNITY_ANDROID || UNITY_EDITOR
			uint trackingCaps = (uint)TrackingCaps.Orientation | (uint)TrackingCaps.MagYawCorrection;

			if (value)
				trackingCaps |= (uint)TrackingCaps.Position;

			OVRManager.capiHmd.ConfigureTracking(trackingCaps, 0);
#endif
		}
	}

	/// <summary>
	/// Gets the tracker's viewing frustum.
	/// </summary>
	public Frustum frustum
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			HmdDesc desc = OVRManager.capiHmd.GetDesc();

			return new Frustum
			{
				nearZ = desc.CameraFrustumNearZInMeters,
				farZ = desc.CameraFrustumFarZInMeters,
				fov = Mathf.Rad2Deg * new Vector2(desc.CameraFrustumHFovInRadians, desc.CameraFrustumVFovInRadians)
			};
#else
			return new Frustum
			{
				nearZ = 0.1f,
				farZ = 1000.0f,
				fov = new Vector2(90.0f, 90.0f)
			};
#endif
		}
	}

	/// <summary>
	/// Gets the tracker's pose, relative to the head's pose at the time of the last pose recentering.
	/// </summary>
	public OVRPose GetPose(double predictionTime = 0d)
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		double abs_time_plus_pred = Hmd.GetTimeInSeconds() + predictionTime;

		return OVRManager.capiHmd.GetTrackingState(abs_time_plus_pred).CameraPose.ToPose();
#else
		return new OVRPose
		{
			position = Vector3.zero,
			orientation = Quaternion.identity
		};
#endif
	}
}
