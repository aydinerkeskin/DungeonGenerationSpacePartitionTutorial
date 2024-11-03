using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Node
    {
        private List<Node> _childrenNodeList;

        public List<Node> ChildrenNodeList { get => _childrenNodeList; }

        public bool Visited { get; set; }
        public Vector2Int BottomLeftAreaCorner { get; set; }
        public Vector2Int BottomRightAreaCorner { get; set; }
        public Vector2Int TopRightAreaCorner { get; set; }
        public Vector2Int TopLeftAreaCorner { get; set; }

        public int TreeLayerIndex { get; set; }

        public Node _parentNode { get; set; }

        public Node(Node parentNode)
        {
            _childrenNodeList = new List<Node>();

            _parentNode = parentNode;

            if (parentNode != null)
            {
                parentNode.AddChild(this);
            }
        }

        public void AddChild(Node child)
        {
            _childrenNodeList.Add(child);
        }

        public void RemoveChild(Node child)
        {
            _childrenNodeList.Remove(child);
        }
    }
}
