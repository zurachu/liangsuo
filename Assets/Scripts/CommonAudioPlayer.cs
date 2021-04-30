using KanKikuchi.AudioManager;

public static class CommonAudioPlayer
{
    public static void PlayButtonClick()
    {
        SEManager.Instance.Play(SEPath.NOTANOMORI_200812290000000032);
    }

    public static void PlayCancel()
    {
        SEManager.Instance.Play(SEPath.NOTANOMORI_200812290000000032);
    }
}
