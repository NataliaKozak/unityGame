﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Messenger : MonoBehaviour
{

    public Text message; // ссылка на текст
    public Image panel;
    private static Coroutine RunMessage; // ссылка на запущенную корутину

    private void Start()
    {
        // берем компонент текста, т.к. текст и скрипт находятся на одном объекте
        message = GetComponent<Text>();
        WriteMessage("Найдите портфель"); // напишите сюда первое сообщение для пользователя
    }

    public void WriteMessage(string text) // метод для запуска корутины с выводом сообщения
    {
        // проверка и остановка корутины, если она уже была запущена
        if (RunMessage != null) StopCoroutine(RunMessage);
        this.message.text = ""; // очистка строки
        panel.enabled = true;
                                // запуск корутины с выводом нового сообщения
                                
        
        RunMessage = StartCoroutine(Message(text));
    }

    private IEnumerator Message(string message) // корутина для вывода сообщений
    {
        this.message.text = message; // записываем сообщение
        yield return new WaitForSeconds(4f); // ждем 4 секунды
        this.message.text = ""; // очищаем строку
        panel.enabled = false;
    }

}
