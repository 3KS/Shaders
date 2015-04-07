using UnityEngine;
using System.Collections;

public class InteractWater : MonoBehaviour {
	Mesh mesh;
	private Vector3[] vertices;
	private Color[] vertexColors;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		mesh.MarkDynamic();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3[] vertices = mesh.vertices;
		Color[] colors = new Color[vertices.Length];
		int i = 0;
		while (i < vertices.Length) {
			colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
			i++;
		}
		mesh.colors = colors;
	}

	/**void OnTriggerStay(Collider other) {
		if(other.transform.tag.Equals("water")) {
			Debug.Log("Entered water");
			int i = 0;
			while (i < vertices.Length) {
				//Debug.Log("Itterated through mesh");
				if(other.bounds.Contains(transform.TransformPoint(vertices[i]*100))) {
					vertexColors[i] = new Color(1, 0, 0, 1);
					Debug.Log("It worked");
				}
				i++;
			}
			meshRenderer.mesh.colors = vertexColors;
		}
	}**/
}
