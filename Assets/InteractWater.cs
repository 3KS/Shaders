using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// </summary>
public class InteractWater : MonoBehaviour {

	public FluidObject[] fluidObjects;
	public int UpdateDelay;
    public float waterParticleDiameter;
	private int DelayCount = 0;
	private bool UpdateWater = true;
    /// <summary>
    /// 
    /// </summary>
	void Start () {
		for (int i = 0; i < fluidObjects.Length; i++) {
			fluidObjects[i].InitializeMesh();
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	void LateUpdate () {
		if (DelayCount == UpdateDelay) {
			for (int i = 0; i < fluidObjects.Length; i++) {
				fluidObjects[i].UpdateMesh();
                fluidObjects[i].SetWaterParticleSize(waterParticleDiameter);
			}
			DelayCount = 0;
			UpdateWater = true;
		} else {
			DelayCount++;
		}
	}

    void OnParticleCollision(GameObject other)
    {
        if (UpdateWater)
        {
            if (other.transform.tag.Equals("water"))
            {
                for (int i = 0; i < fluidObjects.Length; i++)
                {
                    fluidObjects[i].UpdateWaterCollisionObject(this.gameObject);
                }
            }
            UpdateWater = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
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
