using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Leap.Interact
{
  /// <summary>
  /// Represents a Rigid Body.
  /// </summary>
  public class Body
  {
    public enum MotionTypeEnum
    {
      Invalid = 0,
      Static = 1,
      Keyframed = 2,
      Hand = 4,
      Kinematic = 8,
      Dynamic = 16,
    }

    public enum HandAnchorType
    {
      DefaultHoldingAnchor,
      TabletHoldingAnchor,
      BlockHoldingAnchor
    }

    public Body() 
    {
      BodyId = Native.CreateBody();
      //Native.SetBodyMassAndInertia(this, 1f);
    }

    public Body(Native.BodyId bodyId)
    {
      BodyId = bodyId;
    }

    internal Body(IntPtr ptr)
    {
      BodyId.ptr = ptr;
    }
    
    public Body(Shape shape) { Shape = shape;  }

    public bool IsValid() { return BodyId.ptr != IntPtr.Zero; }

    //
    // Body properties
    //

    public float Mass {
      get { return Native.GetBodyMass(this);  }
      set { Native.SetBodyMassAndInertia(this, value); }
    }

    public LeapTransform Transform {
      get { return new LeapTransform(Native.AccessPropertyAsTransform(this, Native.Property.BodyTransform, Native.Mode.Get, LeapTransform.Identity.ToNative())); }
      set { Native.AccessPropertyAsTransform(this, Native.Property.BodyTransform, Native.Mode.Set, value.ToNative()); }
    }

    public MotionTypeEnum MotionType {
      get { return (MotionTypeEnum)Native.AccessPropertyAsInt(this, Native.Property.BodyMotionType, Native.Mode.Get, (int)MotionTypeEnum.Invalid); }
      set { Native.AccessPropertyAsInt(this, Native.Property.BodyMotionType, Native.Mode.Set, (int)value); }
    }

    public bool EnableCollisionShapeHolding {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableCollisionShapeHolding, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableCollisionShapeHolding, Native.Mode.Set, value); }
    }

    public bool EnableAnchorBasedHolding {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableAnchorBasedHolding, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableAnchorBasedHolding, Native.Mode.Set, value); }
    }

    public bool UsePalmPositionForHoldings {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyUsePalmPositionForHoldings, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyUsePalmPositionForHoldings, Native.Mode.Set, value); }
    }

    public bool UsePalmRotationForHoldings {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyUsePalmRotationForHoldings, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyUsePalmRotationForHoldings, Native.Mode.Set, value); }
    }

    public bool EnableReorientationOnMultiHolding {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableReorientationOnMultiHolding, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableReorientationOnMultiHolding, Native.Mode.Set, value); }
    }

    public bool EnableScalingOnMultiHolding {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableScalingOnMultiHolding, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableScalingOnMultiHolding, Native.Mode.Set, value); }
    }

    public bool EnableHandCollisionWhileHolding {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableHandCollisionWhileHolding, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableHandCollisionWhileHolding, Native.Mode.Set, value); }
    }

    public bool EnableRotationWithFingersWhileHolding {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableRotationWithFingersWhileHolding, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableRotationWithFingersWhileHolding, Native.Mode.Set, value); }
    }

    public bool OvertakeControlWithNewerHoldings {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyOvertakeControlWithNewerHoldings, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyOvertakeControlWithNewerHoldings, Native.Mode.Set, value); }
    }

    public bool EnableAveragingTransformsOnMultiHolding {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableAveragingTransformsOnMultiHolding, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableAveragingTransformsOnMultiHolding, Native.Mode.Set, value); }
    }

    public bool EnableClicking {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyEnableClicking, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableClicking, Native.Mode.Set, value); }
    }

    public float MaxMagneticGrabDistance {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.BodyMaxMagneticGrabDistance, Native.Mode.Get, 0.0f); }
      set { Native.AccessPropertyAsFloat(this, Native.Property.BodyMaxMagneticGrabDistance, Native.Mode.Set, value); }
    }
