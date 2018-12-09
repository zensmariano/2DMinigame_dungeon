using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {

	

	public GameObject smallCoin;
	public GameObject mediumCoin;
	public GameObject bigCoin;

	public Vector3 coinSpawnRate;
 
	
	private void CleanCoins(){
	foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
	}

	public void GenerateCoins(GameObject[,] tilePositions, GameObject mmTile){
		
		CleanCoins();

		foreach(GameObject tile in tilePositions){
			GameObject instance = Instantiate(smallCoin, tile.transform.position, Quaternion.identity) as GameObject;
			instance.transform.SetParent(transform);
			/*if (tile == mmTile){
				 if(Random.Range(0,100) < coinSpawnRate.x){
					 GameObject instance = Instantiate(smallCoin, tile.transform.position, Quaternion.identity) as GameObject;
					 instance.transform.SetParent(transform);
				 }
				 if(Random.Range(0,100) < coinSpawnRate.y){
					 GameObject instance = Instantiate(mediumCoin, tile.transform.position, Quaternion.identity) as GameObject;
					 instance.transform.SetParent(transform);
				 }
				 if(Random.Range(0,100) < coinSpawnRate.z){
					 GameObject instance = Instantiate(bigCoin, tile.transform.position, Quaternion.identity) as GameObject;
					 instance.transform.SetParent(transform);
				 }                                                                                                                       
			}
			*/
		}
	}
}
