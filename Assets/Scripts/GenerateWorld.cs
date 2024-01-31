using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GenerateWorld : MonoBehaviour
{   
    [SerializeField] private GameObject player;
    private Vector3 startPosition;
    private Hashtable blockContainer = new Hashtable();

    [Header("Map Size and Level")]
    [SerializeField] private GameObject blockGameObject;
    [SerializeField] private int worldSizeX = 10;
    [SerializeField] private int worldSizeZ = 100;

    [SerializeField] private int noiseHeight = 3;
    [SerializeField] private float gridOffset = 1.1f;

    [Header("Spawnable Objects")]
    private List<Vector3> blockPositions = new List<Vector3>();
    [SerializeField] private List<GameObject> rocks = new List<GameObject>();


    private void Start() {
    }
    private void Update() {
        if(Mathf.Abs(XPlayerMove) >= 1 || Mathf.Abs(ZPlayerMove) >=1){
            for(int x = -worldSizeX; x < worldSizeX; x++){
                for(int z = -worldSizeZ; z < worldSizeZ; z++){
                    Vector3 pos = new Vector3(x * 1 + XPlayerLocation, GenerateNoise(x + XPlayerLocation, z + ZPlayerLocation, 8f) * noiseHeight, z * 1 + ZPlayerLocation);
                    if(!blockContainer.ContainsKey(pos)){
                        GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity) as GameObject;

                        blockContainer.Add(pos, block);
                        blockPositions.Add(block.transform.position);

                        block.transform.SetParent(this.transform);
                    }

                }
            }
        }
    }
    private void GenerateLevel(){
        for(int x = -worldSizeX; x < worldSizeX; x++){
            for(int z = -worldSizeZ; z < worldSizeZ; z++){
                Vector3 pos = new Vector3(x * 1 + startPosition.x, GenerateNoise(x, z, 8f) * noiseHeight, z * 1 + startPosition.z);
                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity) as GameObject;

                blockContainer.Add(pos, block);
                blockPositions.Add(block.transform.position);

                block.transform.SetParent(this.transform);
            }
        }
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

    private float GenerateNoise(int x, int z, float detailScale){
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }

    // private void SpawnObject(){
    //     for(int c = 0; c < 20; c++){
    //         GameObject rock = rocks[Random.Range(0, rocks.Count)];
    //         GameObject toPlaceObject = Instantiate(rock, ObjectSpawnLocation(), Quaternion.identity);
    //     }
    // }

    // private Vector3 ObjectSpawnLocation(){
    //     int rndIndex = Random.Range(0, blockPositions.Count);

    //     Vector3 newPos = new Vector3(blockPositions[rndIndex].x, blockPositions[rndIndex].y + 0.5f, blockPositions[rndIndex].z);
    //     blockPositions.RemoveAt(rndIndex);
    //     return newPos;
    // }
}
