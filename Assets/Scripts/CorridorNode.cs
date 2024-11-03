using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CorridorNode : Node
    {
        private Node _fromNode;
        private Node _toNode;
        private int _corridorWidth;
        private int _modifierDistanceFromWall = 1;

        public CorridorNode(Node fromNode, Node toNode, int corridorWidth) : base(null)
        {
            _fromNode = fromNode;
            _toNode = toNode;
            _corridorWidth = corridorWidth;

            GenerateCorridor();
        }

        private void GenerateCorridor()
        {
            var relativePositionOfStructureTo = CheckPositionNodeToAgainstNodeFrom();

            switch (relativePositionOfStructureTo)
            {
                case RelativePosition.Up:
                    ProcessRoomInRelationUpOrDown(_fromNode, _toNode);
                    break;
                case RelativePosition.Down:
                    ProcessRoomInRelationUpOrDown(_toNode, _fromNode);
                    break;
                case RelativePosition.Right:
                    ProcessRoomInRelationRightOrLeft(_fromNode, _toNode);
                    break;
                case RelativePosition.Left:
                    ProcessRoomInRelationRightOrLeft(_toNode, _fromNode);
                    break;
                default:
                    break;
            }
        }

        private RelativePosition CheckPositionNodeToAgainstNodeFrom()
        {
            Vector2 middlePointNodeFromTemp = ((Vector2)(_fromNode.TopRightAreaCorner + _fromNode.BottomLeftAreaCorner)) / 2;
            Vector2 middlePointNodeTopTemp = ((Vector2)(_toNode.TopRightAreaCorner + _toNode.BottomLeftAreaCorner)) / 2;

            float angle = CalculateAngle(middlePointNodeFromTemp, middlePointNodeTopTemp);

            if ((angle < 45 && angle >= 0) || (angle > -45 && angle < 0))
            {
                return RelativePosition.Right;
            }
            else if (angle > 45 && angle < 135)
            {
                return RelativePosition.Up;
            }
            else if (angle > -135 && angle < -45)
            {
                return RelativePosition.Down;
            }
            else
            {
                return RelativePosition.Left;
            }
        }

        private float CalculateAngle(Vector2 middlePointNodeFromTemp, Vector2 middlePointNodeTopTemp)
        {
            return Mathf.Atan2(middlePointNodeTopTemp.y - middlePointNodeFromTemp.y,
                middlePointNodeTopTemp.x - middlePointNodeFromTemp.x) * Mathf.Rad2Deg;
        }

        private void ProcessRoomInRelationUpOrDown(Node fromNode, Node toNode)
        {
            Node bottomNode = null;

            List<Node> bottomNodeChildrens = StructureHelper.TraverseGraphToExtractLowestLeafes(parentNode: fromNode);

            Node topNode = null;

            List<Node> topNodeChildrens = StructureHelper.TraverseGraphToExtractLowestLeafes(parentNode: toNode);

            var sortedBottomNodes = bottomNodeChildrens.OrderByDescending(child => child.TopRightAreaCorner.y).ToList();

            if (sortedBottomNodes.Count == 1)
            {
                bottomNode = bottomNodeChildrens[0];
            }
            else
            {
                int maxY = sortedBottomNodes[0].TopLeftAreaCorner.y;

                sortedBottomNodes = sortedBottomNodes.Where(child => Mathf.Abs(maxY - child.TopLeftAreaCorner.y) < 10).ToList();

                int index = UnityEngine.Random.Range(0, sortedBottomNodes.Count);

                bottomNode = bottomNodeChildrens[index];
            }

            var possibleNeighboursInTopNodeList = topNodeChildrens.Where(child =>
                GetValidXForNeighbourUpDown(
                    bottomNode.TopLeftAreaCorner,
                    bottomNode.TopRightAreaCorner,
                    child.BottomLeftAreaCorner,
                    child.BottomRightAreaCorner
                ) != 1
            ).OrderBy(child => child.BottomRightAreaCorner.y).ToList();

            if (possibleNeighboursInTopNodeList.Count == 0)
            {
                
            }
        }

        private void ProcessRoomInRelationRightOrLeft(Node fromNode, Node toNode)
        {
            Node leftNode = null;

            List<Node> leftNodeChildrens = StructureHelper.TraverseGraphToExtractLowestLeafes(parentNode: fromNode);

            Node rightNode = null;

            List<Node> rightNodeChildrens = StructureHelper.TraverseGraphToExtractLowestLeafes(parentNode: toNode);

            var sortedLeftNodes = leftNodeChildrens.OrderByDescending(o => o.TopRightAreaCorner.x).ToList();

            if (sortedLeftNodes.Count == 1)
            {
                leftNode = sortedLeftNodes[0];
            }
            else
            {
                int maxX = sortedLeftNodes[0].TopRightAreaCorner.x;

                sortedLeftNodes = sortedLeftNodes.Where(children => Math.Abs(maxX - children.TopRightAreaCorner.x) < 10).ToList();

                int index = UnityEngine.Random.Range(0, sortedLeftNodes.Count);

                leftNode = sortedLeftNodes[index];
            }

            var possibleNeighboursInRightNodeList = rightNodeChildrens.Where(child => 
                GetValidYForNeighbourLeftRight(
                    leftNodeUp: leftNode.TopRightAreaCorner,
                    leftNodeDown: leftNode.BottomRightAreaCorner,
                    rightNodeUp: child.TopLeftAreaCorner,
                    rightNodeDown: child.BottomLeftAreaCorner
                ) != -1
            ).ToList();

            if (possibleNeighboursInRightNodeList.Count <= 0)
            {
                rightNode = toNode;
            }
            else
            { 
                rightNode = possibleNeighboursInRightNodeList[0];
            }

            int y = GetValidYForNeighbourLeftRight(
                leftNodeUp: leftNode.TopLeftAreaCorner, 
                leftNodeDown: leftNode.BottomRightAreaCorner, 
                rightNodeUp: rightNode.TopLeftAreaCorner,
                rightNodeDown: rightNode.BottomLeftAreaCorner);

            while (y == -1 && sortedLeftNodes.Count > 1)
            {
                sortedLeftNodes = sortedLeftNodes.Where(child =>
                    child.TopLeftAreaCorner.y != leftNode.TopLeftAreaCorner.y).ToList();

                leftNode = sortedLeftNodes[0];

                y = GetValidYForNeighbourLeftRight(
                    leftNodeUp: leftNode.TopLeftAreaCorner,
                    leftNodeDown: leftNode.BottomRightAreaCorner,
                    rightNodeUp: rightNode.TopLeftAreaCorner,
                    rightNodeDown: rightNode.BottomLeftAreaCorner);
            }

            BottomLeftAreaCorner = new Vector2Int(leftNode.BottomRightAreaCorner.x, y);
            TopRightAreaCorner = new Vector2Int(rightNode.TopLeftAreaCorner.x, y + this._corridorWidth);
        }

        private int GetValidYForNeighbourLeftRight(
            Vector2Int leftNodeUp, 
            Vector2Int leftNodeDown, 
            Vector2Int rightNodeUp, 
            Vector2Int rightNodeDown)
        {
            if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
            {
                return StructureHelper.CalculateMiddlePoint(
                    leftNodeDown + new Vector2Int(0, _modifierDistanceFromWall),
                    leftNodeUp - new Vector2Int(0, _modifierDistanceFromWall + this._corridorWidth)
                ).y;
            }

            if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
            {
                return StructureHelper.CalculateMiddlePoint(
                    rightNodeDown + new Vector2Int(0, _modifierDistanceFromWall),
                    rightNodeUp - new Vector2Int(0, _modifierDistanceFromWall + this._corridorWidth)
                ).y;
            }

            if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
            {
                return StructureHelper.CalculateMiddlePoint(
                    rightNodeDown + new Vector2Int(0, _modifierDistanceFromWall),
                    leftNodeUp - new Vector2Int(0, _modifierDistanceFromWall)
                ).y;
            }

            if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
            {
                return StructureHelper.CalculateMiddlePoint(
                    leftNodeDown + new Vector2Int(0, _modifierDistanceFromWall),
                    rightNodeUp - new Vector2Int(0, _modifierDistanceFromWall + this._corridorWidth)
                ).y;
            }

            return -1;
        }

        private int GetValidXForNeighbourUpDown(Vector2Int topLeftAreaCorner, Vector2Int topRightAreaCorner, Vector2Int bottomLeftAreaCorner, Vector2Int bottomRightAreaCorner)
        {
            throw new NotImplementedException();
        }
    }
}
