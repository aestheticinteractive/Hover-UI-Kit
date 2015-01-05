using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Leap.Interact
{
  public struct Shape {

    public enum ShapeType
    {
      Invalid,
      Sphere,
      Capsule,
      Box,
      Plane
    }

    //public Shape() { }

    internal Shape(Native.ShapeId shapeId) { ShapeId = shapeId; }

    public enum CapsuleOrientation
    {
      AlongXAxis,
      AlongYAxis,
      AlongZAxis
    }


    public static Shape CreateSphere(float radius) {
      return new Shape(Native.CreateSphereShape(radius));
    }

    public static Shape CreateCapsule(CapsuleOrientation capsuleOrientation, float segmentHalfLength, float radius) {
      return new Shape(Native.CreateCapsuleShape((Native.CapsuleOrientation)capsuleOrientation, segmentHalfLength, radius));
    }

    public static Shape CreateBox(LeapVector3 halfSize, float radius) {
      return new Shape(Native.CreateBoxShape(halfSize.ToNative(), radius));
    }

    public void ReleaseShape() {
      Native.ReleaseShape(this);
    }

    //
    // Shape properties
    //

    public void AddAnchor(LeapTransform t) { Native.AccessPropertyAsTransform(this, Native.Property.ShapeAnchors, Native.Mode.Add, t.ToNative()); }
    public void ClearAnchors() { Native.AccessPropertyAsTransform(this, Native.Property.ShapeAnchors, Native.Mode.Clear, LeapTransform.Identity.ToNative()); }

    public void AddClickAnchor(LeapTransform t) { Native.AccessPropertyAsTransform(this, Native.Property.ShapeClickAnchors, Native.Mode.Add, t.ToNative()); }
    public void ClearClickAnchors() { Native.AccessPropertyAsTransform(this, Native.Property.ShapeClickAnchors, Native.Mode.Clear, LeapTransform.Identity.ToNative()); }



    //
    // Get shape properties for the purpose of rebuilding the shape in Unity
    //

    public ShapeType Type {
      get { return (ShapeType)Native.AccessPropertyAsInt(this, Native.Property.ShapeType, Native.Mode.Get, (int)ShapeType.Invalid); }
      //set { Native.AccessPropertyAsBool(this, Native.Property.ShapeType, Native.Mode.Set, value); }
    }

    public float CapsuleSegmentLength {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.ShapeCapsuleSegmentLength, Native.Mode.Get, 0f); }
      //set { Native.AccessPropertyAsBool(this, Native.Property.ShapeType, Native.Mode.Set, value); }
    }

    public float CapsuleRadius {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.ShapeCapsuleRadius, Native.Mode.Get, 0f); }
      //set { Native.AccessPropertyAsBool(this, Native.Property.ShapeType, Native.Mode.Set, value); }
    }

    //
    // Native reference
    //

    internal Native.ShapeId ShapeId;
    public static implicit operator Native.ShapeId(Shape shape) { return shape.ShapeId; }
    public static implicit operator IntPtr(Shape shape) { return shape.ShapeId.ptr; }
  }
}
