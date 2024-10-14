#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;


public class SceneChangeEditor : Editor
{
    // MenuItem�� ��ο� ditorSceneManager.OpenScene�� ��δ� �ڽ��� Scene ���� �̸����� �����Ѵ�.
    [MenuItem("SceneMove/StartScene &1")]
    private static void StartScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/StartScene.unity");
    }
    [MenuItem("SceneMove/Tutorial &2")]
    private static void TutorialScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Tutorial.unity");
    }
    [MenuItem("SceneMove/MaintenanceScene &3")]
    private static void MainScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MaintenanceScene.unity");
    }

    [MenuItem("SceneMove/TestScene &4")]
    private static void InGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/BattleScene.unity");
    }

    [MenuItem("SceneMove/ThrowingBallScene &5")]
    private static void ThrowingBallScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/ThrowingBallScene.unity");
        Debug.Log("ThrowingBallScene");
    }
}
#endif