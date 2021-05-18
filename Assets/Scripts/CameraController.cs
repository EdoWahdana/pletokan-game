using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float sensitivity = .5f;

    private CinemachineComposer composer;
    private float minimumY = -90f;
    private float maximumY = 90f;
    private float minimumX = -90f;
    private float maximumX = 90f;

    public float vertical;

    void Start()
    {
        composer = GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        composer.m_TrackedObjectOffset.y += vertical * sensitivity;
        composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, -10, 10);
    }
}
