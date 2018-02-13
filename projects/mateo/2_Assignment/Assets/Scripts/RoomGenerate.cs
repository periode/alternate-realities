using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerate : MonoBehaviour {

    float width;
    float length;
    float height = 2f;

    public GameObject floor;
    public GameObject[] walls;
    public GameObject ceiling;
    public GameObject lamp;

    // Use this for initialization
    void Start() {
        width = Random.Range(3f, 6f);
        length = 18f / width;

        GameObject f = Instantiate(floor, transform.position + Vector3.down * 0.5f * height, Quaternion.identity);
        f.transform.SetParent(transform);
        f.transform.localScale = new Vector3(width, 1, length) * 0.1f;

        if (walls.Length != 4) {
            return;
        }

        float x, z, dx, dz;
        Quaternion rot;
        Vector3[] delta = {
            Vector3.right,
            Vector3.back,
            Vector3.left,
            Vector3.forward
        };
        Vector3[] euler = {
            new Vector3(0, 0, 90),
            new Vector3(90, 0, 0),
            new Vector3(0, 0, -90),
            new Vector3(-90, 0, 0)
        };
        for (int i = 0; i < walls.Length; ++i) {
            x = i % 2 == 0 ? 1 : width;
            z = i % 2 == 1 ? 1 : length;

            if (i % 2 == 0) {
                x = 1;
                z = length;
                dx = width * 0.5f;
                dz = 1;
            }
            else {
                x = width;
                z = 1;
                dx = 1;
                dz = length * 0.5f;
            }

            rot = Quaternion.Euler(euler[i]);

            f = Instantiate(walls[i], transform.position + delta[i] * dx * dz, rot);
            f.transform.SetParent(transform);
            f.transform.localScale = new Vector3(x == 1f ? height : x, 1, z == 1 ? height : z) * 0.1f;
        }

        rot = Quaternion.Euler(180, 0, 0);
        f = Instantiate(ceiling, transform.position + Vector3.up * 0.5f * height, rot);
        f.transform.SetParent(transform);
        f.transform.localScale = new Vector3(width, 1, length) * 0.1f;

        f = Instantiate(lamp, transform.position + Vector3.up * 0.4f * height, Quaternion.identity);
        f.transform.SetParent(transform);

        // TODO: Add prop instantiation (light, etc)
    }

    // Update is called once per frame
    void Update() {

    }
}
