using UnityEngine;
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
    }

    public void OnClickTweet()
    {
#if UNITY_WEBGL
        naichilab.UnityRoomTweet.Tweet("liangsuo", message, "unityroom", "unity1week");
#else
        TweetWithoutUnityRoom(message);
#endif
    }
}
