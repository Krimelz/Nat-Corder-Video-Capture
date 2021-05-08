using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoCaptureController : MonoBehaviour
{
    public CameraRecorder cameraRecorder;
    public Button startButton;
    public Button stopButton;
    public Transform content;
    public GameObject pathPrefab;
    public Text pathText;

    private void Start()
    {
        pathText.text = Application.persistentDataPath;
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

        var permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write);

        if (permission == NativeGallery.Permission.Granted)
        {
            NativeGallery.SaveVideoToGallery(path, "Videos", DateTime.Now.Ticks.ToString() + ".mp4");
        }

        AddVideoToList(path);
        //SendVideoJson();
        EnableStartButton();
    }

    public void SendVideoJson()
    {
        string json = cameraRecorder.GetAllVideosJson();
        DeviceMessenger.SendMessageToDevice(json);
    }

    private void LoadVideoNamesFromJson()
    {
        VideoList list = JsonUtility.FromJson<VideoList>(cameraRecorder.GetAllVideosJson());

        foreach (string path in list.videos)
        {
            AddVideoToList(path);
        }
    }

    private void AddVideoToList(string path)
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
