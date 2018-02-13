using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Tower {
    public Vector3 root;
    int tier = 0;
    Vector2 scale = new Vector2(1, 1);

    public Tower(Vector3 r) {
        root = r;
    }

    public void Add(GameObject g, Transform parent = null) {
        GameObject newG = GameObject.Instantiate(g, root + tier * Vector3.up, Quaternion.identity);
        newG.transform.localScale = new Vector3(scale.x, 1, scale.y);

        if (parent) {
            newG.transform.SetParent(parent);
        }

        ++tier;
        scale -= new Vector2(0.1f, 0.1f);
        if (scale == Vector2.zero) {
            scale = new Vector2(-0.1f, -0.1f);
        }
    }
}

public class SpawnScript : MonoBehaviour {

    public GameObject buildy;
    public GameObject ground;

    [Range(1, 50)]
    public int maxTowers = 10;
    [Range(0, 10)]
    public int multTowers = 3;
    [Range(1, 1000)]
    public int maxBlocks = 100;
    int curBlocks = 0;
    [Range(10, 50)]
    public int gridSize = 10;
    [Range(0, 10)]
    public float clearRadius = 3;

    Deck<Vector3> grid;
    Deck<Tower> towers;
    [Range(0.1f, 1f)]
    public float period = 1f;
    float timer = 0;

    // Use this for initialization
    void Start() {  
        grid = new Deck<Vector3>();

        Vector3 cur;
        for (int j = 0; j < gridSize; ++j) {
            for (int i = 0; i < gridSize; ++i) {
                cur = new Vector3(j - gridSize * 0.5f, 0, i - gridSize * 0.5f);
                if (cur.sqrMagnitude <= clearRadius*clearRadius) {
                    continue;
                }
                grid.Add(cur);
            }
        }

        grid.Shuffle();

        towers = new Deck<Tower>();

        Instantiate(ground, transform.position + 0.5f* Vector3.down, Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {
        if (timer >= period) {
            if (towers.Size < maxTowers * multTowers) {
                Vector3 newPos = grid.Draw();
                Tower newT = new Tower(newPos);
                newT.Add(buildy, transform);
                towers.Add(newT, multTowers);
                towers.Shuffle();

                ++curBlocks;
            } else if (curBlocks < maxBlocks) {
                Tower curT = towers.Draw();
                curT.Add(buildy, transform);

                ++curBlocks;
            }

            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
