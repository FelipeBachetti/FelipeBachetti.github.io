using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class StaticImagePlayer : MonoBehaviour
{
    private GameObject player, enemy;
    private VideoPlayer videoPlayer;
    private RawImage image;
    
    public bool isThereEnemy;

    private void Awake() {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        videoPlayer = FindObjectOfType<VideoPlayer>();
        image = GetComponent<RawImage>();
    }

    private void Update() {
        if(isThereEnemy){
            enemy = FindObjectOfType<Enemy>().gameObject;
            var tempColor = image.color;
            tempColor.a = (Mathf.Sqrt((player.transform.position - enemy.transform.position).sqrMagnitude))/500;
            image.color = tempColor;
            videoPlayer.SetDirectAudioMute(0, false);
        }else{
            var tempColor = image.color;
            tempColor.a = 0;
            image.color = tempColor;
            videoPlayer.SetDirectAudioMute(0, true);
        }
    }
}
