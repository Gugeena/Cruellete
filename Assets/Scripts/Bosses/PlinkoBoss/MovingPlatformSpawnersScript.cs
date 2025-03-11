using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSpawnersScript : MonoBehaviour
{
    public GameObject movingPlatform;
    public float time;
    private void Awake()
    {
        StartCoroutine(spawner());
    }
    private IEnumerator spawner()
    {
        Instantiate(movingPlatform, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(time);
        StartCoroutine(spawner());
    }
}
