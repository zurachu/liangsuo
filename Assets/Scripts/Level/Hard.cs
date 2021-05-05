using System.Collections.Generic;

namespace Level
{
    public class Hard : ILevel
    {
        public string Name => "上級";
        public string StatisticName => "hard";
        public bool IsLimitedWaveCount => true;
        public int WaveCount => 10;

        public List<Tile.Info> WaveTileInfos(int waveCount)
        {
            switch (waveCount)
            {
                case 1:
                case 6:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Man, 5);
                case 2:
                case 7:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Pin, 5);
                case 3:
                case 8:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Sou, 5);
                case 4:
                case 5:
                case 9:
                case 10:
                    return Wave.RandomAllTypeTilesInfos(5);
                default:
                    return Wave.AllTilesInfos();
            }
        }

        public float RecoveryTimeByHit(int waveCount) => 0.5f;
        public float RecoveryTimeByClear(int waveCount) => 2f;
    }
}
