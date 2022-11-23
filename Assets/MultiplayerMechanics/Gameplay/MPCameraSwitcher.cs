using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public static class MPCameraSwitcher
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera ActiveCamera = null;
    public static void SwitchCamera(CinemachineVirtualCamera camera){
        camera.Priority = 10;
        ActiveCamera = camera;
    }
    public static void Register(CinemachineVirtualCamera camera){
        cameras.Add(camera);
        
    }
    public static void Unegister(CinemachineVirtualCamera camera){
        cameras.Remove(camera);
    }
}
