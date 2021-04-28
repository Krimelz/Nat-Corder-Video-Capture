using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatSuite.Recorders;
using NatSuite.Recorders.Inputs;
using NatSuite.Recorders.Clocks;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;

[RequireComponent(typeof(Camera))]
public class CameraRecorder : MonoBehaviour
{
    public Vector2Int resolution;
    public float fps;

    new private Camera camera;
    private CameraInput cameraInput;
    private MP4Recorder recorder;
    private RealtimeClock clock;

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
        return await recorder.FinishWriting();
    }

    public string[] GetAllVideoNames()
    {
#if UNITY_EDITOR
        string dataPath = Application.dataPath;
        dataPath = dataPath.Substring(0, dataPath.Length - "/Assets".Length);
        return Directory.GetFiles(dataPath, "*.mp4");
#elif UNITY_IOS || UNITY_ANDROID
        return Directory.GetFiles(Application.persistentDataPath, "*.mp4");
#endif
    }
}
