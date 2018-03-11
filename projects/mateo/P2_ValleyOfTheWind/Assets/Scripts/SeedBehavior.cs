using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : MonoBehaviour {

    Rigidbody rb;
    MeshRenderer mr;
    ParticleSystem ps;

    public GameObject[] trees;
    public GameObject spawnParticle;
    public AudioClip spawnSound;
    public Sprite seedSprite;
    public Color color;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        mr = GetComponentInChildren<MeshRenderer>();
        ps = GetComponent<ParticleSystem>();

        mr.material.color = color;
        ParticleSystem.MainModule m = ps.main;
        m.startColor = color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        GameObject effects = GameObject.Find("[Effects]");
        if (effects == null) {
            effects = new GameObject();
            effects.name = "[Effects]";
        }

        GameObject part = Instantiate(spawnParticle, transform.position, Quaternion.identity);
        ParticleSystem.MainModule p = part.GetComponentInChildren<ParticleSystem>().main;
        p.startColor = color;
        AudioSource audio = part.GetComponent<AudioSource>();
        if (audio) {
            audio.clip = spawnSound;
            audio.Play();
        }

        part.transform.SetParent(effects.transform);

        GameObject treeCont = GameObject.Find("[Trees]");
        if (treeCont == null) {
            treeCont = new GameObject();
            treeCont.name = "[Trees]";
        }

        GameObject treePrefab = trees[Random.Range(0, trees.Length)];

        GameObject tree = GameObject.Instantiate(treePrefab, transform.position + Vector3.down * 0.2f, Quaternion.identity);
        tree.transform.SetParent(treeCont.transform);

        Destroy(gameObject);
    }
}
