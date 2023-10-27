using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Recording : MonoBehaviour
{
    private bool isRecording = false;
    public bool isReading { get; private set; } = false;

    private CameraController cameraController;

    private MemoryStream memoryStream;
    private BinaryWriter binaryWriter;
    private BinaryReader binaryReader;

    [SerializeField] private GameObject recordingPanel;

    private Dictionary<string, bool> animationBools = new Dictionary<string, bool>();

    private void Awake()
    {
        memoryStream = new MemoryStream(); //—оздает поток, резервным хранилищем которого €вл€етс€ пам€ть.
        binaryWriter = new BinaryWriter(memoryStream, Encoding.ASCII); //«аписывает примитивные типы в двоичный поток и поддерживает запись строк в заданной кодировке.
        binaryReader = new BinaryReader(memoryStream, Encoding.ASCII);
        InitAnimationBools();

    }

    private void InitAnimationBools()
    {
        animationBools["idle"] = false;
        animationBools["twirl_left"] = false;
        animationBools["twirl_right"] = false;
        animationBools["left_passage"] = false;
        animationBools["right_passage"] = false;
    }

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        StartRecording();
    }

    public void StartRecording()
    {

        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream, Encoding.ASCII);
        binaryReader = new BinaryReader(memoryStream, Encoding.ASCII);

        memoryStream.SetLength(0);
        ResetReplayFrame();
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    //«аписывает примитивные типы в двоичный поток и поддерживает запись строк в заданной кодировке.
    private void ResetReplayFrame() //сброс кадра воспроизведени€
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        binaryWriter.Seek(0, SeekOrigin.Begin);
    }

    public void Read()
    {
        Debug.Log("READ");
        ResetReplayFrame();
        cameraController.currentPlayer.GetComponent<DroneMovement>().animatedGameObject = null;
        isReading = true;
        recordingPanel.SetActive(true);
    }


    private void TryReading()
    {
        if (isReading)
        {
            if (binaryReader.PeekChar() > -1)
            {
                float pos_x = binaryReader.ReadSingle();
                float pos_y = binaryReader.ReadSingle();
                float pos_z = binaryReader.ReadSingle();

                bool idle = binaryReader.ReadBoolean();
                bool twirllLeft = binaryReader.ReadBoolean();
                bool twirlRight = binaryReader.ReadBoolean();
                bool leftPassage = binaryReader.ReadBoolean();
                bool rightPassage = binaryReader.ReadBoolean();

                float rot_x = binaryReader.ReadSingle();
                float rot_y = binaryReader.ReadSingle();
                float rot_z = binaryReader.ReadSingle();

                OptimizedSetAnimation("idle", idle);
                OptimizedSetAnimation("twirl_left", twirllLeft);
                OptimizedSetAnimation("twirl_right", twirlRight);
                OptimizedSetAnimation("left_passage", leftPassage);
                OptimizedSetAnimation("right_passage", rightPassage);

                cameraController.currentPlayer.gameObject.transform.localPosition = new Vector3(pos_x, pos_y, pos_z);

                cameraController.currentPlayer.gameObject.transform.localRotation = Quaternion.Euler(rot_x, rot_y, rot_z);
            }
            else
            {
                isReading = false;
                recordingPanel.SetActive(false);
                isReading = false;
                TakeVictory();
            }
        }
    }

    private void TryWriting()
    {

        Vector3 playerPosition = cameraController.currentPlayer.gameObject.transform.localPosition;


        //Write positions -------------------------------------------------------

        float position_x = playerPosition.x;
        float position_y = playerPosition.y;
        float position_z = playerPosition.z;

        binaryWriter.Write(position_x);
        binaryWriter.Write(position_y);
        binaryWriter.Write(position_z);


        //Write animations -------------------------------------------------------

        var droneAnimator = cameraController.currentPlayer.GetComponentInChildren<Animator>();

        binaryWriter.Write(droneAnimator.GetBool("idle"));

        binaryWriter.Write(droneAnimator.GetBool("twirl_left"));
        binaryWriter.Write(droneAnimator.GetBool("twirl_right"));
        binaryWriter.Write(droneAnimator.GetBool("left_passage"));
        binaryWriter.Write(droneAnimator.GetBool("right_passage"));

        //Write rotations -------------------------------------------------------
        Vector3 playerRotation = cameraController.currentPlayer.gameObject.transform.eulerAngles;

        float rotation_x = playerRotation.x;
        float rotation_y = playerRotation.y;
        float rotation_z = playerRotation.z;

        binaryWriter.Write(rotation_x);
        binaryWriter.Write(rotation_y);
        binaryWriter.Write(rotation_z);
    }

    private void FixedUpdate()
    {
        //If read == true we are reading coordinations
        TryReading();

        //If it's not recording we must not write coordinations
        if (!isRecording) return;

        if (cameraController.currentPlayer == null)
        {
            Debug.LogError("Player is not found. Init this");
            return;
        }

        //Just write coordinations
        TryWriting();

    }

    public void SkipRepeating() => TakeVictory();

    private void TakeVictory()
    {
        ResetReplayFrame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OptimizedSetAnimation(string boolName, bool value)
    {

        if (animationBools[boolName] != value)
        {
            var droneAnimator = cameraController.currentPlayer.GetComponentInChildren<Animator>();

            droneAnimator.SetBool(boolName, value);
            animationBools[boolName] = value;
        }

    }

}
