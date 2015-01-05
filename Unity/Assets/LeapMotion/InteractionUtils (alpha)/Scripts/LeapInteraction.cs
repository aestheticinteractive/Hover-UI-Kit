using UnityEngine;
using System.Collections;

namespace Leap.Interact
{
  [RequireComponent(typeof(Rigidbody))]
  [RequireComponent(typeof(Collider))]
  public class LeapInteraction : MonoBehaviour {

    public enum HandlingModeEnum {
      Fixed,
      RotateWithOtherHand, 
      Rotate,
      RotateAndScale,
      Averaged
    }

    public HandlingModeEnum HandlingMode = HandlingModeEnum.Rotate;

    [HideInInspector]
    public bool LockRotation = true;
    
    // Use predefined holding orientation.
    public bool PredefinedRotation = true;

    // Maximum distance for magnetic grabbing.
    public float MagneticDistance = 50.0f; // default in millimeters ?  // units ?  // tested

    // Determines how strongly the object is pulled towards the hodling hand. Any value.
    public float StrengthFactor = 1.0f;

    // Leap-space distance for starting a pinch.
    public float MinPinchDistnace = 70.0f;
    
    // Leap-space distance for maximum strength on a pinch.
    public float FullPinchDistance = 40.0f;


    [HideInInspector]
    public bool EnableCollisionShapeHolding = false;
    [HideInInspector]
    public bool EnableAnchorBasedHolding = true;

    // Use palm's positon to position the held body.
    [HideInInspector]
    public bool PositionFromPalm = false;
    // Use palm's rotation to orient the held body.
    [HideInInspector]
    public bool RotationFromPalm = true;


    // Enable rotation while holding with two hands.
    [HideInInspector]
    public bool EnableAnchorRotation = false;
    // Enable scaling while holding with two hands.
    [HideInInspector]
    public bool EnableScaling = false;
    // Enable reorientation on hand collision while holdling.
    [HideInInspector]
    public bool EnableHandCollision = false;
    // Average transforms on multi-hand holding
    [HideInInspector]
    public bool AverageTransforms = false;

    // Enable extra rotation from relative movement of holding fingers.
    public bool RotateWithFingers = false;

    // Not applicable to averaged holding.
    // Set newer holdings as primary holidings (where the held body is attached).
    public bool ToggleMainHolding = false;

    [HideInInspector]
    public bool EnableClicking = true; // not exposed

    [HideInInspector]
    public bool UseVelocity = false; // 

    [HideInInspector]
    public bool GenerateDefaultClickAnchors = false;

    // Always enable smooth grabbing
    [HideInInspector]
    public bool EnableSmoothGrabbing = true;


    // Rotational constraint is only going to be enforced at this strength (0-1 scale).
    [HideInInspector]
    public float MinStrengthToLockRotation = 0.0f;

    [HideInInspector]
    public bool CollisionsWithHandFilteredOut = false;

    public Body.HandAnchorType HandAnchorType = Body.HandAnchorType.DefaultHoldingAnchor;

    public bool GenerateAnchors = false;

    
    // Temp per-body values used by the UnityUtil or Scene scripts.
    [HideInInspector]
    public Vector3 tmpVelocity = Vector3.zero;
    [HideInInspector]
    public Vector3 tmpAngularVelocity = Vector3.zero;
    [HideInInspector]
    public bool velocityToTransfer = false;
    [HideInInspector]
    public float scale = 1.0f;


    private bool m_started = false;

    public void ApplyToBody(Body body)
    {
      switch(HandlingMode)
      {
      case HandlingModeEnum.Fixed:
        EnableAnchorRotation = false;
        EnableScaling = false;
        EnableHandCollision = false;
        AverageTransforms = false;
        break;
      case HandlingModeEnum.RotateWithOtherHand:
        EnableAnchorRotation = false;
        EnableScaling = false;
        EnableHandCollision = true;
        AverageTransforms = false;
        break;
      case HandlingModeEnum.Rotate:
        EnableAnchorRotation = true;
        EnableScaling = false;
        EnableHandCollision = true;
        AverageTransforms = false;
        break;
      case HandlingModeEnum.RotateAndScale:
        EnableAnchorRotation = true;
        EnableScaling = true;
        EnableHandCollision = true;
        AverageTransforms = false;
        break;
      case HandlingModeEnum.Averaged:
        EnableAnchorRotation = false;
        EnableScaling = false;
        EnableHandCollision = true;
        AverageTransforms = true;
        break;
      }

      MinStrengthToLockRotation = LockRotation ? 0.0f : 1.0f;

      body.MotionType = Body.MotionTypeEnum.Dynamic;

      body.EnableCollisionShapeHolding = EnableCollisionShapeHolding;
      body.EnableAnchorBasedHolding = EnableAnchorBasedHolding;
      
      body.UsePalmPositionForHoldings = PositionFromPalm;
      body.UsePalmRotationForHoldings = RotationFromPalm;
      
      body.EnableReorientationOnMultiHolding = EnableAnchorRotation;
      body.EnableScalingOnMultiHolding = EnableScaling;
      body.EnableHandCollisionWhileHolding = EnableHandCollision;
      body.EnableRotationWithFingersWhileHolding = RotateWithFingers;
      body.OvertakeControlWithNewerHoldings = ToggleMainHolding;
      
      body.EnableAveragingTransformsOnMultiHolding = AverageTransforms;
      
      body.MaxMagneticGrabDistance = MagneticDistance;

      body.EnableClicking = EnableClicking;

      if (GenerateAnchors) { body.GenerateDefaultAnchors(); }
      if (GenerateDefaultClickAnchors) { body.GenerateDefaultClickAnchors(); }
      if (EnableSmoothGrabbing) { body.EnableSmoothGrabbing(); }

      body.UseCurrentRelativeRotationWhenLockingRotation = !PredefinedRotation;
      body.LockRotationAboveThisStrength = MinStrengthToLockRotation;

      body.PinchDistanceThresholdForMinStrength = MinPinchDistnace;
      body.PinchDistanceThresholdForMaxStrength = FullPinchDistance;

      body.SetPalmAnchor(HandAnchorType);

      body.SetMagneticStrength(StrengthFactor);
    }

    void Start () {
      InteractionSceneSetup.EnsureInstanceInitialized();
      AddRemoveBodyUtil.Instance.AddBodyToLeapFromUnity(rigidbody);
      m_started = true;
    }
    
    void Update () {
    }

    void OnEnable() {
    }
    
    void OnDisable() {
      if (m_started)
        AddRemoveBodyUtil.Instance.RemoveBodyFromLeap(rigidbody);
    }
  }
}
