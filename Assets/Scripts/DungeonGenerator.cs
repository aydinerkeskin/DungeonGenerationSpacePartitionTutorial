using Assets.Scripts;
using System.Collections.Generic;

public class DungeonGenerator
{
    private int _dungeonWidth;
    private int _dungeonLength;

    private List<RoomNode> _allNodesCollection = new List<RoomNode>();

    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        _dungeonWidth = dungeonWidth;
        _dungeonLength = dungeonLength;
    }

    public List<Node> CalculateDungeon(
        int maxIterations, 
        int roomWidthMin, 
        int roomLengthMin, 
        float roomBottomCornerModifier, 
        float roomTopCornerModifier, 
        int roomOffset,
        int corridorWidth)
    {
        var bsp = new BinarySpacePartitioner(_dungeonWidth, _dungeonLength);

        _allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);

        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);

        var roomGenerator = new RoomGenerator(maxIterations: maxIterations, roomLengthMin: roomLengthMin, roomWidthMin: roomWidthMin);

        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(
            roomSpaces: roomSpaces,
            roomBottomCornerModifier: roomBottomCornerModifier,
            roomTopCornerModifier: roomTopCornerModifier,
            roomOffset: roomOffset);

        var corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCorridor(
            allNodesCollection: _allNodesCollection,
            corridorWidth: corridorWidth);

        return new List<Node>(roomList);
    }
}
