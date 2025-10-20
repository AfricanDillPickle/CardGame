using UnityEngine;
using UnityEngine.EventSystems;

public class cameraManager : MonoBehaviour
{
    public Camera mainCamera;

    public float tweenSpeed = 2f;

    private Vector3 targetPos;
    private Quaternion targetRot;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        targetPos = mainCamera.transform.position;
        targetRot = mainCamera.transform.rotation;
    }

    void Update()
    {
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position, targetPos, Time.deltaTime * tweenSpeed);

        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation, targetRot, Time.deltaTime * tweenSpeed);
    }

    public void lookDown()
    {
        targetPos = new Vector3(0f, 2.5f, 0f);
        targetRot = Quaternion.Euler(90f, 90f, 0f);
    }

    public void lookUp()
    {
        targetPos = new Vector3(-2.47f, 0.64f, 0f);
        targetRot = Quaternion.Euler(0f, 90f, 0f);
    }

    public void lookJury()
    {
        targetPos = new Vector3(-2.95f, 0.64f, 0.57f);
        targetRot = Quaternion.Euler(0f, 128.41f, 0f);
    }

    public void lookBack()
    {
        targetPos = new Vector3(-2.47f, 0.64f, 0f);
        targetRot = Quaternion.Euler(0f, 90f, 0f);
    }
}
