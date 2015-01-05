/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// A deforming, very low poly count hand.
public class PolyHand : HandModel {

  public Transform palm;

  public override void InitHand() {
    SetPalmOrientation();

    for (int f = 0; f < fingers.Length; ++f) {
      if (fingers[f] != null)
        fingers[f].InitFinger();
    }
  }

  public override void UpdateHand() {
    SetPalmOrientation();

    for (int f = 0; f < fingers.Length; ++f) {
      if (fingers[f] != null)
        fingers[f].UpdateFinger();
    }
  }

  protected void SetPalmOrientation() {
    if (palm != null) {
      palm.position = GetPalmPosition();
      palm.rotation = GetPalmRotation();
    }
  }
}
