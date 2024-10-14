using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIManager
{
    private int _order = 10;
    private Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
    private UI_Scene _sceneUI = null;

    public List<UIBase> UIlist = new List<UIBase>();
    // Store에서 사용
    public List<Item> storeSaveItems;
    public Canvas changeCanvas;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UIRoot");
            if (root == null)
            {
                root = new GameObject { name = "@UIRoot" };
            }
            return root;
        }
    }

    /// <summary>
    /// order 변경을 위해 canvas에게 요청
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="sort"></param>
    public void SetCanvas(GameObject gameObject, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;
        canvas.overrideSorting = true; //중첩 캔버스 내에게 본인 order에만 적용
        gameObject.GetComponent<UIBase>().canvas = canvas;
        //changeCanvas = canvas;

        // 해상도 대응을 위한 컴포넌트 추가
        CanvasScaler canvasScaler = Util.GetOrAddComponent<CanvasScaler>(gameObject);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        Util.GetOrAddComponent<GraphicRaycaster>(gameObject);

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = -20;
        }
    }

    public T MakeItems<T>(Transform parent = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Item/{name}");

        if (parent != null)
            go.transform.SetParent(parent);
        
        return go.GetOrAddComponent<T>(); 
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;
        UIlist.Add(sceneUI);

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowHUD<T>(string name = null) where T : UI_HUD
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/HUD/{name}");

        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;
        UIlist.Add(sceneUI);

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_PopUp popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void ClosePopupUI(UI_PopUp uI_PopUp)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != uI_PopUp) // Peek : 마지막꺼 확인
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    //public T ToggleUI<T>() where T : UIBase
    //{
    //    T ui = FindUI<T>();
    //    if (ui != null)
    //    {
    //        FindUI<T>().gameObject.SetActive(!FindUI<T>().gameObject.activeSelf);
    //        return ui;
    //    }
    //    else
    //        return null;
    //}

    public T FindUI<T>() where T : UIBase
    {
        return UIlist.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
    }

    public T FindPopUp<T>() where T : UI_PopUp
    {
        return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
    }


    //public T PeekPopupUI<T>() where T : UI_PopUp
    //{
    //    if (_popupStack.Count == 0)
    //        return null;

    //    return _popupStack.Peek() as T;
    //}

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
