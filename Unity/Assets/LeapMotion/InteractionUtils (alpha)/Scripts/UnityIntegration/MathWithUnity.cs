using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using UnityEngine;

using Leap.Interact;

namespace Leap.Interact
{
  /// <summary>
  /// This part of LeapQuaternion defines left-right handedness conversion with Unity system
  /// </summary>
  public partial struct LeapVector3
  {
    static public implicit operator LeapVector3(UnityEngine.Vector3 v) { return new LeapVector3(v.x, v.y, -v.z); }
    static public implicit operator UnityEngine.Vector3(LeapVector3 v) { return new UnityEngine.Vector3(v.x, v.y, -v.z); }
  }

  /// <summary>
  /// This part of LeapQuaternion defines left-right handedness conversion with Unity system
  /// </summary>
  public partial struct LeapQuaternion
  {
    static public implicit operator LeapQuaternion(UnityEngine.Quaternion q) { return new LeapQuaternion(q.x, q.y, -q.z, -q.w); }
    static public implicit operator UnityEngine.Quaternion(LeapQuaternion q) { return new UnityEngine.Quaternion(q.x, q.y, -q.z, -q.w); }
  }

  /// <summary>
  /// This part of LeapQuaternion defines left-right handedness conversion with Unity system
  /// </summary>
  public partial class LeapTransform
  {
    static public implicit operator LeapTransform(UnityEngine.Transform t) { return new LeapTransform(t.position, t.rotation); }
    //static public UnityEngine.Transform operator << (UnityEngine.Transform obj, LeapTransform t) { obj.position = t.Position; obj.rotation = t.Rotation; return obj; }
  }
}
