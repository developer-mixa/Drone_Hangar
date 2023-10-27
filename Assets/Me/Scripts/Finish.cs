using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using TMPro;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private GlobalTimer stopwatch;
    private Recording recording;
    public bool isFinish { get; private set; } = false;

    private GameObject canvas;

    private CameraController cameraController;

    [SerializeField] private TMP_Text[] texts;

    private GameObject textHightObject;


    private void Start()
    {
        stopwatch = FindObjectOfType<GlobalTimer>();
        recording = FindObjectOfType<Recording>();
        canvas = GameObject.Find("Canvas");
        textHightObject = GameObject.Find("textHight");


        cameraController = FindObjectOfType<CameraController>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isFinish) return;
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("sdnfi ashfiusdfis bfihsbd hbdfhgbd fgfkd ");
            StartCoroutine(FinishGame());
        }
    }

    private IEnumerator FinishGame()
    {
        textHightObject.SetActive(false);
        var playerSource = cameraController.currentPlayer.gameObject.GetComponentInChildren<AudioSource>();
        playerSource.Stop();
        stopwatch.StopTimer();
        recording.StopRecording();
        canvas.GetComponent<Animator>().SetBool("isFade", true);
        isFinish = true;
        SettingStats();
        yield return new WaitForSeconds(6.50f);
        ShowRepeating();
        playerSource.Play();
    }

    //Animator calls this method ---
    public void ShowRepeating()
    {
        recording.Read();
    }

    private void SettingStats()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            string name = texts[i].gameObject.name;
            string text = texts[i].text;
            Debug.Log(name);
            switch (name)
            {

                case "GeneralTime":
                    text = $"Общее время: {stopwatch.GetCurrentTime()} (сек)";
                    break;

                case "CountDestroyingDrone":
                    text = $"Дрон врезался: {cameraController.currentPlayer.incisionCount} раз";
                    break;

                case "GotBonuses":
                    text = $"Собрано бонусов: {cameraController.currentPlayer.bonusCount}";
                    break;
            }
            texts[i].text = text;

        }
    }

}
