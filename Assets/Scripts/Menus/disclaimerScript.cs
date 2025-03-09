using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class disclaimerScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(warning());
    }
    private IEnumerator warning()
    {
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
