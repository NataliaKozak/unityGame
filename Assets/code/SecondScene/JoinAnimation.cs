using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinAnimation : MonoBehaviour
{
    public Animator animbot;//ссылка на аниматор двери  
    public Transform target;//ссылка на точку для начала анимации
    private Quaternion newRot;//требуемый поворот   
    private Animator anim;//аниматор персонажа
    private bool secondTurn = false;
    private States state;//текущее состояние   
    GameObject plansh;



    private enum States//перечисление состояний персонажа
    {
        wait,//ожидание
        turn,//поворот
        walk//перемещение
    }

    private void Start()
    {
        anim = GetComponent<Animator>();//инициализируем аниматор
        state = States.wait;     //изначально состояние ожидания
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.U))
        {
            int index = SceneManager.GetActiveScene().buildIndex; // берем индекс запущенной сцены
            index = (index == 2) ? 3 : 2; // меняем индекс с 0 на 1 или с 1 на 0
            StartCoroutine(AsyncLoad(index));
        }

        if (Input.GetKey(KeyCode.O))
        {
            GoToPoint();
        }

        


        switch (state)//переключаем в зависимости от состояния
        {

            case States.turn://при повороте к точке
                {
                    
                    transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 2);//интерполируем между начальным поворотом и требуемым
                    if (Mathf.Abs(Mathf.Round(newRot.y * 100)) == Mathf.Abs(Mathf.Round(transform.rotation.y * 100)))//проверяем когда персонаж повернулся
                    {
                        transform.rotation = newRot;//для избежания погрешности
                        if (!secondTurn)
                        {
                            state = States.walk;//переключаем состояние на перемещение
                            anim.SetFloat("MotionSpeed", 2);      //включаем анимацию ходьбы  
                            
                        }
                        else
                        {
                            
                            animbot.SetBool("charger", true);//запуск анимации бота
                            anim.SetBool("charger", true);//запуск анимации персонажа
                            secondTurn = !secondTurn;
                            state = States.wait;
                            plansh = GameObject.Find("планшет");
                            StartCoroutine(WaitSomeTime());

                            
                        }
                    }
                    break;
                }
            case States.walk:
                {
                    transform.position = transform.position + transform.forward * Time.deltaTime;//перемещаем персонажа прямо                  
                    if (Vector3.Distance(transform.position, target.position) <= 0.1)//дошел
                    {
                        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);//для исключения погрешности ставим в требуемую точку
                        anim.SetFloat("MotionSpeed", 0);//выключаем анимацию ходьбы
                        secondTurn = true;
                        state = States.wait;
                        GoToPoint();
                    }
                    break;
                }
        }

    }

    private IEnumerator WaitSomeTime()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("charger", false);
        Destroy(plansh);

        MainManagerSec.Messenger.WriteMessage("Для запуска игры нажмите U.");
    }

    




    public void GoToPoint()//функция для начала выполнения
    {
        
        if (state == States.wait)//если ждем
        {
            
            state = States.turn;//переходим в состояние поворота к точке
            Vector3 relativePos = new Vector3();
            if (!secondTurn)
            {
                relativePos = target.position - transform.position;//вычисляем координату куда нужно будет повернуться
                
            }
            else
            {
                Vector3 forward = target.transform.position + target.transform.forward;
                relativePos = new Vector3(forward.x, transform.position.y, forward.z) - transform.position;
            }
            newRot = Quaternion.LookRotation(relativePos);//указываем нужный поворот
        }

    }

    private IEnumerator AsyncLoad(int index)
    {
        AsyncOperation ready = null;
        ready = SceneManager.LoadSceneAsync(index);
        while (!ready.isDone) // пока сцена не загрузилась
        {
            yield return null; // ждём следующий кадр
        }
    }
}