//
    public LeapTransform DefaultPalmAnchorLeft {
      get { return new LeapTransform(Native.AccessPropertyAsTransform(this, Native.Property.BodyMaxMagneticGrabDistance, Native.Mode.Get, LeapTransform.Identity.ToNative())); }
      set { Native.AccessPropertyAsTransform(this, Native.Property.BodyDefaultPalmAnchorLeft, Native.Mode.Set, value.ToNative()); }
    }

    public LeapTransform DefaultPalmAnchorRight {
      get { return new LeapTransform(Native.AccessPropertyAsTransform(this, Native.Property.BodyMaxMagneticGrabDistance, Native.Mode.Get, LeapTransform.Identity.ToNative()));}
      set { Native.AccessPropertyAsTransform(this, Native.Property.BodyDefaultPalmAnchorRight, Native.Mode.Set, value.ToNative()); }
    }

    public float LockRotationAboveThisStrength {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.BodyLockRotationAboveThisStrength, Native.Mode.Get, 0.0f); }
      set { Native.AccessPropertyAsFloat(this, Native.Property.BodyLockRotationAboveThisStrength, Native.Mode.Set, value); }
    }

    public bool UseCurrentRelativeRotationWhenLockingRotation {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyUseCurrentRelativeRotationWhenLockingRotation, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.BodyUseCurrentRelativeRotationWhenLockingRotation, Native.Mode.Set, value); }
    }

    public float PinchDistanceThresholdForMinStrength {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.BodyPinchDistanceThresholdForMinStrength, Native.Mode.Get, 0.0f); }
      set { Native.AccessPropertyAsFloat(this, Native.Property.BodyPinchDistanceThresholdForMinStrength, Native.Mode.Set, value); }
    }

    public float PinchDistanceThresholdForMaxStrength {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.BodyPinchDistanceThresholdForMaxStrength, Native.Mode.Get, 0.0f); }
      set { Native.AccessPropertyAsFloat(this, Native.Property.BodyPinchDistanceThresholdForMaxStrength, Native.Mode.Set, value); }
    }

    public void SetPalmAnchor(HandAnchorType type) { Native.AccessPropertyAsInt(this, Native.Property.BodyHandAnchorType, Native.Mode.Set, (int)type); }

    public void GenerateDefaultAnchors() { Native.AccessPropertyAsBool(this, Native.Property.BodyGenerateDefaultAnchors, Native.Mode.Call, true); }

    public void GenerateDefaultClickAnchors() { Native.AccessPropertyAsBool(this, Native.Property.BodyGenerateDefaultClickAnchors, Native.Mode.Call, true); }

    public void EnableSmoothGrabbing()  { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableSmoothGrabbing, Native.Mode.Call, true); }

    public float Scale {
      get { return Native.AccessPropertyAsFloat(this, Native.Property.BodyScale, Native.Mode.Get, 0.0f); }
    }

    public void SetMagneticStrength(float strength) { Native.AccessPropertyAsFloat(this, Native.Property.BodyMagneticStrength, Native.Mode.Set, strength); }

    //
    // Body state
    //

    public bool IsHeld {
      get { return Native.AccessPropertyAsBool(this, Native.Property.BodyIsHeld, Native.Mode.Get, false); }
      //set { Native.AccessPropertyAsBool(this, Native.Property.BodyEnableScalingOnMultiHolding, Native.Mode.Set, value); }
    }

    /// <summary>
    /// The shape assigned to the body. This may be shared between multiple bodies.
    /// </summary>
    public Shape Shape
    {
      get { return new Shape(Native.GetBodyShape(this)); }
      set { Native.SetBodyShape(this, value); }
    }

    //
    // Native reference
    //

    public void Invalidate() { BodyId.ptr = IntPtr.Zero; }

    public Native.BodyId BodyId;
    public static implicit operator Native.BodyId(Body body) { return body.BodyId; }
    public static implicit operator IntPtr(Body body) { return body.BodyId.ptr; }
  }
}
