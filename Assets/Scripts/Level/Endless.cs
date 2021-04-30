using System.Collections.Generic;

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
    }
}
