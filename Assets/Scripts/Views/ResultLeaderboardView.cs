using UnityEngine;
using UnityEngine.UI;

public class ResultLeaderboardView : MonoBehaviour
{
    [SerializeField] private PlayFabLeaderboardScrollView leaderboardScrollView;
    [SerializeField] private Text titleText;
    [SerializeField] private CanvasGroup buttonCanvasGroup;

    private Level.ILevel level;
    private int score;

    public async void Initialize(Level.ILevel level, int maxResultsCount, int score)
    {
        this.level = level;
        this.score = score;
        UIUtility.TrySetText(titleText, $"{level.Name}ランキング");

        buttonCanvasGroup.interactable = false;
        await leaderboardScrollView.Initialize(level.StatisticName, maxResultsCount, score);
        buttonCanvasGroup.interactable = true;
    }

    public void OnClickTweet()
    {
        var message = $"二索 ~ Find the 2 ~ {level.Name}でスコア{score}点を達成！";
#if UNITY_WEBGL
        naichilab.UnityRoomTweet.Tweet("liangsuo", message, "unityroom", "unity1week");
#else
        TweetWithoutUnityRoom(message);
#endif
    }
}
