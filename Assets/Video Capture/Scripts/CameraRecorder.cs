using UnityEngine;
using NatSuite.Recorders;
using NatSuite.Recorders.Inputs;
using NatSuite.Recorders.Clocks;
using System.Threading.Tasks;
using System.IO;
using System;

[RequireComponent(typeof(Camera))]
public class CameraRecorder : MonoBehaviour
{
    public Vector2Int resolution;
    public float fps;

    new private Camera camera;
    private CameraInput cameraInput;
    private MP4Recorder recorder;
    private RealtimeClock clock;

    private string savePath;

    private void Awake()
    {
#if UNITY_EDITOR
        savePath = Application.persistentDataPath;
#elif UNITY_IOS || UNITY_ANDROID
        savePath = Application.persistentDataPath;
#endif
    }

    private void Start()
    {
        
        camera = GetComponent<Camera>();
    }

    public void StartRecording()
    {
        recorder = new MP4Recorder(resolution.x, resolution.y, fps);
        clock = new RealtimeClock();
        cameraInput = new CameraInput(recorder, clock, camera);
    }

    public async Task<string> StopRecording()
    {
        cameraInput.Dispose();

        string path = await recorder.FinishWriting();
        
        return path;
    }

    public string GetAllVideosJson()
    {
        VideoList list = new VideoList();
        list.videos = Directory.GetFiles(savePath, "*.mp4");

        return JsonUtility.ToJson(list, true);
    }
}

[Serializable]
public class VideoList
{
    public string[] videos;
}
