using System;
using UnityEngine;

public class CharAnimation : MonoBehaviour
{
    private Characters characters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characters = GetComponent<Characters>();
    }

    private void Update()
    {
        ChooseAnimation(characters);
    }

    private void ChooseAnimation(Characters c)
    {
        c.Anim.SetBool("IsIdle",false);
        c.Anim.SetBool("IsWalk",false);

        switch (c.State)
        {
            case CharState.Idle:
                c.Anim.SetBool("IsIdle",true);
                break;
            case CharState.Walk:
            case CharState.WalkToEnemy:
                c.Anim.SetBool("IsWalk",true);
                break;
            
               
        }
    }
}
