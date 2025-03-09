using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineAnimationScript : MonoBehaviour
{
    private Animator animator;
    public string gameObjectName;

    void Start()
    {
       // gameObjectName = gameObject.name;
        animator = GetComponent<Animator>();
        StartCoroutine(RedAnimationCoroutine());
        //if (gameObjectName == "P-rank") StartCoroutine(RedAnimationCoroutine());
    }

    private IEnumerator RedAnimationCoroutine()
    {
        animator.SetBool("PlayClip", true);
        Debug.Log("PlayClip is true");
        yield return new WaitForSeconds(3f);
        animator.SetBool("PlayClip", false);
        Debug.Log("PlayClip is false");

        yield return new WaitForSeconds(0.1f); 

        RepeatCoroutine(RedAnimationCoroutine());
    }

    void RepeatCoroutine(IEnumerator n)
    {
        StartCoroutine(n);
    }
}
