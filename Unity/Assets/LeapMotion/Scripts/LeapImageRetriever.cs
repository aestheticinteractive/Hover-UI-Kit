/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Leap;

public struct LMDevice
{
  public static int PERIPERAL_WIDTH = 640;
  public static int PERIPERAL_HEIGHT = 240;
  public static int DRAGONFLY_WIDTH = 608;
  public static int DRAGONFLY_HEIGHT = 540;
  public static int MANTIS_WIDTH = 640;
  public static int MANTIS_HEIGHT = 240;

  public int width;
  public int height;
  public int pixels;
  public bool isRobustMode;
  public LM_DEVICE type;

  public LMDevice (LM_DEVICE device = LM_DEVICE.INVALID)
  {
    type = device;
    switch (type)
    {
      case LM_DEVICE.PERIPHERAL:
        width = PERIPERAL_WIDTH;
        height = PERIPERAL_HEIGHT;
        break;
      case LM_DEVICE.DRAGONFLY:
        width = DRAGONFLY_WIDTH;
        height = DRAGONFLY_HEIGHT;
        break;
      case LM_DEVICE.MANTIS:
        width = MANTIS_WIDTH;
        height = MANTIS_HEIGHT;
        break;
      default:
        width = 0;
        height = 0;
        break;
    }
    this.pixels = width * height;
    isRobustMode = false;
  }

  public void UpdateRobustMode(int height)
  {
    switch (type)
    {
      case LM_DEVICE.PERIPHERAL:
        isRobustMode = (height < PERIPERAL_HEIGHT) ? true : false;
        break;
      case LM_DEVICE.DRAGONFLY:
        isRobustMode = (height < DRAGONFLY_HEIGHT) ? true : false;
        break;
      case LM_DEVICE.MANTIS:
        isRobustMode = (height < MANTIS_HEIGHT) ? true : false;
        break;
      default:
        isRobustMode = false;
        break;
    }
  }
}

public enum LM_DEVICE
{
  INVALID = -1,
  PERIPHERAL = 0,
  DRAGONFLY = 1,
  MANTIS = 2
}

// To use the LeapImageRetriever you must be on version 2.1+
// and enable "Allow Images" in the Leap Motion settings.
public class LeapImageRetriever : MonoBehaviour
{
  private Shader IR_NORMAL_SHADER;
  private Shader IR_UNDISTORT_SHADER;
  private Shader IR_UNDISTORT_SHADER_FOREGROUND;
  private Shader RGB_NORMAL_SHADER;
  private Shader RGB_UNDISTORT_SHADER;

  public bool doUpdate = true;
  public bool rescaleController = true;

  public const int DEFAULT_DISTORTION_WIDTH = 64;
  public const int DEFAULT_DISTORTION_HEIGHT = 64;
  public const int IMAGE_WARNING_WAIT = 10;

  public int imageIndex = 0;
  public Color imageColor = Color.white;
  public float gammaCorrection = 1.0f;
  public bool overlayImage = false;
  public bool undistortImage = true;
  public bool blackIsTransparent = true;

  private HandController controller_ = null;
  private LMDevice attached_device_ = new LMDevice();

  // Main texture.
  protected Texture2D main_texture_;
  protected Color32[] image_pixels_;
  protected int image_misses_ = 0;

  // Distortion textures.
  protected Texture2D distortionX_;
  protected Texture2D distortionY_;
  protected Color32[] dist_pixelsX_;
  protected Color32[] dist_pixelsY_;

  private LM_DEVICE GetDevice(int width)
  {
    const bool OVERRIDE_MANTIS = false;
    if (OVERRIDE_MANTIS)
    {
      return LM_DEVICE.MANTIS;
    }

    if (width == LMDevice.PERIPERAL_WIDTH)
    {
      return LM_DEVICE.PERIPHERAL;
    }
    else if (width == LMDevice.DRAGONFLY_WIDTH)
    {
      return LM_DEVICE.DRAGONFLY;
    }
    else if (width == LMDevice.MANTIS_WIDTH)
    {
      return LM_DEVICE.MANTIS;
    }
    return LM_DEVICE.INVALID;
  }

  protected void SetShader()
  {
    DestroyImmediate(renderer.material);
    switch (attached_device_.type)
    {
      case LM_DEVICE.PERIPHERAL:
        renderer.material = (undistortImage) ? new Material((overlayImage) ? IR_UNDISTORT_SHADER_FOREGROUND : IR_UNDISTORT_SHADER) : new Material(IR_NORMAL_SHADER);
        if ( rescaleController ) { controller_.transform.localScale = Vector3.one * 1.6f; }
        break;
      case LM_DEVICE.DRAGONFLY:
        renderer.material = (undistortImage) ? new Material(RGB_UNDISTORT_SHADER) : new Material(RGB_NORMAL_SHADER);
        if ( rescaleController ) { controller_.transform.localScale = Vector3.one; }
        break;
      case LM_DEVICE.MANTIS:
        renderer.material = (undistortImage) ? new Material((overlayImage) ? IR_UNDISTORT_SHADER_FOREGROUND : IR_UNDISTORT_SHADER) : new Material(IR_NORMAL_SHADER);
        if ( rescaleController ) { controller_.transform.localScale = Vector3.one; }
        break;
      default:
        break;
    }
    main_texture_.wrapMode = TextureWrapMode.Clamp;
    image_pixels_ = new Color32[attached_device_.pixels];
  }

