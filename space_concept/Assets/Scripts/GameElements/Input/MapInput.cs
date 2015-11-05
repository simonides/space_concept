using UnityEngine;
using System.Collections;


//Note this class depends on the map beeing scaled to 1.
public class MapInput : MonoBehaviour
{

    //public const float OrthoZoomSpeed = 1f; // 0.4f;
    public const float ScrollAnimationSpeed = 4.5f;

    public const float CameraBorderOffset = 3f;
    public const float PreventShakeOffset = 50f;//2.5f;

    //private const float SidePanelFactor = 1 - 0.7724556f;

    private float _cameraMinOrthoSize;
    private float _cameraMaxOrthoSize;

    private float _cameraWidthHalf;
    private float _cameraHeightHalf;

    private Space _map;
    private float _smallestPlanetDiameter;
    private Vector3 _moveToPosition;// = new Vector3(0, 0, 0);

    private float _cameraOrthoSizeTo;


    private Vector2 FindHomePlanet()
    {
        //int _planetCount = _map.GetPlanetCount();
        //for (int i = 0; i < _planetCount; ++i)
        //{
        //    PlanetEntity planet = _map.GetPlanetByIndex(i);
        //    if (planet.owner != null && planet.owner.networkPlayer == Network.player)
        //    {   //gehört mir..
        //        print(planet.position);
        //        _map.SetPosition(new Vector2(0, 0), 1);
        //        return _map.GetPosition() - (planet.position + new Vector2(planet.diameter * 2, planet.diameter * 2));
        //    }
        //}
        //return new Vector2(_map.GetPosition().x - _cameraWidthHalf, _map.GetPosition().y - _cameraHeightHalf);
        return new Vector2(0, 0);
    }

    void Zoom(float magnitudeDiff)
    {
        //  Debug.Log("Zoom map by: " + normalisedMagnitudeDiff);
        magnitudeDiff = Mathf.Clamp(magnitudeDiff, -20, 20);
        _cameraOrthoSizeTo += magnitudeDiff; // * OrthoZoomSpeed;
        _cameraOrthoSizeTo = Mathf.Clamp(_cameraOrthoSizeTo, _cameraMinOrthoSize, _cameraMaxOrthoSize);

        Camera.main.orthographicSize = _cameraOrthoSizeTo;
    }

    void OnTouchMove(Vector2 dist)
    {
        // have a different scrollspeed depending on the zoom level

        //float orthoDist = _cameraMaxOrthoSize - _cameraMinOrthoSize;
        //float multiplicand = Camera.main.orthographicSize / orthoDist;
        //multiplicand *= 0.45f;
        //acceleration.x *= multiplicand;
        //acceleration.y *= multiplicand;

        ////print("SingleTouchAnywhere: " + acceleration.x + " | " + acceleration.y);
        //_moveToPosition.x += acceleration.x;
        //_moveToPosition.y += acceleration.y;

        //Debug.Log("move by: " + dist);
        _moveToPosition += new Vector3(dist.x, dist.y, 0f);
    }

    void Awake()
    {
        InputHandler.OnZoom += this.Zoom;               // Subscribe to Zoom Event
        InputHandler.OnTouchMove += this.OnTouchMove;   // Subscribe to OnTouchMove Event

        _map = GameObject.Find("Space").GetComponent<Space>();
        if (_map == null)
        {
            throw new MissingComponentException("Unable to find Init Item.");
        }

        _cameraOrthoSizeTo = Camera.main.orthographicSize; // needed?

        _cameraWidthHalf = Camera.main.orthographicSize * Camera.main.aspect;
        _cameraHeightHalf = Camera.main.orthographicSize;

        _smallestPlanetDiameter = 25; // _map.GetSmallestDiameter();

        RecalculateCameraMinAndMaxSize();


        float cameraInitOrthoSize = Mathf.Max(_cameraMaxOrthoSize, _cameraMinOrthoSize) * 2 / 3; // max function is needed when the map returns a size of 0
        Camera.main.orthographicSize = cameraInitOrthoSize;
        _moveToPosition = _map.transform.position;

        //_moveToPosition = FindHomePlanet();
    }


