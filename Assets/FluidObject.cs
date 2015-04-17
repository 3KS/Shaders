using UnityEngine;
using System.Collections;

[System.Serializable]
public class FluidObject 
{
	public enum AxisOrientation {
		XYZ,
		XZY,
		YXZ,
		YZX,
		ZXY,
		ZYX
	};
    public ParticleSystem system;
    public ParticleSystem.CollisionEvent[] collisionEvents = new ParticleSystem.CollisionEvent[16];
    //public ParticleCollisionEvent[] collisionEvents;
	public Transform meshObject;
	public AxisOrientation MeshAxisLayout;
	public float MeshScale;
	public bool Animated = true;
	//SkinnedMeshRenderer for if the mesh is animated which most likely is the case
	[HideInInspector]
	public SkinnedMeshRenderer skin;
	[HideInInspector]
	public MeshFilter filter;
	//Base mesh object
	[HideInInspector]
	public Mesh mesh;
	//Vertices of the mesh
	[HideInInspector]
	public Vector3[] vertices;
	//Vertex colors for the mesh
	[HideInInspector]
	public Color[] vertexColors;

    [HideInInspector]
    public float waterParticleSize;

	public void InitializeMesh() {
		if(Animated) {
			mesh = new Mesh ();
			skin = meshObject.gameObject.GetComponent<SkinnedMeshRenderer>();
			skin.BakeMesh(mesh);
			vertices = mesh.vertices;
			vertexColors = mesh.colors;

			for(int j = 0; j < vertices.Length; j++) {
				vertexColors[j] = new Color(1, 1, 1, 1);
			}
			skin.sharedMesh.colors = vertexColors;
		} else {
			filter = meshObject.GetComponent<MeshFilter>();
			mesh = filter.mesh;
			vertices = mesh.vertices;
			vertexColors = mesh.colors;

			for(int j = 0; j < vertices.Length; j++) {
				vertexColors[j] = new Color(1, 1, 1, 1);
			}
			filter.mesh.colors = vertexColors;
		}
		//mesh.MarkDynamic();
	}

	public void UpdateMesh() {
		if(Animated) {
			mesh = new Mesh ();
			skin.BakeMesh (mesh);
			vertices = mesh.vertices;
			vertexColors = skin.sharedMesh.colors;
		} else {
			mesh = filter.mesh;
			vertices = mesh.vertices;
			vertexColors = mesh.colors;
		}
	}

	public void UpdateWaterCollision(Collider water) {
		int j = 0;
		while (j < vertices.Length) {
			vertices [j] = CorrectAxis(vertices[j], MeshAxisLayout);
			//Vector3 vertexPos = new Vector3 (fluidObjects[i].meshObject.transform.position.x - fluidObjects[i].vertices [j].x * fluidObjects[i].MeshScale, fluidObjects[i].meshObject.transform.position.y - fluidObjects[i].vertices [j].y * fluidObjects[i].MeshScale, fluidObjects[i].meshObject.transform.position.z - fluidObjects[i].vertices [j].z * fluidObjects[i].MeshScale);
			Vector3 vertexPos;
			if(Animated) {
				vertexPos = new Vector3 (meshObject.transform.position.x - vertices [j].x * MeshScale, meshObject.transform.position.y - vertices [j].y * MeshScale, meshObject.transform.position.z - vertices [j].z * MeshScale);
			} else {
				vertexPos = meshObject.localToWorldMatrix.MultiplyPoint3x4(vertices[j]);
			}
			if (water.bounds.Contains (vertexPos)) {
				vertexColors [j] = new Color (0, 0, 1, 1);
			}
			j++;
		}
		if(Animated) {
			skin.sharedMesh.colors = vertexColors;
		} else {
			filter.mesh.colors = vertexColors;
		}
	}

    public void SetWaterParticleSize(float size)
    {
        waterParticleSize = size;
    }

    public void UpdateWaterCollisionObject(GameObject other)
    {
        //int safeLength = system.safeCollisionEventSize;
        int safeLength = 10;
        //if (collisionEvents.Length < safeLength)
            collisionEvents = new ParticleSystem.CollisionEvent[safeLength];
        int numCollisionEvents = system.GetCollisionEvents(other, collisionEvents);

        int j = 0;
        while (j < vertices.Length)
        {
            vertices[j] = CorrectAxis(vertices[j], MeshAxisLayout);
            //Vector3 vertexPos = new Vector3 (fluidObjects[i].meshObject.transform.position.x - fluidObjects[i].vertices [j].x * fluidObjects[i].MeshScale, fluidObjects[i].meshObject.transform.position.y - fluidObjects[i].vertices [j].y * fluidObjects[i].MeshScale, fluidObjects[i].meshObject.transform.position.z - fluidObjects[i].vertices [j].z * fluidObjects[i].MeshScale);
            Vector3 vertexPos;
            if (Animated)
            {
                vertexPos = new Vector3(meshObject.transform.position.x - vertices[j].x * MeshScale, meshObject.transform.position.y - vertices[j].y * MeshScale, meshObject.transform.position.z - vertices[j].z * MeshScale);
            }
            else
            {
                vertexPos = meshObject.localToWorldMatrix.MultiplyPoint3x4(vertices[j]);
            }
            for(int k = 0; k < collisionEvents.Length; k++) {
                float distance = Mathf.Sqrt(Mathf.Pow((vertexPos.x - collisionEvents[k].intersection.x), 2) +
                    Mathf.Pow((vertexPos.y - collisionEvents[k].intersection.y), 2) +
                    Mathf.Pow((vertexPos.z - collisionEvents[k].intersection.z), 2));
                if (distance < waterParticleSize)
                {
                    vertexColors[j] = new Color(0, 0, 1, 1);
                }
            }
            j++;
        }
        if (Animated)
        {
            skin.sharedMesh.colors = vertexColors;
        }
        else
        {
            filter.mesh.colors = vertexColors;
        }
    }

	private Vector3 CorrectAxis(Vector3 vertex, AxisOrientation axisLayout) {
		switch(axisLayout) {
		case AxisOrientation.XZY:
			return new Vector3(vertex.x, vertex.z, vertex.y);
		case AxisOrientation.YXZ:
			return new Vector3(vertex.y, vertex.x, vertex.z);
		case AxisOrientation.YZX:
			return new Vector3(vertex.y, vertex.z, vertex.x);
		case AxisOrientation.ZXY:
			return new Vector3(vertex.z, vertex.x, vertex.y);
		case AxisOrientation.ZYX:
			return new Vector3(vertex.z, vertex.y, vertex.x);
		default:
			return vertex;
		}
	}
}