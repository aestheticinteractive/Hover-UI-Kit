using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Leap.Interact
{
  public partial class Native
  {
    public enum Property
    {
      LastError,
      
      Instance,
      
      // Scene
      SceneClickBodies,
      SceneGenerateAnchorsForNewBodies, // deprecated
      SceneGenerateClickAnchorsForNewBodies, // deprecated
      //SceneAddSmoothedTransformModifierForNewBodies, !!! ONLY ADD TO THE END OF THE LIST 

      SceneRunCollisionDetection,
      SceneResolveCollisions,
      
      BodyMass,
      BodyShape,
      BodyTransform,
      BodyMotionType,

      BodyEnableCollisionShapeHolding,
      BodyEnableAnchorBasedHolding,

      BodyUsePalmPositionForHoldings,
      BodyUsePalmRotationForHoldings,

      BodyEnableReorientationOnMultiHolding,
      BodyEnableScalingOnMultiHolding,
      BodyEnableHandCollisionWhileHolding,
      BodyEnableRotationWithFingersWhileHolding,
      BodyOvertakeControlWithNewerHoldings,
      BodyEnableAveragingTransformsOnMultiHolding,
      BodyRotationalStrengthLowThreshold,
      BodyRotationalStrengthHighThreshold,
      BodyLockRelativeOrientationOnRotationalStrenghtHighThresholdOnly,

      BodyEnableClicking,
      BodyAlwaysActiveForClicking,

      // don't continue with this...
      HandsCount, //get
      HandBoneTransform, // get, store hand id & finger id & joint id in the vector

      RegisterBodyAddedCallback,
      RegisterBodyRemovedCallback,

      QueryForBodiesAdded,
      QueryForBodiesRemoved,

      ShapeType,
      ShapeCapsuleSegmentLength,
      ShapeCapsuleRadius,

      ShapeAnchors,
      ShapeClickAnchors,

      BodyIsHeld,

      SceneRightIndexFinger,

      SceneAddSmoothedTransformModifierForNewBodies,
      SceneMirrorHandsAlongZ,
      SceneHandDistanceMultiplier,
      SceneDestroyClustersWhileNotHolding,
      BodyMaxMagneticGrabDistance,

      HoldingBody,
      HoldingStrength,
      HoldingTransform,
      HoldingBodyTransform,

      RegisterHoldingHoverOverCallback,
      RegisterHoldingStartsCallback,
      RegisterHoldingUpdatesCallback,
      RegisterHoldingEndsCallback,

      BodyHandAnchorType,
      BodyDefaultPalmAnchorLeft,
      BodyDefaultPalmAnchorRight,
      BodyLockRotationAboveThisStrength,
      BodyUseCurrentRelativeRotationWhenLockingRotation,
      BodyPinchDistanceThresholdForMinStrength,
      BodyPinchDistanceThresholdForMaxStrength,

      BodyGenerateDefaultAnchors,
      BodyGenerateDefaultClickAnchors,
      BodyEnableSmoothGrabbing,

      RegisterBodyAddedCallback_reserverd, // todo: rename to HandAdded
      RegisterBodyRemovedCallback_reserverd, // todo: rename to HandRemoved
      RegisterBodyScaledCallback, // todo: rename to HandAdded
      BodyScale,

      SceneAlwaysRunCollisionForHandsVsHeldObjects,
      BodyMagneticStrength,
      SceneAllowPinchWithAnyFinger,
      SceneDisableHoldingOnPointingIndexFinger,

      NumProperties

    }
    
    public enum Mode
    {
      Get,
      Set,
      
      Add,
      Remove,
      
      Clear,
      
      Craete,
      Release,

      Call,

      NumModeIds
    }

    public enum ErrorCodes
    {
      Success,
      Failure
    }

    //
    // Maths types Vector3, Quaternion, and Transform.
    //

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 { public float x, y, z; }

    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion { public float x, y, z, w; }

    [StructLayout(LayoutKind.Sequential)]
    public struct Transform { public Vector3 pos; private float pad; public Quaternion rot; } // pad uninitialized

    //
    // Handles for Scene, Shape, Body, and Figure.
    //

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneId { internal IntPtr ptr; public SceneId(IntPtr ptr) { this.ptr = ptr; } }

    [StructLayout(LayoutKind.Sequential)]
    public struct ShapeId { internal IntPtr ptr; }

    [StructLayout(LayoutKind.Sequential)]
    public struct BodyId { public IntPtr ptr; public BodyId(IntPtr ptr) { this.ptr = ptr; } }

    [StructLayout(LayoutKind.Sequential)]
    public struct FigureId { internal IntPtr ptr; }

    [StructLayout(LayoutKind.Sequential)]
    public struct HoldingId { internal IntPtr ptr; }

    public static /*extern*/ int SetProperty<T>(IntPtr objectId, Property propertyId, Mode modeId, T property) {
      IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf (property));
      Marshal.StructureToPtr(property, pnt, false);

      if (property is Vector3)
      {
        object tmp = property;
        AccessPropertyAsVector3( objectId, propertyId, modeId, (Vector3)tmp);
      }

      // call method on pointer.
      return 0;
    }


  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern IntPtr AccessPropertyAsPtr(IntPtr objectId, Property propertyId, Mode modeId, IntPtr property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern bool AccessPropertyAsBool(IntPtr objectId, Property propertyId, Mode modeId, bool property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern int AccessPropertyAsInt(IntPtr objectId, Property propertyId, Mode modeId, int property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern float AccessPropertyAsFloat(IntPtr objectId, Property propertyId, Mode modeId, float property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
  public static extern Vector3 AccessPropertyAsVector3 (IntPtr objectId, Property propertyId, Mode modeId, Vector3 property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
  public static extern Quaternion AccessPropertyAsQuaternion(IntPtr objectId, Property propertyId, Mode modeId, Quaternion property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
  public static extern Transform AccessPropertyAsTransform (IntPtr objectId, Property propertyId, Mode modeId, Transform property);

    public delegate void CallbackNoArgsDelegate();
    public delegate void CallbackPtrDelegate(IntPtr ptr);
    public delegate void CallbackIntFloatTranformDelegate(int i, float f, Transform t);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern CallbackNoArgsDelegate AccessPropertyAsNoArgsCallback(IntPtr objectId, Property propertyId, Mode modeId, IntPtr property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern CallbackPtrDelegate AccessPropertyAsPtrCallback(IntPtr objectId, Property propertyId, Mode modeId, IntPtr property);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern CallbackIntFloatTranformDelegate AccessPropertyAsIntFloatTranformCallback(IntPtr objectId, Property propertyId, Mode modeId, IntPtr property);


    //
    // Debug viewer 'Figure' operation.
    //

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern FigureId OpenFigure();

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void UpdateFigure(FigureId figureId, SceneId sceneId);

    //
    // Unit conversion between the client and the Leap scene.
    //

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void SetClientLengthUnit(float unitLengthInMillimeters);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void SetCameraTransform(Transform cameraTransform);


    //
    // Leap Scene creation, manipuation, and destruction.
    //

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern SceneId CreateScene();

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void AddBody(SceneId sceneId, BodyId bodyId);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void RemoveBody(SceneId sceneId, BodyId bodyId);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void UpdateScene(SceneId sceneId, float deltaTime);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void DestroyScene(SceneId sceneId);

    //
    // Body creation.
    //

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern BodyId CreateBody();

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void ReleaseBody(BodyId bodyId);


    //
    // Shape creation
    //

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern ShapeId CreateSphereShape(float radius);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern ShapeId CreateCapsuleShape(CapsuleOrientation capsuleOrientation, float segmentHalfLength, float radius);
    public  enum CapsuleOrientation { AlongXAxis, AlongYAxis, AlongZAxis }

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern ShapeId CreateBoxShape(Vector3 halfSize, float radius);

    //// user enum for property

    ////void LM_CALL DestroyShape(ShapeId shape);
  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void ReleaseShape(ShapeId shapeId);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern ShapeId GetBodyShape(BodyId bodyId);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern void SetBodyShape(BodyId bodyId, ShapeId shapeId);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
    public static extern float GetBodyMass(BodyId bodyId);

  [DllImport("Leap3dInteractDll", CallingConvention=CallingConvention.StdCall)]
  public static extern void SetBodyMassAndInertia(BodyId bodyId, float mass);
  }
}
