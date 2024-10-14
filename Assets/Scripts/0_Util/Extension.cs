using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Extension 함수 = this 를 붙임으로써 간접적으로 추가된 메서드와 원래의 메서드를 이어줌
/// 구현 내용은 중복 x
/// </summary>
public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
    {
        UIBase.BindEvent(go, action, type);
    }
}