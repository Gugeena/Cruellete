using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class disclaimerScript : MonoBehaviour
{
    public float time;

    private void Start()
    {
        StartCoroutine(warning());
    }
    private IEnumerator warning()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
