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
using System;

/// <summary>
/// Matches the events in the native plugin.
/// </summary>
public enum RenderEventType
{
	// PC
	BeginFrame = 0,
	EndFrame = 1,
	Initialize = 2,
	Destroy = 3,

	// Android
	InitRenderThread = 0,
	Pause = 1,
	Resume = 2,
	LeftEyeEndFrame = 3,
	RightEyeEndFrame = 4,
	TimeWarp = 5,
	PlatformUI = 6,
	PlatformUIConfirmQuit = 7,
}

/// <summary>
/// Communicates with native plugin functions that run on the rendering thread.
/// </summary>
public static class OVRPluginEvent
{
	/// <summary>
	/// Immediately issues the given event.
	/// </summary>
	public static void Issue(RenderEventType eventType)
	{
		GL.IssuePluginEvent(EncodeType((int)eventType));
	}

	/// <summary>
	/// Create a data channel through the single 32-bit integer we are given to
	/// communicate with the plugin.
	/// Split the 32-bit integer event data into two separate "send-two-bytes"
	/// plugin events. Then issue the explicit event that makes use of the data.
	/// </summary>
	public static void IssueWithData(RenderEventType eventType, int eventData)
	{
		// Encode and send-two-bytes of data
		GL.IssuePluginEvent(EncodeData((int)eventType, eventData, 0));

		// Encode and send remaining two-bytes of data
		GL.IssuePluginEvent(EncodeData((int)eventType, eventData, 1));

		// Explicit event that uses the data
		GL.IssuePluginEvent(EncodeType((int)eventType));
	}

	// PRIVATE MEMBERS
	//------------------------------
	// Pack event data into a Uint32:
	// - isDataFlag
	// - bytePos
	// - eventId data is associated with
	// - 2-bytes of event data
	//
	// 31    30   29...25  24...16 15...0
	// [data][pos][eventid][unused][payload]
	//------------------------------
	private const UInt32 IS_DATA_FLAG = 0x80000000;
	private const UInt32 DATA_POS_MASK = 0x40000000;
	private const int DATA_POS_SHIFT = 30;
	private const UInt32 EVENT_TYPE_MASK = 0x3E000000;
	private const int EVENT_TYPE_SHIFT = 25;
	private const UInt32 PAYLOAD_MASK = 0x0000FFFF;
	private const int PAYLOAD_SHIFT = 16;

	private static int EncodeType(int eventType)
	{
		return (int)((UInt32)eventType & ~IS_DATA_FLAG); // make sure upper bit is not set
	}

	private static int EncodeData(int eventId, int eventData, int pos)
	{
		UInt32 data = 0;
		data |= IS_DATA_FLAG;
		data |= (((UInt32)pos << DATA_POS_SHIFT) & DATA_POS_MASK);
		data |= (((UInt32)eventId << EVENT_TYPE_SHIFT) & EVENT_TYPE_MASK);
		data |= (((UInt32)eventData >> (pos * PAYLOAD_SHIFT)) & PAYLOAD_MASK);

		return (int)data;
	}

	private static int DecodeData(int eventData)
	{
//		bool hasData   = (((UInt32)eventData & IS_DATA_FLAG) != 0);
		UInt32 pos     = (((UInt32)eventData & DATA_POS_MASK) >> DATA_POS_SHIFT);
//		UInt32 eventId = (((UInt32)eventData & EVENT_TYPE_MASK) >> EVENT_TYPE_SHIFT);
		UInt32 payload = (((UInt32)eventData & PAYLOAD_MASK) << (PAYLOAD_SHIFT * (int)pos));

		return (int)payload;
	}
}
