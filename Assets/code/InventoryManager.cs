using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Inventory; // ссылка на панель с инвентарём

    [SerializeField]
    private UIObject[] objects; //массив элементов UI, отображающих предметы

    private void Start()
    {
        Inventory.SetActive(false); // скрываем инвентарь
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // отслеживаем нажатие клавиши “I”
        {
            Inventory.SetActive(!Inventory.activeSelf); // инвертируем состояние инвентаря

            // обновляем предметы в инвентаре, если инвентарь открытый
            if (Inventory.activeSelf)
            {
                UpdateUI();
            }
        }
    }

    // публичный метод для добавления объекта в инвентарь
    public void AddItem(GameObject objectInScene)
    {
        foreach (UIObject obj in objects) // обходим массив UI объектов
        {
            if (objectInScene.name.Equals(obj.name))
            // если имя подобранного объекта совпадаем с именем одного из объектов в массиве
            {
                obj.State = true; // отмечаем объект в массиве как активный (подобранный)
                break; // выходим из цикла, если нашли подходящий объект
            }
        }

        // если после добавления элемента инвентарь был открыт - обновляем его
        if (Inventory.activeSelf)
        {
            UpdateUI();
        }
    }

    private void UpdateUI() // метод обновления инвентаря
    {
        foreach (UIObject obj in objects) // обходим массив объектов
        {
            obj.UpdateImage(); // обновляем каждый из объектов
        }
    }

    public bool CheckItems() // проверка, все ли объекты собраны
    {
        foreach (UIObject obj in objects) // обходим массив объектов
        {
            if (!obj.State)
            {
                return false; // если находим хоть один не активный - возвращаем false
            }
        }
        return true;
        // если цикл прошел по всем предметам и не был выявлен ни один не активный
    }
}
