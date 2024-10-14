using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogSystem))]
public class TutorialDialog : TutorialBase
{
    // ĳ���͵��� ��縦 �����ϴ� DialogSystem
    private DialogSystem dialogSystem;

    public override void Enter()
    {
        dialogSystem = GetComponent<DialogSystem>();
        dialogSystem.Setup();
    }

    public override void Execute(TutorialController controller)
    {
        // ���� �б⿡ ����Ǵ� ��� ����
        bool isCompleted = dialogSystem.UpdateDialog();

        // ���� �б��� ��� ������ �Ϸ�Ǹ�
        if (isCompleted)
        {
            Managers.Sound.PlayShot(Managers.Sound.sfxClips.clips[(int)SFXClip.Button]);
            // ���� Ʃ�丮��� �̵�
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
