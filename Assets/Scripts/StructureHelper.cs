using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class StructureHelper
    {
        public static List<Node> TraverseGraphToExtractLowestLeafes(Node parentNode)
        {
            Queue<Node> nodesToCheck = new Queue<Node>();

            List<Node> listToReturn = new List<Node>();

            if (parentNode.ChildrenNodeList.Count == 0)
            {
                return new List<Node>() { parentNode };
            }

            foreach (Node child in parentNode.ChildrenNodeList)
            {
                nodesToCheck.Enqueue(child);
            }

            while (nodesToCheck.Count > 0)
            {
                var currentNode = nodesToCheck.Dequeue();

                if (currentNode.ChildrenNodeList.Count == 0)
                {
                    listToReturn.Add(currentNode);
                }
                else
                {
                    foreach (var child in currentNode.ChildrenNodeList)
                    {
                        nodesToCheck.Enqueue(child);
                    }
                }
            }

            return listToReturn;
         }

        public static Vector2Int GenerateBottomLeftCornerBetween(
            Vector2Int boundaryLeftPoint, 
            Vector2Int boundaryRightPoint, 
            float pointModifier, 
            int offset)
        {
            int minX = boundaryLeftPoint.x + offset;
            int maxX = boundaryRightPoint.x - offset;
            int minY = boundaryLeftPoint.y + offset;
            int maxY = boundaryRightPoint.y - offset;

            return new Vector2Int(
                UnityEngine.Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
                UnityEngine.Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier)));
        }

        public static Vector2Int GenerateTopRightCornerBetween(
            Vector2Int boundaryLeftPoint,
            Vector2Int boundaryRightPoint,
            float pointModifier,
            int offset)
        {
            int minX = boundaryLeftPoint.x + offset;
            int maxX = boundaryRightPoint.x - offset;
            int minY = boundaryLeftPoint.y + offset;
            int maxY = boundaryRightPoint.y - offset;

            var xRandom = UnityEngine.Random.Range((int)(minX + (maxX - minX) * pointModifier), maxX);
            var yRandom = UnityEngine.Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY);

            return new Vector2Int(xRandom, yRandom);
        }

        public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
        {
            var sum = v1 + v2;
            var tempVector = sum / 2;
            return new Vector2Int((int)tempVector.x, (int)tempVector.y);
        }
    }

    public enum RelativePosition
    { 
        Up,
        Down,
        Right,
        Left
    }
}
