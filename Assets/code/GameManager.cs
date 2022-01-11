using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Coroutine end; // ссылка на запущенную корутину, чтобы не проиграть после выигрыша
    private GameObject user;
    private Animator userAnim;

    public void WinGame() // в случае выигрыша
    {
        if (end == null) // проверяем, была ли уже выиграна или проиграна игра
        {
            user = GameObject.Find("персонаж");
            userAnim = user.GetComponent<Animator>();
            userAnim.SetBool("win", true);
            MainManagerLast.Messenger.WriteMessage("Поздравляем, вы выиграли!");
            end = StartCoroutine(BeforeExit1()); // запускаем окончание игры через 4 секунды
        }
    }

    public void LoseGame() // в случае проигрыша
    {
        if (end == null)
        {
            user = GameObject.Find("персонаж");
            userAnim = user.GetComponent<Animator>();
            userAnim.SetBool("sadMood", true);
            MainManager.Messenger.WriteMessage("Вы проиграли!");
            end = StartCoroutine(BeforeExit());
        }
    }

    public void ExitGame() // выход из игры
    {
        Application.Quit();
    }

    private IEnumerator BeforeExit() // корутина перед выходом для прочтения последних сообщений
    {
        yield return new WaitForSeconds(10f);
        MainManager.sceneChanger.OpenNewScene(); // выходим в главное меню
    }

    private IEnumerator BeforeExit1() // корутина перед выходом для прочтения последних сообщений
    {
        yield return new WaitForSeconds(10f);
        MainManager.sceneChanger.OpenNewScene(); // выходим в главное меню
    }

}
