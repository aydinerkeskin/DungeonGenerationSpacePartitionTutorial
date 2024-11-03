using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Line
    {
        Orientation _orientation;
        Vector2Int _coordinates;

        public Orientation Orientation { get => _orientation; }
        public Vector2Int Coordinates { get => _coordinates; }

        public Line(Orientation orientation, Vector2Int coordinates)
        {
            _orientation = orientation;
            _coordinates = coordinates;
        }
    }

    public enum Orientation
    {
        Horizontal = 0,
        Vertical = 1
    }
}
