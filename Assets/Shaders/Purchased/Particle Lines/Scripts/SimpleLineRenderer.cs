using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class SimpleLineRenderer : MonoBehaviour {
	private Mesh mesh;
	private int[] triangles;
	private Vector2[] uvs;
	public List<Vertex> points;
	private int currentVertexPointCount;
	[HideInInspector] public Transform cacheTransform;
	public LayerMask layerMask = -1;

	public bool hardEdge;

	void Awake() {
		mesh = new Mesh();
		points = new List<Vertex>();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		cacheTransform = transform;
	}

	void LateUpdate()
	{
		Camera[] c = Camera.allCameras;
		for (int i = 0; i < c.Length; i++)
		{
			if (c[i].isActiveAndEnabled && layerMask == (layerMask | (1 << c[i].gameObject.layer)))
				UpdateLineMesh(c[i]);
		}
	}

	void EditorInit(){
		if (points == null) points = new List<Vertex>();
		if (cacheTransform == null) cacheTransform = transform;
	}

	public void SetPosition(int index, Vector3 position) {
#if UNITY_EDITOR
		EditorInit();
#endif
		if (index < 0 || points == null || index >= points.Count) return;
		points[index].position = position;
	}

	public void SetWidth(int index, float width) {
#if UNITY_EDITOR
		EditorInit();
#endif
		if (index < 0 || index >= points.Count) return;
		points[index].width = width;
	}

	public void SetWidth(float startWidth, float endWidth) {
#if UNITY_EDITOR
		EditorInit();
#endif
		for (int i = 0; i < points.Count; i++) 
			points[i].width = Mathf.Lerp(startWidth, endWidth, (float)i / (points.Count - 1));
		
	}

	public void SetColor(Color startColor, Color endColor) {
#if UNITY_EDITOR
		EditorInit();
#endif
		for (int i = 0; i < points.Count; i++) 
			points[i].color = Color.Lerp(startColor, endColor, (float)i / (points.Count - 1));
		
	}

	public void SetColor(int index, Color color) {
#if UNITY_EDITOR
		EditorInit();
#endif
		if (index < 0 || index >= points.Count) return;
		points[index].color = color;
	}

	public void UpdateLineMesh(Camera targetCamera) {
#if UNITY_EDITOR
		EditorInit();
#endif
		cacheTransform.position = Vector3.zero;
		cacheTransform.rotation = Quaternion.identity;
		Vector3 localViewPos = transform.InverseTransformPoint(targetCamera.transform.position);
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		Color[] colors = mesh.colors;
		Vector3 prevTan = Vector3.zero;
		Vector3 prevDir = Vector3.zero;
		for (int i = 0; i < points.Count - 1; i++) {
			Vector3 faceNormal = (localViewPos - points[i].position).normalized;
			Vector3 dir = (points[i + 1].position - points[i].position);
			Vector3 tan = Vector3.Cross(dir, faceNormal).normalized;
			Vector3 offset;
			if (hardEdge){
				offset = (prevTan + tan).normalized * points[i].width / 2.0f;
				prevTan = tan;
			} else {
				offset = (tan).normalized * points[i].width / 2.0f;
			}

			vertices[i * 2] = points[i].position - offset;
			vertices[i * 2 + 1] = points[i].position + offset;
			normals[i * 2] = normals[i * 2 + 1] = faceNormal;
			colors[i * 2] = colors[i * 2 + 1] = points[i].color;
			if (i == points.Count - 2) {
				vertices[i * 2 + 2] = points[i + 1].position - tan * points[i + 1].width / 2.0f;
				vertices[i * 2 + 3] = points[i + 1].position + tan * points[i + 1].width / 2.0f;
				normals[i * 2 + 2] = normals[i * 2 + 3] = faceNormal;
				colors[i * 2 + 2] = colors[i * 2 + 3] = points[i + 1].color;
			}
			prevDir = dir;
		}
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.colors = colors;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
	}

	public void SetVertexCount(int vertexPointCount) {
		if (vertexPointCount != currentVertexPointCount) {
			currentVertexPointCount = vertexPointCount;
			if (mesh == null) mesh = new Mesh();
			mesh.Clear();
#if UNITY_EDITOR
			EditorInit();
#endif
			GetComponent<MeshFilter>().sharedMesh = mesh;
			if (points.Count > vertexPointCount)
				points.RemoveRange(vertexPointCount, points.Count - vertexPointCount);
			while (points.Count < vertexPointCount)
				points.Add(new Vertex());
			mesh.vertices = new Vector3[points.Count * 2];
			mesh.normals = new Vector3[points.Count * 2];
			mesh.colors = new Color[points.Count * 2];
			triangles = new int[(points.Count * 2) * 3];
			int j = 0;
			for (int i = 0; i < points.Count * 2 - 3; i += 2, j++) {
				triangles[i * 3] = j * 2;
				triangles[i * 3 + 1] = j * 2 + 1;
				triangles[i * 3 + 2] = j * 2 + 2;
				triangles[i * 3 + 3] = j * 2 + 1;
				triangles[i * 3 + 4] = j * 2 + 3;
				triangles[i * 3 + 5] = j * 2 + 2;
			}
			uvs = new Vector2[points.Count * 2];
			for (int i = 0; i < points.Count; i++) {
				uvs[i * 2] = uvs[i * 2 + 1] = new Vector2((float)i / (points.Count - 1), 0);
				uvs[i * 2 + 1].y = 1.0f;
			}
		}
	}
}

public class Vertex {
	public Vector3 position = Vector3.zero;
	public Color color = Color.white;
	public float width = 1.0f;
}