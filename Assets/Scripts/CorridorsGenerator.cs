using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class CorridorsGenerator
    {
        public List<Node> CreateCorridor(List<RoomNode> allNodesCollection, int corridorWidth)
        {
            var corridorList = new List<Node>();

            var structuresToCheck = new Queue<RoomNode>(allNodesCollection.OrderByDescending(o => o.TreeLayerIndex));

            while (structuresToCheck.Count > 0)
            {
                var node = structuresToCheck.Dequeue();

                if (node.ChildrenNodeList.Count == 0)
                {
                    continue;
                }

                var corridor = new CorridorNode(
                    fromNode: node.ChildrenNodeList[0],
                    toNode: node.ChildrenNodeList[1],
                    corridorWidth: corridorWidth);
                corridorList.Add(corridor);
            }

            return corridorList;
        }
    }
}
