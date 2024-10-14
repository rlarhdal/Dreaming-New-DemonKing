using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public static string TargetScene { get; set; }
    [SerializeField] Camera cam;
    private Slider progressBar;
    private TextMeshProUGUI progressText;
    private float minimumLoadingTime = 2.0f;//로딩씬 지속 시간

    private void Start()
    {
//        if (GameObject.FindGameObjectWithTag(nameof(Player))== null)
//            cam.gameObject.GetComponent<AudioListener>().enabled = true;
//        else
//            cam.gameObject.GetComponent<AudioListener>().enabled = false;
        SetupUI();
        StartCoroutine(LoadAsyncScene());
    }

    private void SetupUI()
    {
        // 슬라이더와 텍스트를 찾아서 연결
        progressBar = GameObject.Find("LoadingBar").GetComponent<Slider>();
        progressText = GameObject.Find("LoadingTXT").GetComponent<TextMeshProUGUI>();
    }

    private IEnumerator LoadAsyncScene()
    {
        yield return null; // 한 프레임 대기

        float startTime = Time.time;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(TargetScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (progressText != null)
            {
                progressText.text = $"Loading... {progress * 100}%";
            }

            if (TargetScene == "BattleScene" && Managers.Sheet.enemys == null)
            {
                progressText.text = $"스테이지 정보 불러오는 중";
                yield return StartCoroutine(Managers.Sheet.LoadData(sheetType.Enemy));
            }

            if (asyncOperation.progress >= 0.9f)
             {
                progressText.text = $"로딩 완료";
                yield return new WaitForSeconds(0.5f);
                asyncOperation.allowSceneActivation = true;
             }
             // 그냥 게이지 차면 씬 넘어가는 코드

            //if (asyncOperation.progress >= 0.9f)
            //{
            //    // 최소 로딩 시간 대기
            //    float elapsedTime = Time.time - startTime;
            //    if (elapsedTime >= minimumLoadingTime)
            //    {
            //        progressText.text = "Press Any Key...";

            //        // 아무 키나 눌렀는지 확인
            //        if (Input.anyKeyDown)
            //        {
            //            Debug.Log("로딩 완료. 씬 전환.");
            //            asyncOperation.allowSceneActivation = true;
            //        }
            //    }
            //} // 게이지 다 차면 클릭 또는 터치 해야 넘어가는 코드
            yield return null;
        }
    }
}