    public void Update()
    {
        RecalculateCameraMinAndMaxSize();

        Vector3 oldPosition = _map.transform.position;
        if (oldPosition != _moveToPosition)
        {
            Vector3 newPosition = Vector2.Lerp(
                 _map.transform.position,
                 _moveToPosition,
                 ScrollAnimationSpeed * Time.deltaTime);
           

            if (Vector3.SqrMagnitude(newPosition - oldPosition) >= .5f)
            {
                _map.transform.position = newPosition;
            }
        }
        CorrectPosition();

    }


    private void RecalculateCameraMinAndMaxSize()
    {
        Vector2 mapSize = _map.GetSize();

        float minOrthoSizeX = ((mapSize.x + mapSize.x) / Camera.main.aspect / 2) - PreventShakeOffset; //-1 that the camera doesn't shake
        float minOrthoSizeY = (mapSize.y / 2) - PreventShakeOffset;

        _cameraMinOrthoSize = _smallestPlanetDiameter * 4f;
        _cameraMaxOrthoSize = Mathf.Min(minOrthoSizeX, minOrthoSizeY);
    }

    // used to avoid black borders around the map or scroll completley away
    private void CorrectPosition()
    {

        Vector2 mapTranslation = _map.transform.position;
        Vector2 mapSize = _map.GetSize();
        Vector2 mapOrigin = _map.GetCenter();
        float xMin = mapTranslation.x + (mapSize.x - mapSize.x / 2f - mapOrigin.x) * -1;
        float xMax = mapTranslation.x + (mapSize.x / 2f + mapOrigin.x);
        float yMin = mapTranslation.y + (mapSize.y - mapSize.y / 2f - mapOrigin.y) * -1;
        float yMax = mapTranslation.y + (mapSize.y / 2f + mapOrigin.y);


        _cameraWidthHalf = Camera.main.orthographicSize * Camera.main.aspect;
        _cameraHeightHalf = Camera.main.orthographicSize;
        float cameraXMin = (Camera.main.transform.position.x - _cameraWidthHalf);
        float cameraXMax = (Camera.main.transform.position.x + _cameraWidthHalf); // /*- (_cameraWidthHalf * 2 /** SidePanelFactor*/) + 1*/);
        float cameraYMin = (Camera.main.transform.position.y - _cameraHeightHalf);
        float cameraYMax = (Camera.main.transform.position.y + _cameraHeightHalf);

        if (xMin > cameraXMin)
        {
            print("left " + xMin + " " + cameraXMin);
            _moveToPosition = new Vector2(
                    cameraXMin + (mapSize.x - mapSize.x / 2f - mapOrigin.x) - CameraBorderOffset,
                    _map.transform.position.y);
            _map.transform.position = _moveToPosition;
        }

        if (xMax < cameraXMax)
        {
            print("right");
            _moveToPosition = new Vector2(
                  ((cameraXMax * -1) + mapSize.x / 2f + mapOrigin.x) * -1 + CameraBorderOffset,
                   _map.transform.position.y);
            _map.transform.position = _moveToPosition;
        }

        if (yMin > cameraYMin) // check bottom
        {
            print("bottom");
            _moveToPosition = new Vector2(
                  _map.transform.position.x,
                   cameraYMin + (mapSize.y - mapSize.y / 2f - mapOrigin.y) - CameraBorderOffset);
            _map.transform.position = _moveToPosition;
        }

        if (yMax < cameraYMax) // check top
        {
            print("top");
            _moveToPosition = new Vector2(
                   _map.transform.position.x,
                    ((cameraYMax * -1) + mapSize.y / 2f + mapOrigin.y) * -1 + CameraBorderOffset);
            _map.transform.position = _moveToPosition;
        }
        _moveToPosition.z = 10;
        _map.transform.position = new Vector3(_map.transform.position.x, _map.transform.position.y,10f);
    }

    void OnDestroy()
    {
        InputHandler.OnZoom -= this.Zoom;               // Unsubscribe from Zoom Event
        InputHandler.OnTouchMove += this.OnTouchMove;   // Unsubscribe from OnTouchMove Event
    }
}


