using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    private AudioSource _audioSource;
    private AudioClip _bonusClip;
    private Recording _recording;

    private const int RATIO_HIGHT = 8;

    private const string DIMENSION_STRING_FORMAT = "0.00";

    private int surfaceMask = 1 << 9;

    private Ray playerRay = new Ray();

    private TMP_Text _textHeight;

    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public bool isWork;

    private string _lastHeightValue = "";

    private DroneMovement droneMovement;

    [SerializeField] private bool calculateHight = true;

    public int bonusCount { get; private set; } = 0;
    public int incisionCount { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        droneMovement = GetComponent<DroneMovement>();
        _audioSource = GetComponentInChildren<AudioSource>();
        _bonusClip = Resources.Load("audios/bonus_clip") as AudioClip;
        _recording = FindObjectOfType<Recording>();
        _textHeight = GameObject.Find("textHight").GetComponent<TMP_Text>();
        isWork = true; //It was false
        _audioSource.enabled = true;

    }

    public void IncrementIncision()
    {
        incisionCount += 1;
    }

    public void TurnOn()
    {
        _audioSource.enabled = true;
        isWork = true;
    }

    public void TurnOff()
    {
        _audioSource.enabled = false;
        isWork = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWork) return;

        if (Input.anyKey)
        {
            if (_recording.isReading)
            {
                _recording.SkipRepeating();
            }
        }
        if(calculateHight)
        FindHight();

    }
    private void FindHight()
    {
        //setting ray, direction -> down
        playerRay.origin = transform.position;
        playerRay.direction = transform.up * -1;

        Debug.DrawRay(transform.position, transform.forward, Color.yellow);

        RaycastHit hit;

        //We take into account only the 9th layer using surfaceMask
        if (Physics.Raycast(playerRay, out hit, Mathf.Infinity, surfaceMask))
        {
            //Updating the text without unnecessary renderings
            string textHeight = ConvertHeightToText(hit.point);
            if (textHeight != _lastHeightValue) _textHeight.text = textHeight;
            _lastHeightValue = textHeight;
        }
        else
        {
            Debug.LogError("We can not be out of the map! Fix it");
        }
    }

    private string ConvertHeightToText(Vector3 point)
    {
        //It'll be the final text of the height
        string formatHeight = "";

        //height in metters
        float heightM = MathF.Round(Vector3.Distance(gameObject.transform.position, point) / RATIO_HIGHT, 2);

        //dimension type (metters or santimetrs or kilometrs)
        string dimensionType = "m";

        //height in from (default in metters)
        float floatFormatHeight = heightM;

        //translate into sm and km
        if (heightM < 1)
        {
            dimensionType = "sm";
            floatFormatHeight = (heightM * 100f);
        }
        else if (heightM > 1000)
        {
            dimensionType = "km";
            floatFormatHeight = (heightM / 1000f);
        }

        //we show decimal places only for the meter
        formatHeight = dimensionType == "sm" ? floatFormatHeight.ToString() : floatFormatHeight.ToString(DIMENSION_STRING_FORMAT);

        //format text =)
        return $"current height: {formatHeight}({dimensionType})";
    }



    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (!isWork) return;
        Debug.Log("asdas " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Bonuses")
        {
            Debug.Log("isBonus");
            MonoBehaviour monoBehavior = collision.gameObject.GetComponent<MonoBehaviour>();
            if (monoBehavior is Bonus)
            {
                _audioSource.PlayOneShot(_bonusClip);
                (monoBehavior as Bonus).Take(gameObject);
                bonusCount += 1;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        incisionCount += 1;
    }

}
