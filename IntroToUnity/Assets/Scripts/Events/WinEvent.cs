using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinEvent : MonoBehaviour
{
    public int nextLevel;

    Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerWins"))
        {
            animator.SetBool("Transition", false);
        }
    }

    public void PlayerWins()
    {
        StartCoroutine(Win());
        animator.SetBool("Transition", true);
        animator.SetBool("playerWins", true);
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(nextLevel);
    }

}