  protected void SetRenderer(ref Image image)
  {
    renderer.material.mainTexture = main_texture_;
    renderer.material.SetColor("_Color", imageColor);
    renderer.material.SetInt("_DeviceType", Convert.ToInt32(attached_device_.type));
    renderer.material.SetFloat("_GammaCorrection", gammaCorrection);
    renderer.material.SetInt("_BlackIsTransparent", blackIsTransparent ? 1 : 0);

    renderer.material.SetTexture("_DistortX", distortionX_);
    renderer.material.SetTexture("_DistortY", distortionY_);
    renderer.material.SetFloat("_RayOffsetX", image.RayOffsetX);
    renderer.material.SetFloat("_RayOffsetY", image.RayOffsetY);
    renderer.material.SetFloat("_RayScaleX", image.RayScaleX);
    renderer.material.SetFloat("_RayScaleY", image.RayScaleY);
  }

  protected void InitiateShaders() 
  {
    IR_NORMAL_SHADER = Resources.Load<Shader>("LeapIRDistorted");
    IR_UNDISTORT_SHADER = Resources.Load<Shader>("LeapIRUndistorted");
    IR_UNDISTORT_SHADER_FOREGROUND = Resources.Load<Shader>("LeapIRUndistorted_Foreground");
    RGB_NORMAL_SHADER = Resources.Load<Shader>("LeapRGBDistorted");
    RGB_UNDISTORT_SHADER = Resources.Load<Shader>("LeapRGBUndistorted");
  }

  protected bool InitiateTexture(ref Image image)
  {
    int width = image.Width;
    int height = image.Height;

    attached_device_ = new LMDevice(GetDevice(width));
    attached_device_.UpdateRobustMode(height);
    if (attached_device_.width == 0 || attached_device_.height == 0)
    {
      attached_device_ = new LMDevice();
      Debug.LogWarning("No data in the image texture.");
      return false;
    }
    else
    {
      switch (attached_device_.type)
      {
        case LM_DEVICE.PERIPHERAL:
          main_texture_ = new Texture2D(attached_device_.width, attached_device_.height, TextureFormat.Alpha8, false);
          break;
        case LM_DEVICE.DRAGONFLY:
          main_texture_ = new Texture2D(attached_device_.width, attached_device_.height, TextureFormat.RGBA32, false);
          break;
        case LM_DEVICE.MANTIS:
          main_texture_ = new Texture2D(attached_device_.width, attached_device_.height, TextureFormat.Alpha8, false);
          break;
        default:
          main_texture_ = new Texture2D(attached_device_.width, attached_device_.height, TextureFormat.Alpha8, false);
          break;
      }
      main_texture_.wrapMode = TextureWrapMode.Clamp;
      image_pixels_ = new Color32[attached_device_.pixels];
    }
    return true;
  }

  protected bool InitiateDistortion(ref Image image)
  {
    int width = image.DistortionWidth / 2;
    int height = image.DistortionHeight;

    if (width == 0 || height == 0)
    {
      Debug.LogWarning("No data in image distortion");
      return false;
    }
    else
    {
      dist_pixelsX_ = new Color32[width * height];
      dist_pixelsY_ = new Color32[width * height];
      DestroyImmediate(distortionX_);
      DestroyImmediate(distortionY_);
      distortionX_ = new Texture2D(width, height, TextureFormat.RGBA32, false);
      distortionY_ = new Texture2D(width, height, TextureFormat.RGBA32, false);
      distortionX_.wrapMode = TextureWrapMode.Clamp;
      distortionY_.wrapMode = TextureWrapMode.Clamp;
    }

    return true;
  }

  protected bool InitiatePassthrough(ref Image image)
  {
    if (!InitiateTexture(ref image))
      return false;

    if (!InitiateDistortion(ref image))
      return false;

    SetShader();
    SetRenderer(ref image);

    return true;
  }

  protected void LoadMainTexture(ref Image image)
  {
    byte[] image_data = image.Data;
    switch (attached_device_.type)
    {
      case LM_DEVICE.PERIPHERAL:
      case LM_DEVICE.MANTIS:
        if (attached_device_.isRobustMode) 
        {
          int width = attached_device_.width;
          int height = attached_device_.height;
          int data_index = 0;
          for (int j = 0; j < height; j += 2)
          {
            for (int i = 0; i < width; ++i)  
            {
              image_pixels_[i + (j + 0) * width].a = image_data[data_index];
              image_pixels_[i + (j + 1) * width].a = image_data[data_index];
              data_index++;
            }
          }
        }
        else
        {
          for (int i = 0; i < image_data.Length; ++i)
          {
            image_pixels_[i].a = image_data[i];
          }
        }
        break;
      case LM_DEVICE.DRAGONFLY:
        int image_index = 0;
        for (int i = 0; i < image_data.Length; image_index++)
        {
          image_pixels_[image_index].r = image_data[i++];
          image_pixels_[image_index].g = image_data[i++];
          image_pixels_[image_index].b = image_data[i++];
          image_pixels_[image_index].a = image_data[i++];
        }
        gammaCorrection = Mathf.Max(gammaCorrection, 1.7f);
        break;
      default:
        for (int i = 0; i < image_data.Length; ++i)
          image_pixels_[i].a = image_data[i];
        break;
    }

    main_texture_.SetPixels32(image_pixels_);
    main_texture_.Apply();
  }

