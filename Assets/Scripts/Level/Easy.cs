using System.Collections.Generic;

namespace Level
{
    public class Easy : ILevel
    {
        public string StatisticName => "easy";
        public bool IsLimitedWaveCount => true;
        public int WaveCount => 10;

        public List<Tile.Info> WaveTileInfos(int waveCount)
        {
            switch (waveCount)
            {
                case 1:
                case 6:
                    return Wave.AllTilesInfos();
                case 2:
                case 7:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Man, 3);
                case 3:
                case 8:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Pin, 3);
                case 4:
                case 9:
                    return Wave.OneTypeTilesInfos(Tile.Info.Type.Sou, 3);
                case 5:
                case 10:
                    return Wave.RandomAllTypeTilesInfos(3);
                default:
                    return Wave.AllTilesInfos();
            }
        }
    }
}
