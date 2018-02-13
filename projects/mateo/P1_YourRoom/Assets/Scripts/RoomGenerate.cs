using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class RoomGenerate : MonoBehaviour {

    public float width;
    public float length;
    [Range(2f, 3f)]
    public float height = 2f;

    public GameObject floor;
    public GameObject[] walls;
    public GameObject ceiling;
    public GameObject lamp;
    public GameObject[] props;

    struct SpotData {
        public Vector3 pos;
        public Quaternion rot1;
        public Quaternion rot2;
    }

    // Use this for initialization
    void Start() {
        GenerateBounds();

        Deck<SpotData> spots = GenerateSpots();

        GenerateProps(spots);
    }

    // Update is called once per frame
    void Update() {

    }

    void GenerateBounds() {
        // randomize room dimensions
        width = Random.Range(4f, 7f);
        length = 28f / width;

        // make the floor
        GameObject f = Instantiate(floor, transform.position + Vector3.down * 0.5f * height, Quaternion.identity);
        f.transform.SetParent(transform);
        f.transform.localScale = new Vector3(width, 1, length) * 0.1f;

        // make sure we have walls
        if (walls.Length != 4) {
            return;
        }

        // hard-coded translations and rotations for the walls
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

        // make walls (number magic to align properly because eww hard-coding)
        for (int i = 0; i < walls.Length; ++i) {
            x = i % 2 == 0 ? 1f : width;
            z = i % 2 == 1 ? 1f : length;

            // parallel to x or parallel to z?
            if (i % 2 == 0) {
                x = 1f;
                z = length;
                dx = width * 0.5f;
                dz = 1f;
            }
            else {
                x = width;
                z = 1f;
                dx = 1f;
                dz = length * 0.5f;
            }

            rot = Quaternion.Euler(euler[i]);

            f = Instantiate(walls[i], transform.position + delta[i] * dx * dz, rot);
            f.transform.SetParent(transform);
            f.transform.localScale = new Vector3(x == 1f ? height : x, 1f, z == 1f ? height : z) * 0.1f;
        }


        // flip ceiling and put it in
        rot = Quaternion.Euler(180, 0, 0);
        f = Instantiate(ceiling, transform.position + Vector3.up * 0.5f * height, rot);
        f.transform.SetParent(transform);
        f.transform.localScale = new Vector3(width, 1f, length) * 0.1f;

        // hey it's a lamp!
        f = Instantiate(lamp, transform.position + Vector3.up * 0.4f * height, Quaternion.identity);
        f.transform.SetParent(transform);
    }

    Deck<SpotData> GenerateSpots() {
        // deck of spots
        Deck<SpotData> spots = new Deck<SpotData>();

        // start at corner
        float x = transform.position.x - width / 2f;
        float z = transform.position.z - length / 2f;

        // start with a half-step offset to center coordinates
        float dx = width / 6f;
        float dz = length / 6f;

        SpotData curData = new SpotData();
        Vector3 curVec = Vector3.zero;
        Quaternion curRot = Quaternion.identity;

        // iterate through a grid
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                // current spot is corner + offsets
                curVec.x = x + dx;
                curVec.z = z + dz;

                curVec.y = 0f;

                // skip the center
                if (!(i == 1 && j == 1)) {
                    curData.pos = curVec;
                    SpotRotations(ref curData, i, j);
                    spots.Add(curData);
                }

                // increase x offset
                dx += width / 3f;
            }
            // reset x offset, increase z offset
            dx = width / 6f;
            dz += length / 3f;
        }
        // shuffle
        spots.Shuffle();

        // yay
        return spots;
    }

    void SpotRotations(ref SpotData curData, int i, int j) {
        // corner spots get two distinct rotation options
        // side spots get the same option twice
        if (i % 2 == 0 && j % 2 == 0) {
            // prefabs all face +z direction by default

            if (i == 2) { // top wall: rotate 180
                curData.rot1 = Quaternion.AngleAxis(180, Vector3.up);
            }
            else { // bottom wall: rotate 0
                curData.rot1 = Quaternion.identity;
            }

            if (j == 2) { // left wall: rotate 270
                curData.rot2 = Quaternion.AngleAxis(270, Vector3.up);
            }
            else { // right wall: rotate 90
                curData.rot2 = Quaternion.AngleAxis(90, Vector3.up);
            }
        }
        else {
            // Same as above but double
            if (i == 2) { // top
                curData.rot1 = curData.rot2 = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (i == 0) { // bottom
                curData.rot1 = curData.rot2 = Quaternion.identity;
            }
            else if (j == 2) { // left
                curData.rot1 = curData.rot2 = Quaternion.AngleAxis(270, Vector3.up);
            }
            else if (j == 0) { // right
                curData.rot1 = curData.rot2 = Quaternion.AngleAxis(90, Vector3.up);
            }
        }
    }

    void GenerateProps(Deck<SpotData> spots) {
        SpotData curSpot;
        Quaternion curRot;
        GameObject cur;

        // x y z offsets
        float dy = 0, dz = 0;

        // Iterate through props
        for (int i = 0; i < props.Length; ++i) {
            // draw a spot
            curSpot = spots.Draw();
            curRot = Random.Range(0f, 1f) < 0.5f ? curSpot.rot1 : curSpot.rot2;

            cur = Instantiate(props[i], curSpot.pos, curRot);

            // prop should have SizeInfo script attached
            SizeInfo si = cur.GetComponent<SizeInfo>();
            if (!si) {
                continue;
            }

            float dot = Vector3.Dot(cur.transform.forward, Vector3.forward);
            float align = 0;

            if (dot*dot < 0.1f) { // dot*dot around 0 -> align along X axis
                align = width / 6f;
            } else { // dot*dot around 1 -> align along Z axis
                align = length / 6f;
            }

            // align with floor
            dy = (si.height - height) * 0.5f;
            // align with wall
            dz = si.length * 0.5f - align;

            // apply alignment to local space
            cur.transform.Translate(0, dy, dz, Space.Self);

            cur.transform.SetParent(transform);
        }
    }
}
