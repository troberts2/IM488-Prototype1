 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class RandomPointsOnSurface : MonoBehaviour
{
    [Tooltip("Maximal high of the surface.")]
    public float surfaceHigh = 1f; //
    [Tooltip("Number of random points to generate on the surface.")]
    public int numPoints = 20;
    [Tooltip("Maximal number of iterations to find the points.")]
    public int maxIterations = 200;
    [Tooltip("Size of the generated sphere primitives.")]
    public Vector3 scalePrimitives = new Vector3(100f, 100f, 100f);
    [Tooltip("Color of the generated sphere primitives.")]
    public Color colorPrimitives = Color.red;
 
    private bool isUnityTerrain = false;
    private MeshCollider m_collider;
    private float bboxScale = 1f;

    public TerrainChunk terrainChunk;

 
    void Start()
    {

    }
    public void InitializeObject(){
        m_collider = GetComponent<MeshCollider>();
 
        // Check if we have a Unity terrain
        Terrain terrain = GetComponent<Terrain>();
        if (terrain != null)
        {
            surfaceHigh = terrain.terrainData.size.y;
            isUnityTerrain = true;
        }
 
        GenerateRandomPositions();
    }
 
    void GenerateRandomPositions()
    {
        Vector3 pointRandom;
        Vector3 pointOnSurface = Vector3.zero;
        bool pointFound = false;
        int indexPoints = 0;
        int indexLoops = 0;
        do
        {
            indexLoops++;
            // Double the size of the bounding box here to get better results if not a Unity terrain
            if (isUnityTerrain) bboxScale = 1f;
            else bboxScale = 2f;
 
            if (isUnityTerrain) pointRandom = RandomPointInBounds(m_collider.bounds, bboxScale);
            else pointRandom = RandomPointInBounds(m_collider.bounds, m_collider.bounds.size.magnitude) - transform.position;
 
            pointFound = GetRandomPointOnColliderSurface(pointRandom, out pointOnSurface);
 
            if (true)
            {
                indexPoints++;
                GameObject hazard = Instantiate(FindObjectOfType<HazardsData>().hazards[Random.Range(0, FindObjectOfType<HazardsData>().hazards.Length)], terrainChunk.meshObject.transform);
                hazard.SetActive(true);
                hazard.transform.position = pointRandom;
                hazard.transform.localScale = scalePrimitives;
            }
        } while ((indexPoints < numPoints) && (indexLoops < maxIterations));
    }
 
    private bool GetRandomPointOnColliderSurface(Vector3 point, out Vector3 pointSurface)
    {
 
        Vector3 pointOnSurface = Vector3.zero;
        RaycastHit hit;
        bool pointFound = false;
        // Raycast against the surface of the transform
        Debug.DrawRay(new Vector3(point.x, point.y - surfaceHigh, point.z), transform.up * surfaceHigh, Color.green, 5f);
        if (Physics.Raycast(new Vector3(point.x, point.y - surfaceHigh, point.z), transform.up, out hit, Mathf.Infinity))
        {
            Debug.Log("Found point up");
            pointOnSurface = hit.point;
            pointFound = true;
        }
        else
        {
            Debug.DrawRay(new Vector3(point.x, point.y - surfaceHigh, point.z), -transform.up * surfaceHigh, Color.red, 5f);
            if (Physics.Raycast(new Vector3(point.x, point.y - surfaceHigh, point.z), -transform.up, out hit, Mathf.Infinity))
            {
                Debug.Log("Found point -up");
                pointOnSurface = hit.point;
                pointFound = true;
            }
        }
 
        pointSurface = pointOnSurface;
        return pointFound;
    }
 
    private Vector3 RandomPointInBounds(Bounds bounds, float scale)
    {
        Vector3 extents = terrainChunk.bounds.size /2f;
        Vector3 point = new Vector3(
        Random.Range( -extents.x, extents.x ),
        Random.Range( -extents.y, extents.y ),
        Random.Range( -extents.y, extents.y )
        )  + bounds.center;
        return transform.TransformPoint( point );
    }
 
    /* Dont work for planes?
    Vector3 GetRandomPointInCollider(Bounds bounds)
    {
        Vector3 point;
        point = RandomPointInBounds(bounds);
        return Physics.ClosestPoint(point, m_collider, m_collider.transform.position, m_collider.transform.rotation);
    }*/
 
    void OnDrawGizmos()
    {
        Bounds bounds;
        if (m_collider == null) bounds = transform.GetComponent<Collider>().bounds;
        else bounds = m_collider.bounds;
 
        // Draw the bounding box of the tranform
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
 
}