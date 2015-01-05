/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Leap;

// Overall Controller object that will instantiate hands and tools when they appear.
public class HandController : MonoBehaviour {

  // Reference distance from thumb base to pinky base in mm.
  protected const float GIZMO_SCALE = 5.0f;
  protected const float MM_TO_M = 0.001f;

  public bool separateLeftRight = false;
  public HandModel leftGraphicsModel;
  public HandModel leftPhysicsModel;
  public HandModel rightGraphicsModel;
  public HandModel rightPhysicsModel;

  public ToolModel toolModel;

  public bool isHeadMounted = false;
  public bool mirrorZAxis = false;

  // If hands are in charge of Destroying themselves, make this false.
  public bool destroyHands = true;

  public Vector3 handMovementScale = Vector3.one;

  // Recording parameters.
  public bool enableRecordPlayback = false;
  public TextAsset recordingAsset;
  public float recorderSpeed = 1.0f;
  public bool recorderLoop = true;
  
  protected LeapRecorder recorder_ = new LeapRecorder();
  
  protected Controller leap_controller_;

  protected Dictionary<int, HandModel> hand_graphics_;
  protected Dictionary<int, HandModel> hand_physics_;
  protected Dictionary<int, ToolModel> tools_;

  private bool flag_initialized_ = false;
  private bool show_hands_ = true;
  private long prev_graphics_id_ = 0;
  private long prev_physics_id_ = 0;
  
  void OnDrawGizmos() {
    // Draws the little Leap Motion Controller in the Editor view.
    Gizmos.matrix = Matrix4x4.Scale(GIZMO_SCALE * Vector3.one);
    Gizmos.DrawIcon(transform.position, "leap_motion.png");
  }

  void InitializeFlags()
  {
    // Optimize for top-down tracking if on head mounted display.
    Controller.PolicyFlag policy_flags = leap_controller_.PolicyFlags;
    if (isHeadMounted)
      policy_flags |= Controller.PolicyFlag.POLICY_OPTIMIZE_HMD;
    else
      policy_flags &= ~Controller.PolicyFlag.POLICY_OPTIMIZE_HMD;

    leap_controller_.SetPolicyFlags(policy_flags);
  }

  void Awake() {
    leap_controller_ = new Controller();
  }

  void Start() {
    // Initialize hand lookup tables.
    hand_graphics_ = new Dictionary<int, HandModel>();
    hand_physics_ = new Dictionary<int, HandModel>();

    tools_ = new Dictionary<int, ToolModel>();

    if (leap_controller_ == null) {
      Debug.LogWarning(
          "Cannot connect to controller. Make sure you have Leap Motion v2.0+ installed");
    }

    if (enableRecordPlayback && recordingAsset != null)
      recorder_.Load(recordingAsset);
  }

  public void IgnoreCollisionsWithHands(GameObject to_ignore, bool ignore = true) {
    foreach (HandModel hand in hand_physics_.Values)
      Leap.Utils.IgnoreCollisions(hand.gameObject, to_ignore, ignore);
  }

  protected HandModel CreateHand(HandModel model) {
    HandModel hand_model = Instantiate(model, transform.position, transform.rotation)
                           as HandModel;
    hand_model.gameObject.SetActive(true);
    Leap.Utils.IgnoreCollisions(hand_model.gameObject, gameObject);
    return hand_model;
  }

  protected void DestroyHand(HandModel hand_model) {
    if (destroyHands)
      Destroy(hand_model.gameObject);
    else
      hand_model.SetLeapHand(null);
  }

  protected void UpdateHandModels(Dictionary<int, HandModel> all_hands,
                                  HandList leap_hands,
                                  HandModel left_model, HandModel right_model) {
    List<int> ids_to_check = new List<int>(all_hands.Keys);

    // Go through all the active hands and update them.
    int num_hands = leap_hands.Count;
    for (int h = 0; h < num_hands; ++h) {
      Hand leap_hand = leap_hands[h];
      
      HandModel model = (mirrorZAxis != leap_hand.IsLeft) ? left_model : right_model;

      // If we've mirrored since this hand was updated, destroy it.
      if (all_hands.ContainsKey(leap_hand.Id) &&
          all_hands[leap_hand.Id].IsMirrored() != mirrorZAxis) {
        DestroyHand(all_hands[leap_hand.Id]);
        all_hands.Remove(leap_hand.Id);
      }

      // Only create or update if the hand is enabled.
      if (model != null) {
        ids_to_check.Remove(leap_hand.Id);

        // Create the hand and initialized it if it doesn't exist yet.
        if (!all_hands.ContainsKey(leap_hand.Id)) {
          HandModel new_hand = CreateHand(model);
          new_hand.SetLeapHand(leap_hand);
          new_hand.MirrorZAxis(mirrorZAxis);
          new_hand.SetController(this);

          // Set scaling based on reference hand.
          float hand_scale = MM_TO_M * leap_hand.PalmWidth / new_hand.handModelPalmWidth;
          new_hand.transform.localScale = hand_scale * transform.lossyScale;

          new_hand.InitHand();
          new_hand.UpdateHand();
          all_hands[leap_hand.Id] = new_hand;
        }
        else {
          // Make sure we update the Leap Hand reference.
          HandModel hand_model = all_hands[leap_hand.Id];
          hand_model.SetLeapHand(leap_hand);
          hand_model.MirrorZAxis(mirrorZAxis);

          // Set scaling based on reference hand.
          float hand_scale = MM_TO_M * leap_hand.PalmWidth / hand_model.handModelPalmWidth;
          hand_model.transform.localScale = hand_scale * transform.lossyScale;
          hand_model.UpdateHand();
        }
      }
    }

    // Destroy all hands with defunct IDs.
    for (int i = 0; i < ids_to_check.Count; ++i) {
      DestroyHand(all_hands[ids_to_check[i]]);
      all_hands.Remove(ids_to_check[i]);
    }
  }

