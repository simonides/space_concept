using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;


public class InputHandler : MonoBehaviour, IEventSystemHandler
{

    //public LayerMask TouchInputMask;            
    public GameObject[] SingleTouchReceiver;    // one finger
    public GameObject[] DoubleTouchReceiver;    // tow fingers

    public delegate void ZoomEvent(float normalisedMagnitudeDiff); // To tell listeners that the map the mouse scroll or pinch is used by the user
    public static event ZoomEvent OnZoom;

    public delegate void TouchMoveEvent(Vector2 deltaPosition); // Use this delegate 
    public static event TouchMoveEvent OnTouchMove;


    // to interpret different gestures
    private bool _touchMoved = false;
    private bool _clickstarted = false;

    // to calculate the delta for the mouse
    private Vector3 _oldMousePosition;



    private void Zoom(float magnitudeDiff)
    {
        //Debug.Log("Zoom, send to subscribers: " + normalisedMagnitudeDiff);
        if (OnZoom != null) // null means there are no subscribers
        {
            OnZoom(magnitudeDiff);
        }
    }


    private void TouchMove(Vector2 deltaPosition)
    {
        if (OnTouchMove != null)
        {
            OnTouchMove(deltaPosition);
        }
    }


    public void Awake()
    {
        Input.simulateMouseWithTouches = false;
    }




    //change to fixed??
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed");
            //_uiHandler.ShowEndGameDialog();
        }
        //#if UNITY_EDITOR

        // wenn der button nicht mehr geklickt ist soll 
        if (!Input.GetMouseButton(0))
        {
            _clickstarted = false;
        }

        if (Input.GetMouseButton(0))
        {

            //Debug.Log("Mouse clicked: ");

            if (!_clickstarted)
            {
                //print("mousepressed:");
                _oldMousePosition = Input.mousePosition;
                _clickstarted = true;
            }


            //if (_oldMousePosition != Input.mousePosition) // this is safe because unity implements this like that: Vector3.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
            // but the delta may need to be higher to avoid clicks with jitter to be interpreted as movements
            if (Vector3.SqrMagnitude(_oldMousePosition - Input.mousePosition) >= 20.0f)
            {
                //print("mouse moved " + Vector3.SqrMagnitude(_oldMousePosition - Input.mousePosition));
                //_touchMoved = true;

                Vector3 deltaMousePos = _oldMousePosition - Input.mousePosition;
                _oldMousePosition = Input.mousePosition;

                TouchMove(deltaMousePos);
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("clicked on point");
            if (!_touchMoved)
            {
                SendTouchClick(Input.mousePosition);
            }
            _touchMoved = false;
            _clickstarted = false;
        }

        float scrollSpeed = Input.GetAxis("Mouse ScrollWheel");
        if (scrollSpeed > 0) // forward
        {
            Zoom(-12);

        }
        else if (scrollSpeed < 0)
        {
            Zoom(12);
        }


        //if (scrollSpeed != 0)
        //{
        //    scrollSpeed *= -1f;
        //    scrollSpeed *= 30f;
        //    //scrollSpeed *= 100f;
        //    //// t       speed
        //    ////------------------ -
        //    //// 0       0
        //    //// 1       0.00990
        //    //// 2       0.01960
        //    //// 3       0.02912
        //    //// 4       0.03846
        //    //// 5       0,04761
        //    //// 6       0.05660
        //    //// 42      0.29577
        //    //// 100     0.5
        //    //// 333     0.76905
        //    //// 1234    0.92503
        //    //// + inf    1.0
        //    ////1f - 100f / (100f + t)

        //#endif

        if (Input.touchCount == 1)
        {

            // Debug.Log("touching.. ");
            Touch touch = Input.GetTouch(0);
            //if (touch.position.x > (Screen.width - Screen.width * SidebarWidth))
            //{
            //    return;
            //}
            TouchPhase phase = touch.phase;
            switch (phase)
            {
                case TouchPhase.Began:
                    _touchMoved = false;
                    break;

                case TouchPhase.Moved:
                    _touchMoved = true;
                    TouchMove(touch.deltaPosition);
                    break;

                case TouchPhase.Stationary:
                    TouchMove(touch.deltaPosition);
                    break;

                case TouchPhase.Ended:
                    if (!_touchMoved)
                    {
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
        }
        else if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            //if (touchZero.position.x > (Screen.width - Screen.width * SidebarWidth))
            //{
            //    return;
            //}

            //if (GameEventSystem.IsPointerOverGameObject(touchZero.fingerId) || GameEventSystem.IsPointerOverGameObject(touchOne.fingerId))
            //{
            //return;
            //}
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;

            deltaMagnitudediff *= .22f;

            Zoom(deltaMagnitudediff);
            //foreach (var receiver in DoubleTouchReceiver)
            //{
            //    receiver.SendMessage("DoubleTouchAnywhere", deltaMagnitudediff, SendMessageOptions.DontRequireReceiver);
            //}
        }
    }


    private void SendTouchClick(Vector3 InputPos)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(InputPos);

        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);
        if (hits.Count() == 0)
        {
            //_uiHandler.ClickedOnEmptySpace();
            return;
        }
        foreach (RaycastHit2D hit in hits)
        {
            GameObject recipient = hit.transform.gameObject;
            if (hits.Count() > 1 && recipient.CompareTag("ship"))
            { // should go throug and fire to planet only
                continue;
            }
            recipient.SendMessage("SingleTouchClick", SendMessageOptions.DontRequireReceiver);
        }

    }
}

