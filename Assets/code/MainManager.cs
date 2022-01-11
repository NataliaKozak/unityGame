using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{

    public static Messenger messenger; // ссылка на скрипт для вывода сообщений

    public static SceneChanger sceneChanger;

    private static InventoryManager inventory;

    public static GameManager game;

    public static GameManager Game
    {
        get
        {
            if (game == null)
                game = FindObjectOfType<GameManager>();
            return game;
        }
        private set
        {
            game = value;
        }
    }

    public static Messenger Messenger
    {
        get
        {
            if (messenger == null) // инициализация по запросу
            {
                messenger = FindObjectOfType<Messenger>();
            }

            return messenger;
        }
        private set
        {
            messenger = value;
        }
    }

    public static Messenger MessengerLastScene
    {
        get
        {
            if (messenger == null) // инициализация по запросу
            {
                messenger = FindObjectOfType<Messenger>();
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
