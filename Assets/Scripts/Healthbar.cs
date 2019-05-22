using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    private float timer;

    private bool showing;

    public GameObject player;

    public Sprite[] numbers = {null, null, null, null, null};

    private Animator animator;

    private Image _healthImg;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerBase>().gameObject;
        _healthImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 3)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 3 && showing == false)
        {
            animator.SetBool("Showing", showing);
        }

    }

    public void changeHP(bool shake = false)
    {
        if (shake)
        {
            GetComponent<Shaker>().setShake(20, 0.5f, 10, Shaker.ShakeStyle.X);
        }

        if (player.GetComponent<PlayerBase>().HP > 0)
        {
            _healthImg.sprite = numbers[player.GetComponent<PlayerBase>().HP];
        }
        else if (player.GetComponent<PlayerBase>().HP <= 0)
        {
            _healthImg.sprite = numbers[0];
        }
        

        if (player.GetComponent<PlayerBase>().HP != player.GetComponent<PlayerBase>().MaxHP)
        {
            showing = true;
            animator.SetBool("Showing", showing);
        }
        else if (player.GetComponent<PlayerBase>().HP == player.GetComponent<PlayerBase>().MaxHP && showing == true)
        {
            timer = 0;
            showing = false;
        }
    }
}
