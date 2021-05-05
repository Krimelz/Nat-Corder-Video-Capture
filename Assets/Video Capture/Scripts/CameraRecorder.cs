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

#if UNITY_EDITOR
    private string savePath = "D:/";
#elif UNITY_ANDROID
    private string savePath = Application.persistentDataPath;
#elif UNITY_IOS
    private string savePath = Application.persistentDataPath;
#endif

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
        string newPath = Path.Combine(savePath, GetVideoName(path));

        File.Move(path, newPath);

        return newPath;
    }

    public string GetAllVideosJson()
    {
        VideoList list = new VideoList();
        list.videos = Directory.GetFiles(savePath, "*.mp4");

        return JsonUtility.ToJson(list, true);
    }

    private string GetVideoName(string path)
    {
        int lastIndex = path.LastIndexOf('\\');
        int length = path.Length - lastIndex - 1;

        string videoName = path.Substring(lastIndex + 1, length);

        return videoName;
    }
}

[Serializable]
public class VideoList
{
    public string[] videos;
}
