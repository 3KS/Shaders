using UnityEngine;
using System.Collections;

public class InteractWater : MonoBehaviour {
	public Transform meshObject;
	private SkinnedMeshRenderer mesh;
	private Vector3[] vertices;
	private Color[] vertexColors;
	public Axis upAxis;

	public enum Axis {
		X,
		Y,
		Z
	};


	// Use this for initialization
	void Start () {
		mesh = meshObject.gameObject.GetComponent<SkinnedMeshRenderer>();
		vertices = mesh.sharedMesh.vertices;
		vertexColors = mesh.sharedMesh.colors;
		int i = 0;
		while (i < vertices.Length) {
			//Debug.Log("Itterated through mesh");
			vertexColors[i] = new Color(1, 1, 1, 1);

			i++;
		}
		mesh.sharedMesh.colors = vertexColors;

		//mesh.MarkDynamic();
	}
	
	// Update is called once per frame
	void Update () {

		vertices = mesh.sharedMesh.vertices;
		vertexColors = mesh.sharedMesh.colors;
		Debug.Log (vertices [0]);
		Debug.Log (vertices [1]);
		Vector3 vertexPos2 = meshObject.position + vertices[0];
		//vertexPos = Quaternion.Inverse(meshObject.rotation) * vertexPos;
		//Debug.Log (vertexPos2);
		int i = 0;
		while (i < vertices.Length) {
			//Debug.Log("Itterated through mesh");
			Vector3 vertexPos =  transform.localToWorldMatrix.MultiplyPoint3x4(vertices[i]);
			Vector3 tempPos = new Vector3(vertexPos.x, vertexPos.y, vertexPos.z);
			//vertexPos = meshObject.localToWorldMatrix.MultiplyPoint3x4(vertexPos);
			Quaternion temp = new Quaternion();
			temp.eulerAngles = new Vector3(90,0,0);
			vertexPos = temp * vertexPos;
			//Transform temp = meshObject;
			//while(temp.parent != null) {
			//	temp = temp.parent;
			//	vertexPos = Quaternion.Inverse(temp.rotation) * vertexPos;
			//}
			if(upAxis == Axis.X) {
				if(vertexPos.x < .01f) {
					vertexColors[i] = new Color(0, 0, 1, 1);
					//Debug.Log("It worked");
				} else {
					vertexColors[i] = new Color(1, 1, 1, 1);
				}
			} else if(upAxis == Axis.Y) {
				if(vertexPos.y < -.6f) {
					vertexColors[i] = new Color(0, 0, 1, 1);
					//Debug.Log("It worked");
				} else {
					vertexColors[i] = new Color(1, 1, 1, 1);
				}
			} else {
				if(tempPos.y < 0f) {
					vertexColors[i] = new Color(Mathf.Abs (tempPos.x), Mathf.Abs(tempPos.y), 1, 1);
					//Debug.Log("It worked");
				} else {
					vertexColors[i] = new Color(1, 1, 1, 1);
				}
			}

			i++;
		}
		mesh.sharedMesh.colors = vertexColors;
		/**int i = 0;
		while (i < vertices.Length) {
			colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
			i++;
		}
		mesh.mesh.colors = colors;**/
	}

	void OnTriggerStay(Collider other) {
		if(other.transform.tag.Equals("water")) {
			//Debug.Log("Entered water");
			int i = 0;
			while (i < vertices.Length) {
				//Debug.Log("Itterated through mesh");
				Vector3 vertexPos = meshObject.position + vertices[i];
				//vertexPos = Quaternion.Inverse(meshObject.rotation) * vertexPos;

				if(other.bounds.Contains(vertexPos)) {
					vertexColors[i] = new Color(0, 0, 1, 1);
					//Debug.Log("It worked");
				}
				i++;
			}
			mesh.sharedMesh.colors = vertexColors;
		}
	}
}
