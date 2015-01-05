using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {
  public GameObject leapMotionOVRController = null;
  public HandController handController = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
    if (leapMotionOVRController == null || handController == null)
      return;

    // Move forward if both palms are facing outwards! Whoot!
    HandModel[] hands = handController.GetAllGraphicsHands();
    if (hands.Length > 1)
    {
      Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
      Vector3 normal0 = hands[0].GetPalmNormal().normalized;

      Vector3 direction1 = (hands[1].GetPalmPosition() - handController.transform.position).normalized;
      Vector3 normal1 = hands[1].GetPalmNormal().normalized;

      if (Vector3.Dot(direction0, normal0) > direction0.sqrMagnitude * 0.5f && Vector3.Dot(direction1, normal1) > direction1.sqrMagnitude * 0.5f)
      {
        Vector3 target = (hands[0].GetPalmPosition() + hands[1].GetPalmPosition()) / 2.0f;
        leapMotionOVRController.transform.position = Vector3.Lerp(leapMotionOVRController.transform.position, target, 0.1f);
      }
    }
	}
}
