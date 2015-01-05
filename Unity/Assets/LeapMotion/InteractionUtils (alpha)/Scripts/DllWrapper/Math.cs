using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Leap.Interact
{
  public partial struct LeapVector3
  {
    public float x, y, z;

    public LeapVector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    public void Set(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    internal LeapVector3(Native.Vector3 v) { x = v.x; y = v.y; z = v.z; }

    //static public implicit operator Native.Vector3(LeapVector3 v) { return v.ToNative(); }

    internal Native.Vector3 ToNative() {
      Native.Vector3 v = new Native.Vector3();
      v.x = x; v.y = y; v.z = z;
      return v;
    }
    static public LeapVector3 Zero() { return new LeapVector3(0f, 0f, 0f); }
  }

  public partial struct LeapQuaternion
  {
    public float x, y, z, w;

    public LeapQuaternion(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }
    public void Set(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }

    internal LeapQuaternion(Native.Quaternion q) { x = q.x; y = q.y; z = q.z; w = q.w; }

    //static public implicit operator Native.Quaternion(LeapQuaternion q) { return q.ToNative(); }

    internal Native.Quaternion ToNative() {
      Native.Quaternion q = new Native.Quaternion();
      q.x = x; q.y = y; q.z = z; q.w = w;
      return q;
    }

    static public LeapQuaternion Identity() { return new LeapQuaternion(0f, 0f, 0f, 1f); } 
  }

  public partial class LeapTransform
  {
    public LeapVector3 Position;
    public LeapQuaternion Rotation;

    public LeapTransform() { }
    public LeapTransform(LeapVector3 v, LeapQuaternion q) { Position = v; Rotation = q; }
    public void Set(LeapVector3 v, LeapQuaternion q) { Position = v; Rotation = q; }

    internal LeapTransform(Native.Transform t) { 
     Position = new LeapVector3(t.pos);
    Rotation = new LeapQuaternion(t.rot);
  }

    //static public implicit operator Native.Transform(LeapTransform t) { return t.ToNative(); }

    internal Native.Transform ToNative() {
      Native.Transform t = new Native.Transform();
      t.pos = Position.ToNative();
      t.rot = Rotation.ToNative();
      return t;
    }

    static public LeapTransform Identity = new LeapTransform(LeapVector3.Zero(), LeapQuaternion.Identity());
  }
}
