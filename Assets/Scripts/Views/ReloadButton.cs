using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReloadButton : Button
{
    private new void Awake()
    {
        base.Awake();
        onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); });
    }
}
