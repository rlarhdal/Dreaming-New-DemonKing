using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EventScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = SceneType.EndingScene;

        Managers.Camera.bossCam = GameObject.Find("BossCam").GetComponent<CinemachineVirtualCamera>();
        Managers.Camera.bossCam.gameObject.SetActive(false);

        // Create Player
        GameObject player = Managers.Game.player.gameObject;
        Managers.Camera.playerCam = player.GetComponentInChildren<CinemachineVirtualCamera>();

        player.transform.position = new Vector3(-3, 0, 0);
        GameObject obj = player.GetComponentInChildren<Animator>().gameObject;
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);

        Managers.Direction.PlayerAction(false);
    }

    public override void Clear()
    {

    }
}
