#if UNITY_IOS
using System.Runtime.InteropServices;
#endif
using System;
using UnityEngine;

public static class DeviceMessenger
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void SendMessageToIOS(string message);
#endif

    public static void SendMessageToDevice(string json)
    {
        Debug.Log(json);

#if UNITY_ANDROID
        AndroidJavaClass instance = new AndroidJavaClass("ai.replika.unity.bridge.UnityBridge");
        instance.CallStatic("sendMessageToAndroid", json);
#endif

#if UNITY_IOS && !UNITY_EDITOR
        SendMessageToIOS(json);
#endif
    }
}