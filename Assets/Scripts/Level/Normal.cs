using System.Collections.Generic;

namespace Level
{
    public class Normal : ILevel
    {
        public string Name => "中級";
        public string StatisticName => "normal";
        public bool IsLimitedWaveCount => true;
        public int WaveCount => 10;

        public List<Tile.Info> WaveTileInfos(int waveCount)
        {
            switch (waveCount)
            {
                case 1:
                case 6:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Man, 4);
                case 2:
                case 7:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Pin, 4);
                case 3:
                case 8:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Sou, 4);
                case 4:
                case 5:
                case 9:
                case 10:
                    return Wave.RandomAllTypeTilesInfos(4);
                default:
                    return Wave.AllTilesInfos();
            }
        }

        public float RecoveryTimeByHit(int waveCount) => 0.5f;
        public float RecoveryTimeByClear(int waveCount) => 2f;
    }
}