  protected ToolModel CreateTool(ToolModel model) {
    ToolModel tool_model = Instantiate(model, transform.position, transform.rotation) as ToolModel;
    tool_model.gameObject.SetActive(true);
    Leap.Utils.IgnoreCollisions(tool_model.gameObject, gameObject);
    return tool_model;
  }

  protected void UpdateToolModels(Dictionary<int, ToolModel> all_tools,
                                  ToolList leap_tools, ToolModel model) {
    List<int> ids_to_check = new List<int>(all_tools.Keys);

    // Go through all the active tools and update them.
    int num_tools = leap_tools.Count;
    for (int h = 0; h < num_tools; ++h) {
      Tool leap_tool = leap_tools[h];
      
      // Only create or update if the tool is enabled.
      if (model) {

        ids_to_check.Remove(leap_tool.Id);

        // Create the tool and initialized it if it doesn't exist yet.
        if (!all_tools.ContainsKey(leap_tool.Id)) {
          ToolModel new_tool = CreateTool(model);
          new_tool.SetController(this);
          new_tool.SetLeapTool(leap_tool);
          new_tool.InitTool();
          all_tools[leap_tool.Id] = new_tool;
        }

        // Make sure we update the Leap Tool reference.
        ToolModel tool_model = all_tools[leap_tool.Id];
        tool_model.SetLeapTool(leap_tool);
        tool_model.MirrorZAxis(mirrorZAxis);

        // Set scaling.
        tool_model.transform.localScale = transform.lossyScale;

        tool_model.UpdateTool();
      }
    }

    // Destroy all tools with defunct IDs.
    for (int i = 0; i < ids_to_check.Count; ++i) {
      Destroy(all_tools[ids_to_check[i]].gameObject);
      all_tools.Remove(ids_to_check[i]);
    }
  }

  public Controller GetLeapController() {
    return leap_controller_;
  }

  public Frame GetFrame() {
    if (enableRecordPlayback && recorder_.state == RecorderState.Playing)
      return recorder_.GetCurrentFrame();

    return leap_controller_.Frame();
  }

  void Update() {
    if (leap_controller_ == null)
      return;
    
    UpdateRecorder();
    Frame frame = GetFrame();

    if (frame != null && !flag_initialized_)
    {
      InitializeFlags();
    }

    if (Input.GetKeyDown(KeyCode.H))
    {
      show_hands_ = !show_hands_;
    }

    if (show_hands_)
    {
      if (frame.Id != prev_graphics_id_)
      {
        UpdateHandModels(hand_graphics_, frame.Hands, leftGraphicsModel, rightGraphicsModel);
        prev_graphics_id_ = frame.Id;
      }
    }
    else
    {
      // Destroy all hands with defunct IDs.
      List<int> hands = new List<int>(hand_graphics_.Keys);
      for (int i = 0; i < hands.Count; ++i)
      {
        DestroyHand(hand_graphics_[hands[i]]);
        hand_graphics_.Remove(hands[i]);
      }
    }
  }

  void FixedUpdate() {
    if (leap_controller_ == null)
      return;

    Frame frame = GetFrame();

    if (frame.Id != prev_physics_id_)
    {
      UpdateHandModels(hand_physics_, frame.Hands, leftPhysicsModel, rightPhysicsModel);
      UpdateToolModels(tools_, frame.Tools, toolModel);
      prev_physics_id_ = frame.Id;
    }
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

  public HandModel[] GetAllGraphicsHands() {
    if (hand_graphics_ == null)
      return new HandModel[0];

    HandModel[] models = new HandModel[hand_graphics_.Count];
    hand_graphics_.Values.CopyTo(models, 0);
    return models;
  }

  public HandModel[] GetAllPhysicsHands() {
    if (hand_physics_ == null)
      return new HandModel[0];

    HandModel[] models = new HandModel[hand_physics_.Count];
    hand_physics_.Values.CopyTo(models, 0);
    return models;
  }

  public void DestroyAllHands() {
    if (hand_graphics_ != null) {
      foreach (HandModel model in hand_graphics_.Values)
        Destroy(model.gameObject);

      hand_graphics_.Clear();
    }
    if (hand_physics_ != null) {
      foreach (HandModel model in hand_physics_.Values)
        Destroy(model.gameObject);

      hand_physics_.Clear();
    }
  }

  public float GetRecordingProgress() {
    return recorder_.GetProgress();
  }

  public void StopRecording() {
    recorder_.Stop();
  }

  public void PlayRecording() {
    recorder_.Play();
  }

  public void PauseRecording() {
    recorder_.Pause();
  }

  public string FinishAndSaveRecording() {
    string path = recorder_.SaveToNewFile();
    recorder_.Play();
    return path;
  }

  public void ResetRecording() {
    recorder_.Reset();
  }

  public void Record() {
    recorder_.Record();
  }

  protected void UpdateRecorder() {
    if (!enableRecordPlayback)
      return;

    recorder_.speed = recorderSpeed;
    recorder_.loop = recorderLoop;

    if (recorder_.state == RecorderState.Recording) {
      recorder_.AddFrame(leap_controller_.Frame());
    }
    else {
      recorder_.NextFrame();
    }
  }
}
