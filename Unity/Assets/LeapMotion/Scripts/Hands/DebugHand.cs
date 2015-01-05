/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// A debugging hand that draws debug lines connecting important positions.
public class DebugHand : HandModel {

  public override void InitHand() {
    for (int f = 0; f < fingers.Length; ++f) {
      if (fingers[f] != null)
        fingers[f].InitFinger();
    }

    DrawDebugLines();
  }

  public override void UpdateHand() {
    for (int f = 0; f < fingers.Length; ++f) {
      if (fingers[f] != null)
        fingers[f].UpdateFinger();
    }

    DrawDebugLines();
  }

  protected void DrawDebugLines() {
    HandModel hand = GetComponent<HandModel>();
    Debug.DrawLine(hand.GetElbowPosition(), hand.GetWristPosition(), Color.red);
    Debug.DrawLine(hand.GetWristPosition(), hand.GetPalmPosition(), Color.white);
    Debug.DrawLine(hand.GetPalmPosition(),
                   hand.GetPalmPosition() + hand.GetPalmNormal(), Color.black);
    Debug.Log(Vector3.Dot(hand.GetPalmDirection(), hand.GetPalmNormal()));
  }
}
