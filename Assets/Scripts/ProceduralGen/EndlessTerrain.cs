using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    const float scale = 5f;

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public LODinfo[] detailLevels;
    public static float maxViewDist;
    public const float maxViewDistX = 10;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPos;
    Vector2 viewerPosOld;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunksVisibleInViewDist;

    Dictionary<Vector2, TerrainChunk> terrainChunkDic = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    private void Start() {
        mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDist = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunkSize = MapGenerator.mapChunkSize -1;
        chunksVisibleInViewDist = Mathf.RoundToInt(maxViewDist / chunkSize);
        
        UpdateVisibleChunks();
    }

    private void Update() {
        viewerPos = new Vector2(viewer.position.x, viewer.position.z) / scale;
        if((viewerPosOld - viewerPos).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate){
            viewerPosOld = viewerPos;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks(){
        for(int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++){
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }

        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPos.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPos.y / chunkSize);

        for(int yOffset = -chunksVisibleInViewDist; yOffset <= chunksVisibleInViewDist; yOffset++){
            for(int xOffset = -chunksVisibleInViewDist; xOffset <= chunksVisibleInViewDist; xOffset++){
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if(terrainChunkDic.ContainsKey(viewedChunkCoord)){
                    terrainChunkDic[viewedChunkCoord].UpdateTerrainChunk();
                }else{
                    terrainChunkDic.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk{
        GameObject meshObject;
        Vector2 pos;
        Bounds bounds;


        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        LODinfo[] detailLevels;
        LODMesh[] lodMeshes;

        MapData mapData;
        bool mapDataRecieved;
        int previousLODIndex = -1;

        public TerrainChunk(Vector2 coord, int size, LODinfo[] detailLevels, Transform parent, Material material){
            this.detailLevels = detailLevels;

            pos = coord * size;
            bounds = new Bounds(pos, Vector2.one * size);
            Vector3 posV3 = new Vector3(pos.x, 0, pos.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            meshObject.transform.position = posV3 * scale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * scale;
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i < detailLevels.Length; i++){
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            } 

            mapGenerator.RequestMapData(pos, OnMapDataRecieved);
        }

        void OnMapDataRecieved(MapData mapData){
            this.mapData = mapData;
            mapDataRecieved = true;

            Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }


        public void UpdateTerrainChunk(){
            if(mapDataRecieved){
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPos));
                bool visible = viewerDstFromNearestEdge <= maxViewDist;
                float viewerDstFromNearestEdgeX = Mathf.Abs(pos.x - viewerPos.x);
                bool xVisible = viewerDstFromNearestEdgeX <= maxViewDistX;

                if(visible && xVisible){
                    int lodIndex = 0;

                    for(int i = 0; i < detailLevels.Length -1; i++){
                        if(viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold){
                            lodIndex = i+1;
                        }else{
                            break;
                        }
                    }
                    if(lodIndex != previousLODIndex){
                        LODMesh lODMesh = lodMeshes[lodIndex];
                        if(lODMesh.hasMesh){
                            meshFilter.mesh = lODMesh.mesh;
                        }else if(!lODMesh.hasRequestedMesh){
                            lODMesh.RequestedMesh(mapData);
                        }
                    }
                    terrainChunksVisibleLastUpdate.Add(this);
                }
                SetVisible(visible && xVisible);
            }

        }

        public void SetVisible(bool visible){
            meshObject.SetActive(visible);
        }

        public bool IsVisible(){
            return meshObject.activeSelf;
        }
    }
    class LODMesh{
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback){
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        void OnMeshDataRecieved(MeshData meshData){
            mesh = meshData.CreateMesh();
            hasMesh = true;
            updateCallback();
        }

        public void RequestedMesh(MapData mapData){
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
        }
    }

    [System.Serializable]
    public struct LODinfo{
        public int lod;
        public float visibleDstThreshold;
    }
}
