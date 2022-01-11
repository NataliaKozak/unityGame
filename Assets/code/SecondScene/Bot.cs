using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent botagent; // ссылка на агент навигации
    private Animator animbot; // ссылка на аниматор бота
    [SerializeField]
    private GameObject[] points; // массив точек для переходов
                                 //перечисление состояний бота
     GameObject player;
    float weight = 0;

    bool isStartDialog = false;
    bool isCharger = true;
    bool stopMess = true;
    bool startSecDialog = false;
    bool endOfFirstDialog = false;
    GameObject charger;

    [SerializeField] AudioSource firstDialog;
    [SerializeField] AudioSource snachZarad;
    [SerializeField] AudioSource nachIsp;

    private enum states
    {
        waiting, // ожидает
        going, // идёт
        dialog
    }
    states state = states.waiting; // изначальное состояние ожидания

    private void Start()
    {
        animbot = GetComponent<Animator>(); // берем компонент аниматора
        botagent = GetComponent<NavMeshAgent>(); // берем компонент агента
        StartCoroutine(Wait()); // запускаем корутину ожидания
        player = FindObjectOfType<ThirdPersonController>().gameObject;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case (states.waiting):
                {
                    if (PlayerNear())
                    {
                        PrepareToDialog();
                    }
                    break;
                }

            case states.going:
                {
                    if (PlayerNear())
                    {
                       
                        PrepareToDialog();
                    }
                    // если дистанция до пункта назначения меньше заданного расстояния (т.е. бот дошел до выданной ему точки)
                    else if ((Vector3.Distance(transform.position, botagent.destination)) < 0.6f)
                    {
                        StartCoroutine(Wait()); // вызываем корутину ожидания
                    }
                    break;
                }

            case states.dialog:
                {
                    if (!PlayerNear())
                    {
                        StartCoroutine(Wait());
                    }
                    break;
                }
        }
    }

    private bool PlayerNear()
    {
        return (Vector3.Distance(gameObject.transform.position, player.transform.position) < 1.4f);
    }

    private void PrepareToDialog()
    {
        botagent.SetDestination(transform.position); // обнуляем точку, чтобы бот никуда не шёл
        animbot.SetBool("walk", false); // останавливаем анимацию ходьбы
        state = states.dialog; // устанавливаем состояние подхода к объекту в который попали лучом    
        
    }



    private IEnumerator Wait() // корутина ожидания
    {
        botagent.SetDestination(transform.position); // обнуляем точку, чтобы бот никуда не шёл
        animbot.SetBool("walk", false); // останавливаем анимацию ходьбы
        state = states.waiting; // указываем, что бот перешел в режим ожидания

        yield return new WaitForSeconds(6f); // ждем 10 секунд

        botagent.SetDestination(points[Random.Range(0, points.Length)].transform.position);
        // destination – куда идти боту, передаем ему рандомно одну из наших точек
        animbot.SetBool("walk", true); // включаем анимацию ходьбы
        state = states.going; // указываем, что бот находится в движении 
    }

    private IEnumerator WaitSomeSec()
    {
        yield return new WaitForSeconds(22f);
        MainManagerSec.Messenger.WriteMessage("Найдите зарядку от планшета.");
        StartCoroutine(WaitSomeSec1());
    }

    private IEnumerator WaitSomeSec1()
    {
        yield return new WaitForSeconds(4f);
        endOfFirstDialog = true;
    }


    private void OnAnimatorIK()
    {
        if (state == states.dialog)
        {
            if (weight < 1)
            {
                weight += 0.01f;
            }
            
            Vector3 relativePos = player.transform.position - animbot.transform.position;
            Vector3 pov = new Vector3(relativePos.x, 0, relativePos.z);
            Quaternion newRot = Quaternion.LookRotation(pov);

            Vector3 relativePos1 = animbot.transform.position - player.transform.position;
            Vector3 pov1 = new Vector3(relativePos1.x, 0, relativePos1.z);
            Quaternion newRot1 = Quaternion.LookRotation(pov1);

            animbot.transform.rotation = Quaternion.Slerp(animbot.transform.rotation, newRot, Time.deltaTime * 1.5f);

            animbot.SetLookAtWeight(weight); // указываем силу воздействия на голову
            animbot.SetLookAtPosition(player.transform.TransformPoint(Vector3.up * 1.5f)); // указываем куда смотреть
            animbot.SetBool("dialog", true);



            charger = GameObject.Find("зарядка");
            if (charger)
                isCharger = true;
            else
                isCharger = false;

            if (!isStartDialog)
            {
                
                firstDialog.Play();
                StartCoroutine(WaitSomeSec());
                isStartDialog = true;
                stopMess = false;

            }




        }
        else if (weight > 0)
        {
            weight -= 0.01f;
            animbot.SetLookAtWeight(weight);
            animbot.SetLookAtPosition(player.transform.TransformPoint(Vector3.up * 1.5f));
            animbot.SetBool("dialog", false);
            stopMess = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "персонаж" ){
            charger = GameObject.Find("зарядка");
            if (charger)
                isCharger = true;
            else
                isCharger = false;

            if (isCharger && isStartDialog && endOfFirstDialog)
            {
                snachZarad.Play();
                stopMess = false;
            }

            if (stopMess && GameObject.Find("планшет") != null && !isCharger)
            {
                nachIsp.Play();
                StartCoroutine(WaitSomeSec1());
                MainManagerSec.Messenger.WriteMessage("Чтобы забрать планшет у шефа нажмите O.");
                stopMess = false;
            }

        }
    }



}
