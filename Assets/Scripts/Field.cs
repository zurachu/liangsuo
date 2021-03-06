using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private GameObject bottom;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private ParticleSystem particlePrefab;

    private static readonly int dropTileCountPerLine = 5;

    private List<Tile> tiles;

    public bool TargetNumberRemained
    {
        get
        {
            if (ListUtility.IsNullOrEmpty(tiles))
            {
                return false;
            }

            return tiles.Exists(_tile => _tile.TileInfo.IsTargetNumber);
        }
    }

    public void ClearTiles()
    {
        if (!ListUtility.IsNullOrEmpty(tiles))
        {
            foreach (var tile in tiles)
            {
                Destroy(tile.gameObject);
            }
        }

        tiles = new List<Tile>();
    }

    public void Drop(List<Tile.Info> tileInfos, Action onHit, Action onMissed)
    {
        UIUtility.TrySetActive(bottom, true);
        SEManager.Instance.Play(SEPath.NOTANOMORI_200812220000000008, 2f);

        ClearTiles();

        foreach (var (tileInfo, index) in tileInfos.WithIndex())
        {
            var x = -2f + (index % dropTileCountPerLine) * 1f + UnityEngine.Random.Range(-0.25f, 0.25f);
            var y = 6 + index / dropTileCountPerLine * 1.5f;
            var position = new Vector3(x, y);
            var rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f));

            var tile = Instantiate(tilePrefab, position, rotation, transform);
            tile.Initialize(tileInfo, (_tile) =>
            {
                if (_tile.TileInfo.IsTargetNumber)
                {
                    var particle = Instantiate(particlePrefab, _tile.transform.position, _tile.transform.localRotation, transform);
                    RemoveTile(_tile);
                    SEManager.Instance.Play(SEPath.KIN);
                    onHit?.Invoke();
                }
                else
                {
                    RemoveTile(_tile);
                    SEManager.Instance.Play(SEPath.OSII);
                    onMissed?.Invoke();
                }
            });

            tiles.Add(tile);
        }
    }

    public async UniTask Flush()
    {
        UIUtility.TrySetActive(bottom, false);
        SEManager.Instance.Play(SEPath.RIGHT2);
        SEManager.Instance.Play(SEPath.NOTANOMORI_200812220000000008, 2f);
        await UniTask.Delay(2000);
        ClearTiles();
    }

    private void RemoveTile(Tile tile)
    {
        if (!ListUtility.IsNullOrEmpty(tiles))
        {
            tiles.Remove(tile);
        }

        Destroy(tile.gameObject);
    }
}
