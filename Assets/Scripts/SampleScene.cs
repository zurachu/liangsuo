using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Transform dropper;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private CanvasGroup titleCanvasGroup;

    private static readonly int dropTileCountPerLine = 4;

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
    }

    private void Drop()
    {
        var tileInfos = new List<Tile.Info>();
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Man, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Pin, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Sou, _number)));
        tileInfos = ListUtility.Shuffle(tileInfos);

        foreach (var (tileInfo, index) in tileInfos.WithIndex())
        {
            var x = -1.5f + (index % dropTileCountPerLine) * 1f + Random.Range(-0.25f, 0.25f);
            var y = dropper.position.y + index / dropTileCountPerLine * 1.5f;
            var position = new Vector3(x, y);
            var rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            var tile = Instantiate(tilePrefab, position, rotation, dropper);
            tile.Initialize(tileInfo, (_tile) =>
            {
                if (_tile.TileInfo.number == 2)
                {
                    Destroy(tile.gameObject);
                }
            });
        }
    }
}
