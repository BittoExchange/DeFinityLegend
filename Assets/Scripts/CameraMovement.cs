using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public Camera mainCamera;
    public SpriteRenderer map;
    public float zoomSpeed;
    public float moveSpeed;
    public float minZoom;
    public float maxZoom;
    public float momentum;
    public float zoomMomentum;
    public float zoomOffset;

    Vector2 delta;
    float velocity;
    float oldDistance;
    float zoom;
    float mapMinX, mapMaxX, mapMinY, mapMaxY;
    bool fingerChanged = true;
    Vector2 lastPos;

    public static CameraMovement Instance;

    void Awake()
    {
        Instance = this;

        mapMinX = map.transform.position.x - map.bounds.size.x / 2;
        mapMaxX = map.transform.position.x + map.bounds.size.x / 2;

        mapMinY = map.transform.position.y - map.bounds.size.y / 2;
        mapMaxY = map.transform.position.y + map.bounds.size.y / 2;

        zoom = mainCamera.orthographicSize;
    }

    void Update()
    {
        Movement();
        Zoom();
    }

    void Movement()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount == 1 && !IsPointerOverUI())
            {
                if (fingerChanged)
                {
                    lastPos = Input.GetTouch(0).position;
                    fingerChanged = false;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    delta = Input.GetTouch(0).position - lastPos;
                    velocity = delta.magnitude;
                    delta = Vector3.ClampMagnitude(delta, 1);
                    lastPos = Input.GetTouch(0).position;
                }
            }
            else
            {
                fingerChanged = true;
            }
        }
        else if(!IsPointerOverUI())
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                delta = (Vector2)Input.mousePosition - lastPos;
                velocity = delta.magnitude;
                delta = Vector3.ClampMagnitude(delta, 1);
                lastPos = Input.mousePosition;
            }
        }

        velocity = Mathf.Lerp(velocity, 0, momentum * Time.deltaTime);
        transform.position += moveSpeed * velocity * Time.deltaTime * (delta.x * Vector3.left + delta.y * Vector3.down);
        transform.position = ClampCamera(transform.position);
    }

    void Zoom()
    {
        if (Input.touchSupported && Input.touchCount >= 2 && !IsPointerOverUI())
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                oldDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                float newDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                if (oldDistance == 0)
                {
                    oldDistance = newDistance;
                }

                float velocity = Mathf.Abs(newDistance - oldDistance);

                if (newDistance > oldDistance)
                {
                    zoom -= zoomSpeed * Time.deltaTime * velocity;
                }
                else if (newDistance < oldDistance)
                {
                    zoom += zoomSpeed * Time.deltaTime * velocity;
                }

                oldDistance = newDistance;
            }
        }
        else
        {
            zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        }

        if(zoom > maxZoom - zoomOffset)
        {
            zoom = Mathf.Lerp(zoom, maxZoom - zoomOffset, zoomMomentum * Time.deltaTime);
        }
        else if(zoom < minZoom + zoomOffset)
        {
           zoom = Mathf.Lerp(zoom, minZoom + zoomOffset, zoomMomentum * Time.deltaTime);
        }

        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoom, zoomSpeed * Time.deltaTime);
    }

    Vector3 ClampCamera(Vector3 position)
    {
        float height = mainCamera.orthographicSize;
        float width = mainCamera.orthographicSize * mainCamera.aspect;

        float minX = mapMinX + width;
        float maxX = mapMaxX - width;
        float minY = mapMinY + height;
        float maxY = mapMaxY - height;

        float newX = Mathf.Clamp(position.x, minX, maxX);
        float newY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector3(newX, newY, position.z);
    }

    public bool IsPointerOverUI()
    {
        if (!Input.touchSupported)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        if (Input.touchCount == 0)
        {
            return false;
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
