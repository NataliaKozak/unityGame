using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{

    private Animator anim;
    private bool move;
    private CharacterController controller;
    private float speedMove = 1f;
    private float speedTurn = 40f;
    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Animate(Input.GetAxis("Vertical") > 0);


        if (Input.GetMouseButtonDown(0))
        {
            //anim.SetTrigger("user_anim");
            anim.SetTrigger("lift");
            Debug.Log("ff");
        }




        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        Move(verticalInput);
        Turn(horizontalInput);
        Animate(verticalInput > 0);
    }
    private void Animate(bool input)
    {
        if (input != move)
        {
            move = input;
            anim.SetBool("walking", move);
        }

    }


    private void Move(float input)
    {
        // вычисляем вектор направления движения (-1f для эффекта гравитации)
        var movement = new Vector3(0f, -1f, input);
        movement = movement * speedMove * Time.deltaTime; // учитываем скорость и время
                                                          // применяем смещение к контроллеру для передвижения
        controller.Move(transform.TransformDirection(movement));
    }


    private void Turn(float input)
    {
        var turn = input * speedTurn * Time.deltaTime; // выч-м величину поворота
        transform.Rotate(0f, turn, 0f); // поворачиваем на нужный угол
    }




}

