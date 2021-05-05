using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoCaptureController : MonoBehaviour
{
    public CameraRecorder cameraRecorder;
    public Button startButton;
    public Button stopButton;
    public Transform content;
    public GameObject pathPrefab;

    private void Start()
    {
        LoadVideoNamesFromJson();
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

        AddVideoNameToList(path);
        EnableStartButton();
    }

    private void LoadVideoNamesFromJson()
    {
        VideoList list = JsonUtility.FromJson<VideoList>(cameraRecorder.GetAllVideosJson());

        foreach (string path in list.videos)
        {
            AddVideoNameToList(path);
        }
    }

    private void AddVideoNameToList(string path)
    {
        GameObject go = Instantiate(pathPrefab, content);
        go.GetComponentInChildren<Text>().text = path;
    }

    private void EnableStartButton()
    {
        startButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
    }

    private void EnableStopButton()
    {
        startButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
    }
}
