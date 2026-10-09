using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraBoundaries : MonoBehaviour
{
    [Header("Boundary Settings")]
    [SerializeField] private float wallThickness = 2.0f;
    [SerializeField] private float minHorizontalExtent = 8.89f; // Standard 16:9 half-width for ortho size 5

    private Camera cam;
    private Transform boundariesContainer;
    private BoxCollider2D leftWall;
    private BoxCollider2D rightWall;
    private BoxCollider2D topWall;
    private PhysicsMaterial2D frictionlessMaterial;

    private float lastAspect;
    private float lastOrthoSize;
    private Vector3 lastCamPos;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        EnsureMaterial();
        UpdateBoundaries();
    }

    private void Start()
    {
        EnsureMaterial();
        UpdateBoundaries();
    }

    private void Update()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (cam == null) return;

        // Recalculate if camera parameters or aspect ratio change
        if (cam.aspect != lastAspect || cam.orthographicSize != lastOrthoSize || cam.transform.position != lastCamPos)
        {
            UpdateBoundaries();
        }
    }

    private void EnsureMaterial()
    {
        if (frictionlessMaterial == null)
        {
            frictionlessMaterial = new PhysicsMaterial2D("FrictionlessBoundary")
            {
                friction = 0f,
                bounciness = 0f
            };
        }
    }

    public void UpdateBoundaries()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic) return;

        lastAspect = cam.aspect;
        lastOrthoSize = cam.orthographicSize;
        lastCamPos = cam.transform.position;

        EnsureMaterial();

        // Find or create child container for boundaries
        boundariesContainer = transform.Find("ScreenBoundaries");
        if (boundariesContainer == null)
        {
            GameObject containerObj = new GameObject("ScreenBoundaries");
            containerObj.transform.SetParent(transform);
            containerObj.transform.localPosition = Vector3.zero;
            containerObj.transform.localRotation = Quaternion.identity;
            containerObj.transform.localScale = Vector3.one;
            boundariesContainer = containerObj.transform;
        }

        float vertExtent = cam.orthographicSize;
        float horizExtent = Mathf.Max(vertExtent * cam.aspect, minHorizontalExtent);
        Vector3 camPos = cam.transform.position;
        float height = (vertExtent * 2f) + 20f;

        // 1. Left Wall (blocks moving off-screen to the left)
        leftWall = EnsureWallComponent("LeftWall");
        leftWall.transform.position = new Vector3(camPos.x - horizExtent - (wallThickness / 2f), camPos.y, 0f);
        leftWall.size = new Vector2(wallThickness, height);
        leftWall.sharedMaterial = frictionlessMaterial;

        // 2. Right Wall (blocks moving off-screen to the right)
        rightWall = EnsureWallComponent("RightWall");
        rightWall.transform.position = new Vector3(camPos.x + horizExtent + (wallThickness / 2f), camPos.y, 0f);
        rightWall.size = new Vector2(wallThickness, height);
        rightWall.sharedMaterial = frictionlessMaterial;

        // 3. Top Wall / Ceiling (blocks jumping off-screen above)
        topWall = EnsureWallComponent("TopWall");
        topWall.transform.position = new Vector3(camPos.x, camPos.y + vertExtent + (wallThickness / 2f), 0f);
        topWall.size = new Vector2((horizExtent * 2f) + (wallThickness * 2f), wallThickness);
        topWall.sharedMaterial = frictionlessMaterial;
    }

    private BoxCollider2D EnsureWallComponent(string wallName)
    {
        Transform child = boundariesContainer.Find(wallName);
        GameObject wallObj;
        if (child == null)
        {
            wallObj = new GameObject(wallName);
            wallObj.transform.SetParent(boundariesContainer);
            wallObj.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            wallObj = child.gameObject;
        }

        BoxCollider2D col = wallObj.GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = wallObj.AddComponent<BoxCollider2D>();
        }
        col.isTrigger = false;
        return col;
    }

    private void OnDrawGizmos()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic) return;

        float vertExtent = cam.orthographicSize;
        float horizExtent = Mathf.Max(vertExtent * cam.aspect, minHorizontalExtent);
        Vector3 camPos = cam.transform.position;

        Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
        Gizmos.DrawWireCube(new Vector3(camPos.x, camPos.y, 0f), new Vector3(horizExtent * 2f, vertExtent * 2f, 0f));
    }
}
