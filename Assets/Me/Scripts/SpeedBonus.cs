using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : MonoBehaviour, Bonus
{
    private WaitForSeconds _returnFor;
    private MeshRenderer _meshRenderer;

    [SerializeField] private int plusSpeed = 5;


    private void Start()
    {
        _returnFor = new WaitForSeconds(5);
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Take(GameObject drone)
    {
        _meshRenderer.enabled = false;

        DroneMovement droneMovement = drone.GetComponent<DroneMovement>();

        StartCoroutine(SettingSpeed(droneMovement));
    }

    IEnumerator SettingSpeed(DroneMovement droneMovement)
    {
        ChangeForwardSidewaySpeed(droneMovement, plusSpeed);

        yield return _returnFor;

        ChangeForwardSidewaySpeed(droneMovement, -plusSpeed);

        Destroy(gameObject);
    }

    /// <summary>
    /// adds/decreases the speed value depending on the [speed] sign.
    /// </summary>
    /// <param name="droneMovement">The main script which has a drone speed.</param>
    /// <param name="speed">Drone speed, if speed < 0, then speed decreases, and on the contrary for > 0</param>
    private void ChangeForwardSidewaySpeed(DroneMovement droneMovement, int speed)
    {
        droneMovement.profiles[droneMovement._profileIndex].maxForwardSpeed += speed;
        droneMovement.profiles[droneMovement._profileIndex].maxSidewaySpeed += speed;
    }

}