
using UnityEngine;
using Cinemachine;

public class TutorialCamChange : TutorialBase
{
    enum CurrentCam
    {
        Player,
        Merchant,
        Anvil,
        Digger,
        Portal,
        Boss
    }

    enum ChangeCam
    {
        Player,
        Merchant,
        Anvil,
        Digger,
        Portal,
        Boss
    }

    [SerializeField]
    CurrentCam current;
    [SerializeField]
    ChangeCam change;

    private CinemachineVirtualCamera currentCam;
    private CinemachineVirtualCamera changeCam;


    public override void Enter()
    {
        switch (current)
        {
            case CurrentCam.Player:
                currentCam = Managers.Camera.playerCam; break;
            case CurrentCam.Merchant:
                currentCam = Managers.Camera.virtualCams[3]; break;
            case CurrentCam.Anvil:
                currentCam = Managers.Camera.virtualCams[0]; break;
            case CurrentCam.Portal:
                currentCam = Managers.Camera.virtualCams[2]; break;
            case CurrentCam.Digger:
                currentCam = Managers.Camera.virtualCams[1]; break;
            case CurrentCam.Boss:
                currentCam = Managers.Camera.bossCam; break;
        }

        switch (change)
        {
            case ChangeCam.Player:
                changeCam = Managers.Camera.playerCam; break;
            case ChangeCam.Merchant:
                changeCam = Managers.Camera.virtualCams[3]; break;
            case ChangeCam.Anvil:
                changeCam = Managers.Camera.virtualCams[0]; break;
            case ChangeCam.Portal:
                changeCam = Managers.Camera.virtualCams[2]; break;
            case ChangeCam.Digger:
                changeCam = Managers.Camera.virtualCams[1]; break;
            case ChangeCam.Boss:
                changeCam = Managers.Camera.bossCam; break;
        }
    }

    public override void Execute(TutorialController controller)
    {
        if (ActiveCam())
        {
            controller.SetNextTutorial();
        }
    }

    private bool ActiveCam()
    {
        changeCam.gameObject.SetActive(true);
        currentCam.gameObject.SetActive(false);

        return true;
    }

    public override void Exit()
    {

    }
}
