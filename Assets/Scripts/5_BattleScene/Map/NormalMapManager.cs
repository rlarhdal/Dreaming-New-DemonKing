using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum mapObject
{
    escapePortal,
    nextPortal
}

public class NormalMapManager : MonoBehaviour
{
    private int count = 0;
    [SerializeField] private GameObject nextDoor;
    [SerializeField] private GameObject escapePortal;
    [SerializeField] private GameObject nextPortal;

    public void AddCount()
    {
        count++;
    }

    // 해당 맵의 몬스터의 처치시 개수를 확인한다.
    // 적의 개수가 0이면 (맵에 적이 아예 없다면) 맵 클리어가 되었다고 UI활성화
    public void SubtractCount()
    {
        count--;
        if (count == 0)
        {
            nextDoor.SetActive(false);
            Managers.UI.FindUI<UI_Battle>().SetClear();
            Managers.UI.ShowPopupUI<UI_StageClear>();
        }
    }

    // 원하는 포탈 정보를 얻을 수 있게 메서드 재활용
    public Vector3 GetPos(mapObject mapEnum)
    {
        switch (mapEnum)
        {
            case mapObject.escapePortal:
                return escapePortal.transform.position;
            case mapObject.nextPortal:
                return nextPortal.transform.position;
            default:
                return Vector3.zero;
        }
    }
}
