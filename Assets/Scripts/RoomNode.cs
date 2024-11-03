using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoomNode : Node
    {
        public int Width { get => (int)(this.TopRightAreaCorner.x - this.BottomLeftAreaCorner.x); }
        public int Length { get => (int)(this.TopRightAreaCorner.y - this.BottomLeftAreaCorner.y); }

        public RoomNode(Vector2Int bottomLeftAreaCorner,
            Vector2Int topRightAreaCorner,
            Node parentNode,
            int index) : base(parentNode)
        {
            this.BottomLeftAreaCorner = bottomLeftAreaCorner;
            this.TopRightAreaCorner = topRightAreaCorner;
            this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
            this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, topRightAreaCorner.y);
            this.TreeLayerIndex = index;
        }
    }
}
