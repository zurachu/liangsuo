using System.Collections.Generic;

namespace Level
{
    public interface ILevel
    {
        string Name { get; }
        string StatisticName { get; }
        bool IsLimitedWaveCount { get; }
        int WaveCount { get; }
        List<Tile.Info> WaveTileInfos(int waveCount);
    }
}
