using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int DungeonWidth;
    public int DungeonLength;
    public int RoomWidthMin;
    public int RoomLengthMin;
    public int MaxIterations;
    public int CorridorWidth;

    [Range(0.0f, 0.3f)]
    public float RoomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float RoomTopCornerModifier;
    [Range(0, 2)]
    public int RoomOffset;

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    private void CreateDungeon()
    {
        DungeonGenerator generator = new DungeonGenerator(DungeonWidth, DungeonLength);

        var listOfRooms = generator.CalculateDungeon(
            maxIterations: MaxIterations,
            roomWidthMin: RoomWidthMin,
            roomLengthMin: RoomLengthMin,
            roomBottomCornerModifier: RoomBottomCornerModifier,
            roomTopCornerModifier: RoomTopCornerModifier,
            roomOffset: RoomOffset,
            corridorWidth: CorridorWidth);

        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
            {
                topLeftV,
                topRightV,
                bottomLeftV,
                bottomRightV
            };

        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
            {
                0,
                1,
                2,
                2,
                1,
                3
            };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
    }
}
