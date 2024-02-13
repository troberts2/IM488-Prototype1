using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehavior : MonoBehaviour
{
    bool hitLand = false;
    // Start is called before the first frame update
    void Awake(){
        //StartCoroutine(DestroyAfter3());
    }
    void Update()
    {
        if(!hitLand){
          FindLand();  
        }
        
    }



    public void FindLand(){
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            hitLand = true;
        }else{
            ray = new Ray(transform.position, Vector3.up);
            if(Physics.Raycast(ray, out hit)){
                transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                hitLand = true;
            }
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Obstacle")){
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyAfter3(){
        yield return new WaitForSeconds(3f);
        if(!hitLand){
            Destroy(gameObject);
        }
    }
}
