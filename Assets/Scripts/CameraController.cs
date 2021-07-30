using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public float sensitivity = .01f;

    private CinemachineComposer composer;
    /*private float minimumY = -90f;
    private float maximumY = 90f;
    private float minimumX = -90f;
    private float maximumX = 90f;*/

    public float vertical;
    public bool isZoom;

    new Camera camera;

    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        composer.m_TrackedObjectOffset.y += vertical * sensitivity;
        composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, -10, 10);
    }

    public void zoomIn()
    {
        isZoom = true;
        camera.fieldOfView = 25f;
    }

    public void zoomOut()
    {
        isZoom = false;
        camera.fieldOfView = 60f;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
