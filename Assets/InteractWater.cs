using UnityEngine;
using System.Collections;

public class InteractWater : MonoBehaviour {

	public FluidObject[] fluidObjects;
	public int UpdateDelay;
	private int DelayCount = 0;
	private bool UpdateWater = true;

	void Start () {
		for (int i = 0; i < fluidObjects.Length; i++) {
			fluidObjects[i].InitializeMesh();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (DelayCount == UpdateDelay) {
			for (int i = 0; i < fluidObjects.Length; i++) {
				fluidObjects[i].UpdateMesh();
			}
			DelayCount = 0;
			UpdateWater = true;
		} else {
			DelayCount++;
		}
	}

	void OnTriggerStay(Collider other) {
		if (UpdateWater) {
			if (other.transform.tag.Equals ("water")) {
				for (int i = 0; i < fluidObjects.Length; i++) {
					fluidObjects[i].UpdateWaterCollision(other);
				}
			}
			UpdateWater = false;
		}
	}


}
