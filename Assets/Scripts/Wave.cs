using System.Collections.Generic;
using System.Linq;

public static class Wave
{
    public static List<Tile.Info> AllTiles()
    {
        var tileInfos = new List<Tile.Info>();
        tileInfos.AddRange(AllNumbers(Tile.Info.Type.Man));
        tileInfos.AddRange(AllNumbers(Tile.Info.Type.Pin));
        tileInfos.AddRange(AllNumbers(Tile.Info.Type.Sou));
        return ListUtility.Shuffle(tileInfos);
    }

    public static List<Tile.Info> AllNumbers(Tile.Info.Type type)
    {
        return Numbers().ConvertAll(_number => new Tile.Info(type, _number));
    }

    public static List<Tile.Info> RandomOneTypeTiles(int count, int targetCount)
    {
        var numbers = new List<int>(Enumerable.Repeat(Tile.Info.TargetNumber, targetCount).ToList());

        var notTargetNumbers = Numbers();
        notTargetNumbers.Remove(Tile.Info.TargetNumber);
        for (var i = targetCount; i < count; i++)
        {
            numbers.Add(ListUtility.Random(notTargetNumbers));
        }

        var type = RandomTileType();
        var tileInfos = numbers.ConvertAll(_number => new Tile.Info(type, _number));
        return ListUtility.Shuffle(tileInfos);
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