  protected bool LoadDistortion(ref Image image)
  {
    if (image.DistortionWidth == 0 || image.DistortionHeight == 0)
    {
      Debug.LogWarning("No data in the distortion texture.");
      return false;
    }

    if (undistortImage)
    {
      float[] distortion_data = image.Distortion;
      int num_distortion_floats = 2 * distortionX_.width * distortionX_.height;

      // Move distortion data to distortion x textures.
      for (int i = 0; i < num_distortion_floats; i += 2)
      {
        // The distortion range is -0.6 to +1.7. Normalize to range [0..1).
        float dval = (distortion_data[i] + 0.6f) / 2.3f;
        float enc_x = dval;
        float enc_y = dval * 255.0f;
        float enc_z = dval * 65025.0f;
        float enc_w = dval * 160581375.0f;

        enc_x = enc_x - (int)enc_x;
        enc_y = enc_y - (int)enc_y;
        enc_z = enc_z - (int)enc_z;
        enc_w = enc_w - (int)enc_w;

        enc_x -= 1.0f / 255.0f * enc_y;
        enc_y -= 1.0f / 255.0f * enc_z;
        enc_z -= 1.0f / 255.0f * enc_w;

        int index = i >> 1;
        dist_pixelsX_[index].r = (byte)(256 * enc_x);
        dist_pixelsX_[index].g = (byte)(256 * enc_y);
        dist_pixelsX_[index].b = (byte)(256 * enc_z);
        dist_pixelsX_[index].a = (byte)(256 * enc_w);
      }
      distortionX_.SetPixels32(dist_pixelsX_);
      distortionX_.Apply();

      // Move distortion data to distortion y textures.
      for (int i = 1; i < num_distortion_floats; i += 2)
      {
        // The distortion range is -0.6 to +1.7. Normalize to range [0..1).
        float dval = (distortion_data[i] + 0.6f) / 2.3f;
        float enc_x = dval;
        float enc_y = dval * 255.0f;
        float enc_z = dval * 65025.0f;
        float enc_w = dval * 160581375.0f;

        enc_x = enc_x - (int)enc_x;
        enc_y = enc_y - (int)enc_y;
        enc_z = enc_z - (int)enc_z;
        enc_w = enc_w - (int)enc_w;

        enc_x -= 1.0f / 255.0f * enc_y;
        enc_y -= 1.0f / 255.0f * enc_z;
        enc_z -= 1.0f / 255.0f * enc_w;

        int index = i >> 1;
        dist_pixelsY_[index].r = (byte)(256 * enc_x);
        dist_pixelsY_[index].g = (byte)(256 * enc_y);
        dist_pixelsY_[index].b = (byte)(256 * enc_z);
        dist_pixelsY_[index].a = (byte)(256 * enc_w);
      }
      distortionY_.SetPixels32(dist_pixelsY_);
      distortionY_.Apply();
    }

    return true;
  }

  void Start()
  {
    GameObject hand_controller = GameObject.Find("HandController");
    if (hand_controller && hand_controller.GetComponent<HandController>())
      controller_ = hand_controller.GetComponent<HandController>();

    if (controller_ == null)
      return;

    controller_.GetLeapController().SetPolicyFlags(Controller.PolicyFlag.POLICY_IMAGES);
    InitiateShaders();
  }

  void Update()
  {
    if (controller_ == null)
      return;

    if ( doUpdate == false ) { return; }

    Frame frame = controller_.GetFrame();

    if (frame.Images.Count == 0)
    {
      image_misses_++;
      if (image_misses_ == IMAGE_WARNING_WAIT)
      {
        // TODO: Make this visible IN applications
        Debug.LogWarning("Can't find any images. " +
                          "Make sure you enabled 'Allow Images' in the Leap Motion Settings, " +
                          "you are on tracking version 2.1+ and " +
                          "your Leap Motion device is plugged in.");
      }
      return;
    }

    // Check main texture dimensions.
    Image image = frame.Images[imageIndex];

    if (attached_device_.width != image.Width || attached_device_.height != image.Height)
    {
      if (!InitiatePassthrough(ref image)) {
        Debug.Log ("InitiatePassthrough FAILED");
        return;
      }
    }

    LoadMainTexture(ref image);
    LoadDistortion(ref image);
  }

  void OnApplicationFocus(bool focusStatus) {
    bool paused = focusStatus;
    if (focusStatus) {
            // Ensure reinitialization in Update
            attached_device_.width = 0;
            attached_device_.height = 0;
        }
  }
}
