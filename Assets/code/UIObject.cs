using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObject : MonoBehaviour
{
    public GameObject objectInScene; // соответствующий объект на сцене
    [SerializeField]
    private Image imagePlace; // место для картинки
    [SerializeField]
    private Sprite image; // картинка
    private Image borderplace; // место для обводки
                               // ссылки на текстуры для обводки
    [SerializeField]
    private Sprite red; // красная обводка
    [SerializeField]
    private Sprite green; // зеленая обводка
    public bool State { get; set; } // автоматич свойство состояние подобран/не подобран объект

    private void OnEnable()
    {
        borderplace = gameObject.GetComponent<Image>();
        // инициализация должна произойти до отключения объекта,
        // поэтому OnEnable, а не Start       
    }

    public void UpdateImage() // обновить картинку в зависимости от состояния
    {
        if (State) // если объект активен (подобран)
        {
            imagePlace.sprite = image; // отобразить картинку
            borderplace.sprite = green; // сделать обводку зеленой
        }
        else // если объект еще не подобран
        {
            imagePlace.sprite = null; // не отображать картинку
            borderplace.sprite = red; // сделать обводку красной
        }
    }

}
