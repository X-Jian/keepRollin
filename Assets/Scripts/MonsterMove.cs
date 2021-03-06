﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MonsterMove : MonoBehaviour
{

    public float speed;
    private bool moveRight;
    private bool moveUp;
    private bool isPlayed;
    public Rigidbody rig;
    public float force;
    public bool isHorizontal;

    public float leftBorder;
    public float rightBorder;
    public float topBorder;
    public float downBorder;
    private float width;
    private float height;
    GameObject obj1;

    public AudioSource ac;

    float tempX = 0f;
    float tempY = 0f;


    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AudioSource>();
        isPlayed = false;
        moveRight = true;
        moveUp = true;
        if (isHorizontal)
        {
            rig.AddForce(this.transform.right * force);
        }
        else
        {
            rig.AddForce(this.transform.up * force);
        }
        obj1 = GameObject.Find("Ball");

        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f,
                                                              Mathf.Abs(-Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        width = rightBorder - leftBorder;
        height = topBorder - downBorder;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 player_postion1 = obj1.transform.position;

        float distance1 = (player_postion1 - this.transform.position).magnitude;

        if (distance1 < 0.37 && !isPlayed)
        {
            isPlayed = true;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameObject.FindGameObjectWithTag("PlayerBall").transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            PlayAudio(GetComponent<AudioSource>().clip);
        }


        tempX = Mathf.Clamp(transform.position.x, leftBorder, rightBorder);
        tempY = Mathf.Clamp(transform.position.y, downBorder, topBorder);
        transform.position = new Vector3(tempX, tempY, transform.position.z);
        

        if (isHorizontal)
        {
            if (moveRight == true && transform.position.x == rightBorder)
            {
                //this.transform.position += Vector3.left * speed * Time.deltaTime;
                rig.AddForce(-this.transform.right * force * 2);
                moveRight = false;
            }

            if (moveRight == false && transform.position.x == leftBorder)
            {
                rig.AddForce(this.transform.right * force * 2);
                moveRight = true;
            }
        }

        else {
            if (moveUp == true && transform.position.y == topBorder)
            {
                rig.AddForce(-this.transform.up * force * 2);
                moveUp = false;
            }

            if (moveUp == false && transform.position.y == downBorder)
            {
                rig.AddForce(this.transform.up * force * 2);
                moveUp = true;
            }
        }
    }

    public void PlayAudio(AudioClip clip, UnityAction callback = null, bool isLoop = false)
    {
        //获取自身音频文件进行播放并且不重复播放
        ac.clip = clip;
        ac.loop = isLoop;
        ac.Play();
        //执行协成获取音频文件的时间
        StartCoroutine(AudioPlayFinished(ac.clip.length, callback));
    }

    private IEnumerator AudioPlayFinished(float time, UnityAction callback)
    {


        yield return new WaitForSeconds(time);
        //声音播放完毕后之下往下的代码  

        GameObject.FindGameObjectWithTag("PlayerBall").transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneControlls.materialType = "0";
        SceneControlls.score = "0";


    }
}
