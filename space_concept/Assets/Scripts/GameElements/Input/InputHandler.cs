using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public
class InputHandler : MonoBehaviour, IEventSystemHandler {
public
  GameObject[] SingleTouchReceiver; // one finger

public
  delegate void ZoomEvent(float normalisedMagnitudeDiff); // To tell listeners
                                                          // that the map the
                                                          // mouse scroll or
                                                          // pinch is used by
                                                          // the user
public
  static event ZoomEvent OnZoom;

public
  delegate void TouchMoveEvent(Vector2 deltaPosition); // Use this delegate
public
  static event TouchMoveEvent OnTouchMove;

  // to interpret different gestures
private
  bool _touchMoved = false;
private
  bool _clickstarted = false;

  // to calculate the delta for the mouse
private
  Vector3 _oldMousePosition;

  // use to deactivate the mapmovement when the menu is active
private
  bool _menuActive = false;

private
  void Zoom(float magnitudeDiff) { // Debug.Log("Zoom, send to subscribers: " +
                                   // normalisedMagnitudeDiff);
    if (OnZoom != null)            // null means there are no subscribers
    {
      OnZoom(magnitudeDiff);
    }
  }

private
  void TouchMove(Vector2 deltaPosition) {
    if (OnTouchMove != null) {
      OnTouchMove(deltaPosition);
    }
  }

public
  void Awake() {
    Input.simulateMouseWithTouches = false;
    MessageHub.Subscribe<MenuActiveEvent>(MapMovement);
  }

private
  void MapMovement(MenuActiveEvent mapEvent) {
    _menuActive = mapEvent.Content;
    Debug.Log("MenuActive event: " + mapEvent.Content);
  }

  void FixedUpdate() {
    if (_menuActive) {
      return;
    }
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Debug.Log("ESC pressed");
      //_uiHandler.ShowEndGameDialog();
    }

    //#if UNITY_EDITOR

    // wenn der button nicht mehr geklickt ist soll
    if (!Input.GetMouseButton(0)) {
      _clickstarted = false;
    }

    if (Input.GetMouseButton(0)) { // Debug.Log("Mouse clicked: ");
      if (!_clickstarted) {
        // print("mousepressed:");
        _oldMousePosition = Input.mousePosition;
        _clickstarted = true;
      }
      // if (_oldMousePosition != Input.mousePosition) // this is safe because
      // unity implements this like that: Vector3.SqrMagnitude(lhs - rhs) <
      // 9.99999944E-11f;
      // but the delta may need to be higher to avoid clicks with jitter to be
      // interpreted as movements
      if (Vector3.SqrMagnitude(_oldMousePosition - Input.mousePosition) >=
          20.0f) {
        // print("mouse moved " + Vector3.SqrMagnitude(_oldMousePosition -
        // Input.mousePosition));
        //_touchMoved = true;

        Vector3 deltaMousePos = _oldMousePosition - Input.mousePosition;
        _oldMousePosition = Input.mousePosition;

        deltaMousePos *= -1f;
        TouchMove(deltaMousePos);
      }
    }

    if (Input.GetMouseButtonUp(0)) {
      if (!_touchMoved) {
        SendTouchClick(Input.mousePosition);
      }
      _touchMoved = false;
      _clickstarted = false;
    }

    float scrollSpeed = Input.GetAxis("Mouse ScrollWheel");
    if (scrollSpeed > 0) // forward
    {
      Zoom(-12);
    } else if (scrollSpeed < 0) {
      Zoom(12);
    }
    if (Input.touchCount == 1) {
      // Debug.Log("touching.. ");
      Touch touch = Input.GetTouch(0);
      TouchPhase phase = touch.phase;
      switch (phase) {
      case TouchPhase.Began:
        _touchMoved = false;
        break;

      case TouchPhase.Moved:
        _touchMoved = true;
        TouchMove(touch.deltaPosition * 3);
        break;

      case TouchPhase.Stationary:
        TouchMove(touch.deltaPosition);
        break;

      case TouchPhase.Ended:
        if (!_touchMoved) {
          SendTouchClick(touch.position);
        }
        break;

      case TouchPhase.Canceled:
        _touchMoved = false;
        break;
      default:
        _touchMoved = false;
        break;
      }
    } else if (Input.touchCount == 2) {
      Touch touchZero = Input.GetTouch(0);
      Touch touchOne = Input.GetTouch(1);
      Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
      Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

      float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
      float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
      float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;
      deltaMagnitudediff *= 2f;

      Zoom(deltaMagnitudediff);
    }
  }

private
  void SendTouchClick(Vector3 InputPos) {
    Vector3 pos = Camera.main.ScreenToWorldPoint(InputPos);

    RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);
    if (hits.Count() == 0) {
      //_uiHandler.ClickedOnEmptySpace();
      return;
    }
    foreach (RaycastHit2D hit in hits) {
      GameObject recipient = hit.transform.gameObject;
      if (hits.Count() > 1 &&
          recipient.CompareTag(
              "Menu")) { // should go throug and fire to planet only
        Debug.Log("menu active");
        continue;
      }
      recipient.SendMessage("SingleTouchClick",
                            SendMessageOptions.DontRequireReceiver);
    }
  }
}
