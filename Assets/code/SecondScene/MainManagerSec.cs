using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagerSec : MonoBehaviour
{
    public static MessengerSecScene messenger; // ссылка на скрипт для вывода сообщений

    public static SceneChanger sceneChanger;

    private static InventoryManager inventory;

    public static MessengerSecScene Messenger
    {
        get
        {
            if (messenger == null) // инициализация по запросу
            {
                messenger = FindObjectOfType<MessengerSecScene>();
            }

            return messenger;
        }
        private set
        {
            messenger = value;
        }
    }
    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        sceneChanger = GetComponent<SceneChanger>();
    }


    public static InventoryManager Inventory
    {
        get
        {
            if (inventory == null)
            {
                inventory = FindObjectOfType<InventoryManager>();
            }
            return inventory;
        }
        private set
        {
            inventory = value;
        }
    }
}
