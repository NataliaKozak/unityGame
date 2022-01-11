using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    bool inHand;
    GameObject bag;
    Transform interactObject;

    [SerializeField]
    IKAnimation playerIK;

    [SerializeField] Animator animator;

    GameObject forCheckObj1;
    GameObject forCheckObj2;
    GameObject forCheckObj3;
    bool dnevn = false;
    bool blockn = false;
    bool ulika = false;

    bool play = false;
    void TakeItemInHand(Transform item)
    {
        interactObject = item;
        interactObject.parent = transform; // делаем руку, родителем объекта
        interactObject.localPosition = new Vector3(0.089f, -0.017f, -0.002f); // устанавливаем положение
        interactObject.localEulerAngles = new Vector3(-124.749f, 170.053f, -70.479f);
        bag = GameObject.Find("портфель");
        bag.GetComponent<Rigidbody>().useGravity = false;
        bag.GetComponent<Rigidbody>().isKinematic = true;
        inHand = true;
        playerIK.StopInteraction();
        //MainManager.Messenger.WriteMessage("Вы подобрали " + item.name + "!"); //" нажмите 'i', чтобы увидеть список необходимых предметов"
        MainManager.Messenger.WriteMessage("Найдите улику, дневник шефа и блокнот!");
    }

    void TakeItemInPocket(GameObject item)
    {

        if (inHand)
        {
            interactObject = item.transform;
            playerIK.StopInteraction();

            
            MainManager.Messenger.WriteMessage("Теперь " + item.name + " в портфеле");
            MainManager.Inventory.AddItem(interactObject.gameObject);

            Destroy(interactObject.gameObject);

          
                

            
        }

        else { 
            //animator.SetTrigger("start");
            MainManager.Messenger.WriteMessage("Сначала найдите портфель ");
        }
        
    }

    void ThroughItem()
    {
       
        if (inHand)
        {
            interactObject.parent = null; // отвязываем объект
            bag = GameObject.Find("портфель");
            bag.GetComponent<Rigidbody>().useGravity = true;
            bag.GetComponent<Rigidbody>().isKinematic = false;
           // MainManager.Inventory.RemoveItem(key);//удаляем объект из инвенторя
            StartCoroutine(ReadyToTake()); // запускаем корутину
        }
    }

    IEnumerator ReadyToTake()
    {
        yield return null;
        inHand = false;//обнуляем ссылку
    }

    private void OnCollisionEnter(Collision collision) // когда случится коллизия с коллайдером предмета 
    {
        if (collision.gameObject.CompareTag("itemForTransfer") && !inHand) // если это портфель и левая рука и не взят объект
        {
            TakeItemInHand(collision.gameObject.transform);
            bag.GetComponent<Rigidbody>().useGravity = false;
            bag.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (collision.gameObject.CompareTag("item"))
        {
            
            TakeItemInPocket(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("zip"))
        {
            interactObject = collision.gameObject.transform;
            playerIK.StopInteraction();

            
            MainManagerSec.Messenger.WriteMessage("Отлично! Вернитесь к шефу чтобы вернуть зарядку.");

            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("item") || ((other.CompareTag("itemForTransfer")) && !inHand))
        {
            if (!inHand)
            {
                //animator.SetTrigger("start");
                MainManager.Messenger.WriteMessage("Чтобы подобрать портфель нажмите E.");

            }
            interactObject = other.transform;
            playerIK.StartInteraction(other.gameObject.transform.position);

        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            ThroughItem();
        }

        CheckDistance();
    }

    void CheckDistance()
    {
        if (!inHand && interactObject != null && Vector3.Distance(transform.position, interactObject.position) > 1f)
        {
            interactObject = null;
            playerIK.StopInteraction();
        }
    }

    private void Update()
    {
        forCheckObj1 = GameObject.Find("дневник шефа");
        if (!forCheckObj1)
        {
            dnevn = true;
        }

        forCheckObj2 = GameObject.Find("блокнот");
        if (!forCheckObj2)
        {
            blockn = true;
        }

        forCheckObj3 = GameObject.Find("улика");
        if (!forCheckObj3)
        {
            ulika = true;
        }


        if (dnevn && blockn && ulika)
        {
            
                StartCoroutine(WaitSome());
             
        }
           
    }

    private IEnumerator WaitSome() // корутина перед выходом для прочтения последних сообщений
    {
        yield return new WaitForSeconds(4f);
        MainManager.Messenger.WriteMessage("Теперь вы можете перейти в другую комнату!");
    }
   
}

