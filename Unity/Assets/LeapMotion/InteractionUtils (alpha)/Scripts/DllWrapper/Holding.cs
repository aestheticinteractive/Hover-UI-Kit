using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leap.Interact
{
  public struct Holding
  {

    internal Holding(Native.HoldingId holdingId) { HoldingId = holdingId; }
    internal Holding(IntPtr ptr) { HoldingId.ptr = ptr; }


    public Body Body
    {
      get {
        IntPtr ptr = Native.AccessPropertyAsPtr(this, Native.Property.HoldingBody, Native.Mode.Get, new IntPtr(0)); 
        return (ptr != IntPtr.Zero) ? new Body(ptr) : null;
      }
    }

    public float Strength
    {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.HoldingStrength, Native.Mode.Get, 0.0f ); }
      set { Native.AccessPropertyAsFloat(this, Native.Property.HoldingStrength, Native.Mode.Set, value ); }
    }

    public LeapTransform Transform {
      get { return new LeapTransform(Native.AccessPropertyAsTransform(this, Native.Property.HoldingTransform, Native.Mode.Get, LeapTransform.Identity.ToNative())); }
      //set { Native.AccessPropertyAsTransform(this, Native.Property.HoldingTransform, Native.Mode.Set, value.ToNative()); }
    }

    public LeapTransform BodyTransform {
      get { return new LeapTransform(Native.AccessPropertyAsTransform(this, Native.Property.HoldingBodyTransform, Native.Mode.Get, LeapTransform.Identity.ToNative())); }
      //set { Native.AccessPropertyAsTransform(this, Native.Property.HoldingTransform, Native.Mode.Set, value.ToNative()); }
    }

    //
    // Native reference
    //
    
    internal Native.HoldingId HoldingId;
    public static implicit operator Native.HoldingId(Holding Holding) { return Holding.HoldingId; }
    public static implicit operator IntPtr(Holding Holding) { return Holding.HoldingId.ptr; }
  }
}
