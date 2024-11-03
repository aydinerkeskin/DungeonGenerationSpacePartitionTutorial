using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoomGenerator
    {
        private int _maxIterations;
        private int _roomLengthMin;
        private int _roomWidthMin;

        public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
        {
            _maxIterations = maxIterations;
            _roomLengthMin = roomLengthMin;
            _roomWidthMin = roomWidthMin;
        }

        public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
        {
            List<RoomNode> listToReturn = new List<RoomNode>();

            foreach (var space in roomSpaces)
            {
                Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                    boundaryLeftPoint: space.BottomLeftAreaCorner,
                    boundaryRightPoint: space.TopRightAreaCorner, 
                    pointModifier: roomBottomCornerModifier, 
                    offset: roomOffset);

                Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
                    boundaryLeftPoint: space.BottomLeftAreaCorner,
                    boundaryRightPoint: space.TopRightAreaCorner,
                    pointModifier: roomTopCornerModifier,
                    offset: roomOffset);

                space.BottomLeftAreaCorner = newBottomLeftPoint;
                space.TopRightAreaCorner = newTopRightPoint;
                space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
                space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);

                listToReturn.Add((RoomNode)space);
            }

            return listToReturn;
        }
    }
}
