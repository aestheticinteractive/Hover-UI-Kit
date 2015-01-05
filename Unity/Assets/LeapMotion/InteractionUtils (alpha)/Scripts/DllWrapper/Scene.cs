using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Leap.Interact
{
  /// <summary>
  /// Represents an interactive Scene.
  /// </summary>
  public class Scene
  {
    //
    // Parameters 
    //

    public float Scale;
    public LeapTransform Transform;

    public int LastError { get { return Native.AccessPropertyAsInt(this, Native.Property.LastError, Native.Mode.Get, 0); } }
    public void ClearLastError() { Native.AccessPropertyAsInt(this, Native.Property.LastError, Native.Mode.Clear, 0); }

    public bool MirrorHandsAlongZ { set { Native.AccessPropertyAsBool(this, Native.Property.SceneMirrorHandsAlongZ, Native.Mode.Set, value); } }
    public float HandDistanceMultiplier { set { Native.AccessPropertyAsFloat(this, Native.Property.SceneHandDistanceMultiplier, Native.Mode.Set, value); } }
    public bool DestroyClustersWhileNotHolding { set { Native.AccessPropertyAsBool(this, Native.Property.SceneDestroyClustersWhileNotHolding, Native.Mode.Set, value); } }

    public bool RunCollisionDetection { set {Native.AccessPropertyAsBool(this, Native.Property.SceneRunCollisionDetection, Native.Mode.Set, value); } }
    public bool ResolveCollisions { set {Native.AccessPropertyAsBool(this, Native.Property.SceneResolveCollisions, Native.Mode.Set, value); } }
    public bool AlwaysRunCollisionForHandsVsHeldObjects { set {Native.AccessPropertyAsBool(this, Native.Property.SceneAlwaysRunCollisionForHandsVsHeldObjects, Native.Mode.Set, value); } }

    public bool AllowPinchWithAnyFinger {
      get { return Native.AccessPropertyAsBool(this, Native.Property.SceneAllowPinchWithAnyFinger, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.SceneAllowPinchWithAnyFinger, Native.Mode.Set, value); }
    }
    
    public bool DisableHoldingOnPointingIndexFinger {
      get { return Native.AccessPropertyAsBool(this, Native.Property.SceneDisableHoldingOnPointingIndexFinger, Native.Mode.Get, false); }
      set { Native.AccessPropertyAsBool(this, Native.Property.SceneDisableHoldingOnPointingIndexFinger, Native.Mode.Set, value); }
    }


    public void DestroyScene() { Native.DestroyScene(this); }

    public LeapTransform GetRightIndexFinger { get { return new LeapTransform(Native.AccessPropertyAsTransform(this, Native.Property.SceneRightIndexFinger, Native.Mode.Get, LeapTransform.Identity.ToNative())); } }

    public void Clear() { Native.AccessPropertyAsInt(this, Native.Property.Instance, Native.Mode.Clear, 0); } 


    static public float ClientUnitLengthInMillimeters { set { Native.SetClientLengthUnit(value); } }
    static public LeapTransform LeapOriginInClient { set { Native.SetCameraTransform(value.ToNative()); } }

    public delegate void BodyNotification(Body body);
    public delegate void HoldingNotification(Holding holding);

    public event BodyNotification OnBodyAdded;
    public event BodyNotification OnBodyRemoved;
    public event HoldingNotification OnHoldingHoverOver = delegate {};
    public event HoldingNotification OnHoldingStarts = delegate {};
    public event HoldingNotification OnHoldingUpdates = delegate {};
    public event HoldingNotification OnHoldingEnds = delegate {};
    public event BodyNotification OnBodyScaled = delegate {};

    private Native.CallbackPtrDelegate bodyAddedDelegate;
    private Native.CallbackPtrDelegate bodyRemovedDelegate;

    private Native.CallbackPtrDelegate holdingHoverOverDelegate;
    private Native.CallbackPtrDelegate holdingStartsDelegate;
    private Native.CallbackPtrDelegate holdingUpdatesDelegate;
    private Native.CallbackPtrDelegate holdingEndsDelegate;

    private Native.CallbackPtrDelegate bodyScaledDelegate;

    public Scene()
    {
      SceneId = Native.CreateScene();
      HasFigure = false;

      bodyAddedDelegate = new Native.CallbackPtrDelegate(BodyAddedCallback);
      bodyRemovedDelegate = new Native.CallbackPtrDelegate(BodyRemovedCallback);

      Native.AccessPropertyAsPtrCallback(this, Native.Property.RegisterBodyAddedCallback, Native.Mode.Set, Marshal.GetFunctionPointerForDelegate(bodyAddedDelegate));
      Native.AccessPropertyAsPtrCallback(this, Native.Property.RegisterBodyRemovedCallback, Native.Mode.Set, Marshal.GetFunctionPointerForDelegate(bodyRemovedDelegate));

      holdingHoverOverDelegate = new Native.CallbackPtrDelegate(HoldingHoverOverCallback);
      holdingStartsDelegate = new Native.CallbackPtrDelegate(HoldingStartsCallback);
      holdingUpdatesDelegate = new Native.CallbackPtrDelegate(HoldingUpdatesCallback);
      holdingEndsDelegate = new Native.CallbackPtrDelegate(HoldingEndsCallback);
    
      Native.AccessPropertyAsPtrCallback(this, Native.Property.RegisterHoldingHoverOverCallback, Native.Mode.Set, Marshal.GetFunctionPointerForDelegate(holdingHoverOverDelegate));
      Native.AccessPropertyAsPtrCallback(this, Native.Property.RegisterHoldingStartsCallback, Native.Mode.Set, Marshal.GetFunctionPointerForDelegate(holdingStartsDelegate));
      Native.AccessPropertyAsPtrCallback(this, Native.Property.RegisterHoldingUpdatesCallback, Native.Mode.Set, Marshal.GetFunctionPointerForDelegate(holdingUpdatesDelegate));
      Native.AccessPropertyAsPtrCallback(this, Native.Property.RegisterHoldingEndsCallback, Native.Mode.Set, Marshal.GetFunctionPointerForDelegate(holdingEndsDelegate));

      bodyScaledDelegate = new Native.CallbackPtrDelegate(BodyScaledCallback);

      Native.AccessPropertyAsPtrCallback(this, Native.Property.RegisterBodyScaledCallback, Native.Mode.Set, Marshal.GetFunctionPointerForDelegate(bodyScaledDelegate));
    }

    ~Scene() {
      DestroyScene();
      Invalidate();
    }

    // Body listener callbacks
    private void BodyAddedCallback(IntPtr ptr) {
    if (UseBodyCallbacks) OnBodyAdded(new Body(ptr));
  }
    private void BodyRemovedCallback(IntPtr ptr) { if (UseBodyCallbacks) OnBodyRemoved(new Body(ptr)); }

    // Holding listener callbacks
    private void HoldingHoverOverCallback(IntPtr ptr) { if (UseHoldingCallbacks) OnHoldingHoverOver(new Holding(ptr)); }
    private void HoldingStartsCallback(IntPtr ptr) { if (UseHoldingCallbacks) OnHoldingStarts(new Holding(ptr)); }
    private void HoldingUpdatesCallback(IntPtr ptr) { if (UseHoldingCallbacks) OnHoldingUpdates(new Holding(ptr)); }
    private void HoldingEndsCallback(IntPtr ptr) { if (UseHoldingCallbacks) OnHoldingEnds(new Holding(ptr)); }

    private void BodyScaledCallback(IntPtr ptr) { if (UseScalingCallbacks) OnBodyScaled(new Body(ptr)); }

    /// <summary>
    /// Adds a body to the scene.
    /// </summary>
    /// <param name="body"></param>
    public void AddBody(Body body)
    {
      Native.AddBody(this, body);
    }

    public void RemoveBody(Body body)
    {
      Native.RemoveBody(this, body); // invalidates body
      body.Invalidate();
    }

    /// <summary>
    /// Opens an internal debugger/visualizer window in the native dll.
    /// </summary>
    public void OpenVisualDebgger()
    {
      if (!HasFigure)
      {
        FigureId = Native.OpenFigure();
        HasFigure = true;
      }
    }

    /// <summary>
    /// Steps the Leap scene. Reads leap data, updates hands, updates body transforms.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
      Native.UpdateScene(this, deltaTime);
      if (HasFigure)
      {
        Native.UpdateFigure(FigureId, SceneId);
      }
    }

    //
    // Native references
    //

    public void Invalidate() { SceneId.ptr = IntPtr.Zero; FigureId.ptr = IntPtr.Zero; }

    private Native.SceneId SceneId;
    public static implicit operator Native.SceneId (Scene scene) { return scene.SceneId; }
    public static implicit operator IntPtr (Scene scene) { return scene.SceneId.ptr; }

    private Native.FigureId FigureId;
    private bool HasFigure;
    public bool UseBodyCallbacks = true;
    public bool UseHoldingCallbacks = true;
    public bool UseScalingCallbacks = true;
  }
}
