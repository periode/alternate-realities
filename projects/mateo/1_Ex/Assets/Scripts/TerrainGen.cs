using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour {

    MeshFilter mf;
    MeshRenderer mr;

    public int num = 10;
    public float scale = 1f;
    public Material mat;

    float perlinTime = 0;
    public float perlinStep = 0.01f;
    public float perlinAmp = 0.5f;

    List<Vector3> vert;
    List<Vector3> norm;
    List<int> tri;

	// Use this for initialization
	void Start () {
        mf = gameObject.AddComponent<MeshFilter>();

        Mesh terrain = new Mesh();
        vert = new List<Vector3>();
        norm = new List<Vector3>();

        Vector3 bias = new Vector3(-num * 0.5f * scale, 0, -num * 0.5f * scale);
        for (int y = 0; y < num; ++y) {
            for (int x = 0; x < num; ++x) {
                vert.Add(new Vector3(x*scale, 0, y*scale) + bias);
                norm.Add(Vector3.up);
            }
        }

        tri = new List<int>();
        for (int y = 0; y < num - 1; ++y) {
            for (int x = 0; x < num - 1; ++x) {
                tri.Add(Index(x, y));
                tri.Add(Index(x, y + 1));
                tri.Add(Index(x+1, y));

                tri.Add(Index(x+1, y));
                tri.Add(Index(x, y + 1));
                tri.Add(Index(x+1, y+1));
            }
        }

        terrain.SetVertices(vert);
        terrain.SetTriangles(tri, 0);

        mf.mesh = terrain;

        mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = mat;
    }

    int Index(int x, int y) {
        return x + num * y;
    }
	
	// Update is called once per frame
	void Update () {
        //mf.mesh.GetVertices(vert);

        Mesh m = mf.mesh;// new Mesh();

        float curX, curY, curZ;
        for (int y = 0; y < num; ++y) {
            for (int x  = 0; x < num; ++x) {
                curX = vert[Index(x, y)].x;
                curZ = vert[Index(x, y)].z;

                curY = perlinAmp*Mathf.PerlinNoise(x * perlinStep + perlinTime, y * perlinStep + perlinTime);

                vert[Index(x, y)] = new Vector3(curX, curY, curZ);
            }
        }

        m.SetVertices(vert);
        m.SetNormals(norm);
        m.SetTriangles(tri, 0);

        //mf.mesh = m;

        perlinTime += Time.deltaTime;
	}
}
