
using System.Linq;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    private GameObject[] rockets;
    private CameraController cameraController;
    [SerializeField] private float detectedZone;
    [SerializeField] private Transform rotation_pvo;

    void Start()
    {
        rockets = GameObject.FindGameObjectsWithTag("grad_rockets");
        cameraController = SecureGameObject.FindObjectOfType<CameraController>();
        rotation_pvo = GameObject.Find("rotation_rocket").GetComponent<Transform>();
    }

    private void WaitDrone()
    {
        if (cameraController == null) {
            return;
        }

        float distance = Vector3.Distance(gameObject.transform.localPosition, cameraController.currentPlayer.transform.localPosition);
        if (distance < detectedZone) {
            TurnToDrone();
        }
    }

    private void TurnToDrone()
    {
        Debug.Log($"turn: {rotation_pvo.gameObject.name}");
        Vector3 targetRotation = new Vector3(rotation_pvo.position.x, rotation_pvo.position.y, cameraController.currentPlayer.transform.position.z);
        rotation_pvo.LookAt(targetRotation);
    }

    // Update is called once per frame
    void Update()
    {
        WaitDrone();   
    }
}
