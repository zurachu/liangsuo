using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResultLeaderboardView : MonoBehaviour
{
    [SerializeField] private PlayFabLeaderboardScrollView leaderboardScrollView;
    [SerializeField] private Text titleText;
    [SerializeField] private CanvasGroup buttonCanvasGroup;

    private string message;

    public async void Initialize(Level.ILevel level, int maxResultsCount, int score)
    {
        message = $"二索 ~ Find the 2 ~ {level.Name}でスコア{score}点を達成！";

        UIUtility.TrySetText(titleText, $"{level.Name}ランキング");

        buttonCanvasGroup.interactable = false;
        await leaderboardScrollView.Initialize(level.StatisticName, maxResultsCount, score);
        buttonCanvasGroup.interactable = true;
        BGMManager.Instance.Play(BGMPath.RESULT, 0.5f);
    }

    public void OnClickTweet()
    {
#if UNITY_WEBGL
        naichilab.UnityRoomTweet.Tweet("liangsuo", message, "unityroom", "unity1week");
#else
        TweetWithoutUnityRoom(message);
#endif
    }

    private void TweetWithoutUnityRoom(string message)
    {
        var messageWithGooglePlayStoreUrl = $"{message}\nhttps://play.google.com/store/apps/details?id=com.zurachu.liangsuo";
        Application.OpenURL("http://twitter.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(messageWithGooglePlayStoreUrl));
    }
}
