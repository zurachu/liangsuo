using System.Collections.Generic;
using System.Linq;

public static class Wave
{
    public static List<Tile.Info> AllTilesInfos()
    {
        var tileInfos = new List<Tile.Info>();
        tileInfos.AddRange(AllNumbersInfos(Tile.Info.Type.Man));
        tileInfos.AddRange(AllNumbersInfos(Tile.Info.Type.Pin));
        tileInfos.AddRange(AllNumbersInfos(Tile.Info.Type.Sou));
        return ListUtility.Shuffle(tileInfos);
    }

    public static List<Tile.Info> RandomAllTypeTilesInfos(int numbersCount)
    {
        var tileInfos = new List<Tile.Info>();
        for (var i = 0; i < numbersCount; i++)
        {
            tileInfos.AddRange(Numbers().ConvertAll(_number => new Tile.Info(RandomTileType(), _number)));
        }

        return ListUtility.Shuffle(tileInfos);
    }

    public static List<Tile.Info> RandomOneTypeTilesInfos(int numbersCount)
    {
        return OneTypeTilesInfos(RandomTileType(), numbersCount);
    }

    public static List<Tile.Info> OneTypeTilesInfos(Tile.Info.Type type, int numbersCount)
    {
        var tileInfos = new List<Tile.Info>();
        for (var i = 0; i < numbersCount; i++)
        {
            tileInfos.AddRange(AllNumbersInfos(type));
        }

        return ListUtility.Shuffle(tileInfos);
    }

    private static List<Tile.Info> AllNumbersInfos(Tile.Info.Type type)
    {
        return Numbers().ConvertAll(_number => new Tile.Info(type, _number));
    }

    private static Tile.Info.Type RandomTileType()
    {
        var types = new List<Tile.Info.Type> { Tile.Info.Type.Man, Tile.Info.Type.Pin, Tile.Info.Type.Sou };
        return ListUtility.Random(types);
    }

    private static List<int> Numbers()
    {
        return Enumerable.Range(1, 9).ToList();
    }
}
