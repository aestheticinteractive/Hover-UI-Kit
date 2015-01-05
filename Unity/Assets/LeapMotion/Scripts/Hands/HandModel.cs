/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// Interface for all hands.
public abstract class HandModel : MonoBehaviour {

  public const int NUM_FINGERS = 5;

  public float handModelPalmWidth = 0.085f;
  public FingerModel[] fingers = new FingerModel[NUM_FINGERS];

  protected Hand hand_;
  protected HandController controller_;
  protected bool mirror_z_axis_ = false;

  public Vector3 GetHandOffset() {
    if (controller_ == null || hand_ == null)
      return Vector3.zero;

    Vector3 additional_movement = controller_.handMovementScale - Vector3.one;
    Vector3 scaled_wrist_position =
        Vector3.Scale(additional_movement, hand_.WristPosition.ToUnityScaled(mirror_z_axis_));

    return controller_.transform.TransformPoint(scaled_wrist_position) -
           controller_.transform.position;
  }

  // Returns the palm position of the hand in relation to the controller.
  public Vector3 GetPalmPosition() {
    return controller_.transform.TransformPoint(hand_.PalmPosition.ToUnityScaled(mirror_z_axis_)) +
           GetHandOffset();
  }

  // Returns the palm rotation of the hand in relation to the controller.
  public Quaternion GetPalmRotation() {
    return GetController().transform.rotation * GetLeapHand().Basis.Rotation(mirror_z_axis_);
  }

  // Returns the palm direction of the hand in relation to the controller.
  public Vector3 GetPalmDirection() {
    return controller_.transform.TransformDirection(hand_.Direction.ToUnity(mirror_z_axis_));
  }

  // Returns the palm normal of the hand in relation to the controller.
  public Vector3 GetPalmNormal() {
    return controller_.transform.TransformDirection(hand_.PalmNormal.ToUnity(mirror_z_axis_));
  }

  // Returns the lower arm direction in relation to the controller.
  public Vector3 GetArmDirection() {
    return controller_.transform.TransformDirection(hand_.Arm.Direction.ToUnity(mirror_z_axis_));
  }

  // Returns the lower arm center in relation to the controller.
  public Vector3 GetArmCenter() {
    Vector leap_center = 0.5f * (hand_.Arm.WristPosition + hand_.Arm.ElbowPosition);
    return controller_.transform.TransformPoint(leap_center.ToUnityScaled(mirror_z_axis_)) +
           GetHandOffset();
  }

  // Returns the lower arm elbow position in relation to the controller.
  public Vector3 GetElbowPosition() {
    Vector3 local_position = hand_.Arm.ElbowPosition.ToUnityScaled(mirror_z_axis_);
    return controller_.transform.TransformPoint(local_position) + GetHandOffset();
  }

  // Returns the lower arm wrist position in relation to the controller.
  public Vector3 GetWristPosition() {
    Vector3 local_position = hand_.Arm.WristPosition.ToUnityScaled(mirror_z_axis_);
    return controller_.transform.TransformPoint(local_position) + GetHandOffset();
  }

  // Returns the rotation quaternion of the arm in relation to the controller.
  public Quaternion GetArmRotation() {
    Quaternion local_rotation = hand_.Arm.Basis.Rotation(mirror_z_axis_);
    return controller_.transform.rotation * local_rotation;
  }

  public Hand GetLeapHand() {
    return hand_;
  }

  public void SetLeapHand(Hand hand) {
    hand_ = hand;
    for (int i = 0; i < fingers.Length; ++i) {
      if (fingers[i] != null) {
        fingers[i].SetLeapHand(hand_);
        fingers[i].SetOffset(GetHandOffset());
      }
    }
  }

  public void MirrorZAxis(bool mirror = true) {
    mirror_z_axis_ = mirror;
    for (int i = 0; i < fingers.Length; ++i) {
      if (fingers[i] != null)
        fingers[i].MirrorZAxis(mirror);
    }
  }

  public bool IsMirrored() {
    return mirror_z_axis_;
  }

  public HandController GetController() {
    return controller_;
  }

  public void SetController(HandController controller) {
    controller_ = controller;
    for (int i = 0; i < fingers.Length; ++i) {
      if (fingers[i] != null)
        fingers[i].SetController(controller_);
    }
  }

  public abstract void InitHand();

  public abstract void UpdateHand();
}
