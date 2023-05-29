using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTraffic : MonoBehaviour
{
    [HideInInspector] public SphereCollider farCar;
    [HideInInspector] public SpawnTraffic spawnTraffic;

    // Update is called once per frame
    void Update()
    {
        //if (!farCar.bounds.Contains(this.gameObject.transform.position))
        //{
        //    spawnTraffic.actualTrafficDensity -= 1;
        //    Destroy(this.gameObject);
        //}
    }
}
