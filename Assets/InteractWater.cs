using UnityEngine;
using System.Collections;

public class InteractWater : MonoBehaviour {


	[System.Serializable]
	public class FluidObject 
	{
		public Transform meshObject;
		public AxisOrientation MeshAxisLayout;
		public float MeshScale;
		public bool Animated = true;
		//SkinnedMeshRenderer for if the mesh is animated which most likely is the case
		[HideInInspector]
		public SkinnedMeshRenderer skin;
		//Base mesh object
		[HideInInspector]
		public Mesh mesh;
		//Vertices of the mesh
		[HideInInspector]
		public Vector3[] vertices;
		//Vertex colors for the mesh
		[HideInInspector]
		public Color[] vertexColors;
	}

	public FluidObject[] fluidObjects;
	public int UpdateDelay;
	private int DelayCount = 0;
	private bool UpdateWater = true;

	public enum AxisOrientation {
		XYZ,
		XZY,
		YXZ,
		YZX,
		ZXY,
		ZYX
	};


	// Use this for initialization
	void Start () {
		for (int i = 0; i < fluidObjects.Length; i++) {
			if(fluidObjects[i].Animated) {
				fluidObjects[i].mesh = new Mesh ();
				fluidObjects[i].skin = fluidObjects[i].meshObject.gameObject.GetComponent<SkinnedMeshRenderer>();
				fluidObjects[i].skin.BakeMesh(fluidObjects[i].mesh);
				fluidObjects[i].vertices = fluidObjects[i].mesh.vertices;
				fluidObjects[i].vertexColors = fluidObjects[i].mesh.colors;
				int j = 0;
				while (j < fluidObjects[i].vertices.Length) {
					fluidObjects[i].vertexColors[j] = new Color(1, 1, 1, 1);
					j++;
				}
				fluidObjects[i].skin.sharedMesh.colors = fluidObjects[i].vertexColors;
			}
		}
		//mesh.MarkDynamic();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (DelayCount == UpdateDelay) {
			for (int i = 0; i < fluidObjects.Length; i++) {
				Debug.Log("Updated Mesh");
				fluidObjects[i].mesh = new Mesh ();
				fluidObjects[i].skin.BakeMesh (fluidObjects[i].mesh);
				fluidObjects[i].vertices = fluidObjects[i].mesh.vertices;
				fluidObjects[i].vertexColors = fluidObjects[i].skin.sharedMesh.colors;
			}
			DelayCount = 0;
			UpdateWater = true;
		} else {
			Debug.Log("Didn't Update");
			DelayCount++;
		}
	}

	void OnTriggerStay(Collider other) {
		if (UpdateWater) {
			if (other.transform.tag.Equals ("water")) {
				for (int i = 0; i < fluidObjects.Length; i++) {
					int j = 0;
					while (j < fluidObjects[i].vertices.Length) {
						fluidObjects[i].vertices [j] = CorrectAxis(fluidObjects[i].vertices[j], fluidObjects[i].MeshAxisLayout);
						Vector3 vertexPos = new Vector3 (fluidObjects[i].meshObject.transform.position.x - fluidObjects[i].vertices [j].x * fluidObjects[i].MeshScale, fluidObjects[i].meshObject.transform.position.y - fluidObjects[i].vertices [j].y * fluidObjects[i].MeshScale, fluidObjects[i].meshObject.transform.position.z - fluidObjects[i].vertices [j].z * fluidObjects[i].MeshScale);

						if (other.bounds.Contains (vertexPos)) {
							fluidObjects[i].vertexColors [j] = new Color (0, 0, 1, 1);
						}
						j++;
					}
						fluidObjects[i].skin.sharedMesh.colors = fluidObjects[i].vertexColors;
					}
				}
				UpdateWater = false;
			}
	}

	private Vector3 CorrectAxis(Vector3 vertex, AxisOrientation axisLayout) {
		switch(axisLayout) {
		case AxisOrientation.XZY:
			return new Vector3(vertex.x, vertex.z, vertex.y);
			break;
		case AxisOrientation.YXZ:
			return new Vector3(vertex.y, vertex.x, vertex.z);
			break;
		case AxisOrientation.YZX:
			return new Vector3(vertex.y, vertex.z, vertex.x);
			break;
		case AxisOrientation.ZXY:
			return new Vector3(vertex.z, vertex.x, vertex.y);
			break;
		case AxisOrientation.ZYX:
			return new Vector3(vertex.z, vertex.y, vertex.x);
			break;
		default:
			return vertex;
		}
	}
}
