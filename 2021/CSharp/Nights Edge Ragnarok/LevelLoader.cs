using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator anim;
    [SerializeField] private bool goToHub;
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" )
        {
            if (!goToHub){
                LoadNextLevel(SceneManager.GetActiveScene().buildIndex +1);
            }
            if (goToHub){
                SceneManager.LoadScene("hub");
            }
        }
    }
    public void LoadNextLevel(int levelIndex){
        StartCoroutine(LoadLevel(levelIndex));
    }
    IEnumerator LoadLevel(int levelIndex){
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelIndex);
    }
}
