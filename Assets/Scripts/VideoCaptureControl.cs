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
        VideoList list = JsonUtility.FromJson<VideoList>(cameraRecorder.GetAllVideoNamesJson());

        foreach (string path in list.videoNames)
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
        start.gameObject.SetActive(true);
        stop.gameObject.SetActive(false);
    }

    private void EnableStopButton()
    {
        start.gameObject.SetActive(false);
        stop.gameObject.SetActive(true);
    }
}
