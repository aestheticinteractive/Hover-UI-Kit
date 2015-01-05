/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

public class DisconnectionNotice : MonoBehaviour {

  public float fadeInTime = 1.0f;
  public float fadeOutTime = 1.0f;
  public AnimationCurve fade;
  public int waitFrames = 10;
  public Texture2D embeddedReplacementImage;
  public Color onColor = Color.white;

  private Controller leap_controller_;
  private float fadedIn = 0.0f;
  private int frames_disconnected_ = 0;

  void Start() {
    leap_controller_ = new Controller();
    SetAlpha(0.0f);
  }

  void SetAlpha(float alpha) {
    guiTexture.color = Color.Lerp(Color.clear, onColor, alpha);
  }

  public bool IsConnected() {
    return leap_controller_.IsConnected;
  }

  public bool IsEmbedded() {
    DeviceList devices = leap_controller_.Devices;
    if (devices.Count == 0)
      return false;
    return devices[0].IsEmbedded;
  }

  void Update() {
    if (embeddedReplacementImage != null && IsEmbedded()) {
      guiTexture.texture = embeddedReplacementImage;
    }

    if (IsConnected())
      frames_disconnected_ = 0;
    else
      frames_disconnected_++;

    if (frames_disconnected_ < waitFrames)
      fadedIn -= Time.deltaTime / fadeOutTime;
    else
      fadedIn += Time.deltaTime / fadeInTime;
    fadedIn = Mathf.Clamp(fadedIn, 0.0f, 1.0f);

    SetAlpha(fade.Evaluate(fadedIn));
  }
}
