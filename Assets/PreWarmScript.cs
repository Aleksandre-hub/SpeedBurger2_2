using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreWarmScript : MonoBehaviour
{

    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Simulate(5.4f);
        ps.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
