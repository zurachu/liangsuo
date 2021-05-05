using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Endless : ILevel
    {
        public string Name => "エンドレスモード";
        public string StatisticName => "endless";
        public bool IsLimitedWaveCount => false;
        public int WaveCount => 0;

        public List<Tile.Info> WaveTileInfos(int waveCount)
        {
            return waveCount switch
            {
                int i when 1 <= i && i <= 10 => new Easy().WaveTileInfos(i),
                int i when 11 <= i && i <= 20 => new Normal().WaveTileInfos(i - 10),
                int i when 21 <= i && i <= 30 => new Hard().WaveTileInfos(i - 20),
                int i when i % 10 == 1 => Wave.OneTypeTilesInfos(Tile.Info.Type.Man, 5),
                int i when i % 10 == 2 => Wave.OneTypeTilesInfos(Tile.Info.Type.Pin, 5),
                int i when i % 10 == 3 => Wave.OneTypeTilesInfos(Tile.Info.Type.Sou, 5),
                _ => Wave.RandomAllTypeTilesInfos(5),
            };
        }

        public float RecoveryTimeByHit(int waveCount)
        {
            return waveCount switch
            {
                int i when 1 <= i && i <= 10 => new Easy().RecoveryTimeByHit(i),
                int i when 11 <= i && i <= 20 => new Normal().RecoveryTimeByHit(i - 10),
                int i when 21 <= i && i <= 30 => new Hard().RecoveryTimeByHit(i - 20),
                _ => RecoveryTimeAfterHard(waveCount, 0.5f, 0.1f),
            };
        }

        public float RecoveryTimeByClear(int waveCount)
        {
            return waveCount switch
            {
                int i when 1 <= i && i <= 10 => new Easy().RecoveryTimeByClear(i),
                int i when 11 <= i && i <= 20 => new Normal().RecoveryTimeByClear(i - 10),
                int i when 21 <= i && i <= 30 => new Hard().RecoveryTimeByClear(i - 20),
                _ => RecoveryTimeAfterHard(waveCount, 2f, 0.5f),
            };
        }

        private float RecoveryTimeAfterHard(int waveCount, float max, float min)
        {
            var rate = Mathf.Max(0f, (60 - waveCount) / 30f);
            return max * rate + min * (1f - rate);
        }
    }
}
