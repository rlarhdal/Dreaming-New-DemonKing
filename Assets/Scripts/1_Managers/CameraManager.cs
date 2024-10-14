using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    // cams
    [HideInInspector] public CinemachineVirtualCamera[] virtualCams;
    [HideInInspector] public CinemachineVirtualCamera playerCam;
    [HideInInspector] public CinemachineVirtualCamera bossCam;

    private Player player;

    public void Init()
    {
        player = Managers.Game.player;
        virtualCams = new CinemachineVirtualCamera[4];
        playerCam = player.GetComponentInChildren<CinemachineVirtualCamera>();
    }
}
