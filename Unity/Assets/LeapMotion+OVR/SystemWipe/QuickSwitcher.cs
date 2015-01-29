using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuickSwitcher : MonoBehaviour {

	public bool m_enabled = false;
  [SerializeField]
  private HandController m_handController;
	[SerializeField]
	private float m_minProgressToStartTransition;
	[SerializeField]
	private float m_percentageToLockTransition;
	[SerializeField]
	private Vector3 m_wipeOutPosition;
  [SerializeField]
  private LeapImageRetriever m_imageRetriever;

	private Vector3 m_startPosition;

	private enum TransitionState { ON, OFF, MANUAL, TWEENING };
	private TransitionState m_currentTransitionState;
	// Know what the last locked state was so we know what we're transitioning to.
	private TransitionState m_lastLockedState; 

	// Where are we transitioning to and from
	private Vector3 m_from; 
	private Vector3 m_to;

	private delegate void TweenCompleteDelegate();

	// Use this for initialization
	void Start () {
		m_startPosition = transform.localPosition;
		m_wipeOutPosition = m_startPosition + m_wipeOutPosition;
		m_from = m_startPosition;
		m_to = m_wipeOutPosition;
		m_lastLockedState = TransitionState.ON;
		SystemWipeRecognizerListener.Instance.SystemWipeUpdate += onWipeUpdate;
    TweenToOffPosition();
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void onWipeUpdate(object sender, SystemWipeArgs eventArgs) {
		if ( !m_enabled ) { return; }

		string debugLine = "Debug";
		if ( eventArgs.WipeInfo.Status == Leap.Util.Status.SwipeAbort ) {
			debugLine += " | Abort";
			// If the user aborts, tween back to original location
			if ( m_lastLockedState == TransitionState.ON ) {
				TweenToOnPosition();
			}
			else {
				TweenToOffPosition();
			}
		}

		if ( m_currentTransitionState == TransitionState.MANUAL ) {
			debugLine += " | Manual Control";
			float percentage = Mathf.Clamp01(eventArgs.WipeInfo.Progress);

			debugLine += ": " + percentage;
			transform.localPosition = Vector3.Lerp(m_from, m_to, percentage);

			// If we're sure of the gesture, just go make the transition
			if ( percentage >= m_percentageToLockTransition ) {
				debugLine += " | Transition Cofirmed";
				if ( m_lastLockedState == TransitionState.OFF ) {
					TweenToOnPosition();
				}
				else {
					TweenToOffPosition();
				}
			}
		}
		else if ( m_currentTransitionState == TransitionState.TWEENING ) {
			debugLine += " | Currently Tweening";
			//Debug.Log(debugLine);
			return;
		}
		else { // We're either on or off
			debugLine += " | Locked";
			if ( eventArgs.WipeInfo.Progress >= m_minProgressToStartTransition ) {
				debugLine += " | Go To Manual";
				m_currentTransitionState = TransitionState.MANUAL; 
			}
		}

		//Debug.Log(debugLine);
	}

	private void onOnPosition() {
		//Debug.Log("onOnPosition");
		m_currentTransitionState = TransitionState.ON;
		m_lastLockedState = TransitionState.ON;
		m_from = m_startPosition;
		m_to = m_wipeOutPosition;
    m_handController.gameObject.SetActive(false);
	}

	private void onOffPosition() {
		//Debug.Log("onOffPosition");
		m_currentTransitionState = TransitionState.OFF;
		m_lastLockedState = TransitionState.OFF;
		m_from = m_wipeOutPosition;
		m_to = m_startPosition;
    if ( m_imageRetriever != null ) {
      m_imageRetriever.doUpdate = false;
    }
    else {
      Debug.LogError("No image retreiver on: " + gameObject.name);
    }
    m_handController.gameObject.SetActive(true);
	}

	public void TweenToOnPosition() {
		//Debug.Log("tweenToOnPosition");
    m_imageRetriever.doUpdate = true;
		StopAllCoroutines();
		StartCoroutine(doPositionTween(0.0f, 0.1f, onOnPosition));
	}

	public void TweenToOffPosition() {
//		Debug.Log("tweenToOffPosition");
		StopAllCoroutines();
		StartCoroutine(doPositionTween(1.0f, 0.1f, onOffPosition));
	}

	public void TweenToPosition(float percentage, float time = 0.4f) {
		m_currentTransitionState = TransitionState.TWEENING;
		StopAllCoroutines();
		StartCoroutine(doPositionTween(percentage, time));
	}

	private IEnumerator doPositionTween(float goalPercent, float transitionTime, TweenCompleteDelegate onComplete = null) {
//		Debug.Log("doPositionTween: " + goalPercent);
		float startTime = Time.time;

		Vector3 from = transform.localPosition;
		Vector3 to = Vector3.Lerp(m_startPosition, m_wipeOutPosition, goalPercent);

		while ( true ) { 
			float percentage = Mathf.Clamp01((Time.time - startTime)/transitionTime);
//			Debug.Log("Tween step: " + percentage);

			transform.localPosition = Vector3.Lerp(from, to, percentage);

			// Kick out of the loop if we're done
			if ( percentage == 1 ) {
				break;
			} else { // otherwise continue
				yield return 1;
			}
		}

		if ( onComplete != null ) {
			onComplete();
		}
	}
}

