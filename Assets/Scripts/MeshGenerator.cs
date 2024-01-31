using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    //define world size
    [SerializeField] private int Worldx;
    [SerializeField] private int Worldz;
    [SerializeField] private int noiseHeight = 3;

    //create mesh to be our new mesh
    private Mesh mesh;

    //define the arrays needed for mesh
    private int[] triangles;
    private Vector3[] verticies;


    [SerializeField] private GameObject player;
    private Vector3 startPosition;
    private MeshCollider meshCollider;

    private void Start() {
        mesh = new Mesh();
        meshCollider = GetComponent<MeshCollider>();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateMesh();
        UpdateMesh();
    }
    private void Update(){
        UpdateMesh();
    }

    //Generate Mesh
    private void GenerateMesh(){
        verticies = new Vector3[(Worldx +1) * (Worldz + 1)];

        for(int i = 0, z = 0; z <= Worldz; z++){
            for(int x = 0; x <= Worldx; x++){
                float y = GenerateNoise(x, z, 8f) * noiseHeight;
                verticies[i] = new Vector3(x, y, z);
                i++;
            }
        }
        triangles = new int[Worldx * Worldz * 6];

        int tris = 0;
        int verts = 0;
        for(int z = 0; z < Worldz; z++){
            for(int x= 0; x < Worldx; x++){
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + Worldx + 1;
                triangles[tris + 2] = verts + 1;

                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + Worldx + 1;
                triangles[tris + 5] = verts + Worldx + 2;

                verts++;
                tris+=6;
            }
            verts++;
        }

    }


    // private void GenerateMesh(){
    //     triangles = new int[Worldx * Worldz * 6];
    //     verticies = new Vector3[(Worldx+1) * (Worldz + 1)];

    //     for(int i = 0, z = 0; z <= Worldz; z++){
    //         for(int x = 0; x <= Worldx; x++){
    //             verticies[i] = new Vector3(x, 0, z);
    //             i++;
    //         }
    //     }

    //     int tris = 0;
    //     int verts = 0;
    //     for(int z = 0; z < Worldz; z++){
    //         for(int x= 0; x < Worldx; x++){
    //             triangles[tris + 0] = verts + 0;
    //             triangles[tris + 1] = verts + Worldz + 1;
    //             triangles[tris + 2] = verts + 1;

    //             triangles[tris + 3] = verts + 1;
    //             triangles[tris + 4] = verts + Worldz + 1;
    //             triangles[tris + 5] = verts + Worldz + 2;

    //             verts++;
    //             tris+=6;
    //         }
    //         verts++;
    //     }
    // }
    //assign and load our mesh
    private void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        meshCollider.sharedMesh = mesh;

        mesh.RecalculateNormals();
    }

    private float GenerateNoise(int x, int z, float detailScale){
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }
        public int XPlayerMove{
        get{
            return (int)(player.transform.position.x - startPosition.x);
        }
    }
    public int ZPlayerMove{
        get{
            return (int)(player.transform.position.z - startPosition.z);
        }
    }

    private int XPlayerLocation{
        get{
            return (int)Mathf.Floor(player.transform.position.x);
        }
    }
    private int ZPlayerLocation{
        get{
            return (int)Mathf.Floor(player.transform.position.z);
        }
    }

    private void OnDrawGizmos() {
        if(verticies == null){
            return;
        }

        for(int i = 0; i < verticies.Length; i++){
            Gizmos.DrawSphere(verticies[i], .1f);
        }
    }
}
