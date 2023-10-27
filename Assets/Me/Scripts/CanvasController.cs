using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private Finish finish;
    void Start()
    {
        finish = FindObjectOfType<Finish>();
    }

    //Animation event calls this method
    public void ShowRepeating() { }
}
