using DroneController.CameraMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{

    private CameraScript cameraScript;

    private GlobalTimer timer;

    private Recording recording;

    private bool lastPicked = false;

    private Player[] players;

    public Player currentPlayer { get; private set; }

    void Start()
    {
        /*        recording = SecureGameObject.FindObjectOfType<Recording>();

                cameraScript = GetComponent<CameraScript>();
                SecureGameObject.CheckNull(cameraScript);

                players = FindObjectsOfType<Player>();

                timer = SecureGameObject.FindObjectOfType<GlobalTimer>();*/
        currentPlayer = SecureGameObject.FindObjectOfType<Player>(); 

    }

/*    // Update is called once per frame
    void Update()
    {
        if (cameraScript.pickedMyDrone != lastPicked)
        {
            if (cameraScript.pickedMyDrone && cameraScript.ourDrone != null) TurnOffOtherDrones(cameraScript.ourDrone.name);
        }
        lastPicked = cameraScript.pickedMyDrone;

    }*/

/*    private void TurnOffOtherDrones(string choosedDroneName)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].gameObject.name == choosedDroneName)
            {
                currentPlayer = players[i];
                recording.StartRecording();
                timer.StartTimer();
                players[i].GetComponent<Player>().TurnOn();

            }
            else players[i].GetComponent<Player>().TurnOff();
        }
    }

    private void SetBehindPosition(Vector3 newPosition)
    {
        if (newPosition != cameraScript.positionBehindDrone)
        {
            cameraScript.positionBehindDrone = newPosition;
        }
    }*/


}

