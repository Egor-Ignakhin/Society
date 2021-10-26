using UnityEngine;
using System;
using System.Collections;


public class PL_RandomMovement:MonoBehaviour{
    
    public Vector3 rp;
    public int counter;
    public float speed;
    
    public int duplicate;
    public static bool duped;
    
    public IEnumerator Start() {
    	if(this.transform.childCount > 0){
    	InvokeRepeating("RandomPos", 0.0f, UnityEngine.Random.Range(1.1f,.3f));
    	yield return new WaitForSeconds(4.0f);
    	if(!duped){
    	for(int i=0; i < duplicate; i++){	
    		StartCoroutine(Spawn());
    	}
    		duped=true;
    	}
    	}else{
    		Destroy(gameObject);
    	}
    }
    
    public void RandomPos() {
    
    	rp = UnityEngine.Random.insideUnitSphere*30;
    	rp.x*=1.25f;
    	rp.z*=2.0f;
    	rp.y*=.87f;
    	counter=0;
    }
    
    public IEnumerator Spawn() {
    
    	yield return new WaitForSeconds(UnityEngine.Random.value);
    	Instantiate(gameObject);
    	
    	
    }
    
    public void Update() {
    	counter++;
    	transform.position = Vector3.Lerp(transform.position, rp, Time.deltaTime*counter*.05f*speed);
    }
}