using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour, ITutorial
{
    // �ش� Ʃ�丮�� ������ ������ �� 1ȸ ȣ��
    public abstract void Enter();

    // �ش� Ʃ�丮�� ������ �����ϴ� ���� �� ������ ȣ��
    public abstract void Execute(TutorialController controller);

    //�ش� Ʃ�丮�� ������ ������ �� 1ȸ ȣ��
    public abstract void Exit();
}

public interface ITutorial
{
    public void Enter();
    public void Execute(TutorialController controller);
    public void Exit();
}