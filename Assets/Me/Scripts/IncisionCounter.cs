using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncisionCounter : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<CameraController>().currentPlayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        _player.IncrementIncision();
    }
}
