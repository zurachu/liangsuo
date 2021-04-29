using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Timer timer;
    [SerializeField] private CanvasGroup titleCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStartGame()
    {
        UIUtility.TrySetActive(titleCanvasGroup.gameObject, false);
        Drop();
        timer.Reset();
        timer.IsRunning = true;
    }

    private void Drop()
    {
        var tileInfos = new List<Tile.Info>();
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Man, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Pin, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Sou, _number)));
        tileInfos = ListUtility.Shuffle(tileInfos);
        field.Drop(tileInfos, OnHit, null);
    }

    private async void OnHit()
    {
        timer.Recover(1f);

        if (!field.TargetNumberRemained)
        {
            await field.Flush();
            Drop();
        }
    }
}
