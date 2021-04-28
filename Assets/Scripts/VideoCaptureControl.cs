using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoCaptureControl : MonoBehaviour
{
    public CameraRecorder cameraRecorder;
    public Button start;
    public Button stop;
    public Transform content;
    public GameObject pathPrefab;

    public Text videoCount;

    private void Start()
    {
        EnableStartButton();
    }

    public void StartRecording()
    {
        cameraRecorder.StartRecording();
        EnableStopButton();
    }

    public async void StopRecording()
    {
        string path = await cameraRecorder.StopRecording();

        GameObject go = Instantiate(pathPrefab, content);
        go.GetComponentInChildren<Text>().text = path;

        EnableStartButton();
        SetPathesText();
    }

    private void SetPathesText()
    {
        videoCount.text = cameraRecorder.GetAllVideoNames().Length.ToString();
    }

    private void EnableStartButton()
    {
        start.gameObject.SetActive(true);
        stop.gameObject.SetActive(false);
    }

    private void EnableStopButton()
    {
        start.gameObject.SetActive(false);
        stop.gameObject.SetActive(true);
    }
}
