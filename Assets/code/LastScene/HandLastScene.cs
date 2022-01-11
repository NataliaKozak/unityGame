using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLastScene : MonoBehaviour
{
    bool inHand;
    GameObject bag;
    Transform interactObject;

    [SerializeField]
    IKAnimation playerIK;

    [SerializeField] Animator animator;
    public MessangerLastScene mess;

    bool done;
    bool once = false;
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

        else
        {
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
        if (other.CompareTag("lastItem") && !once)
        {
            
            mess.WriteMessage("Чтобы рассмотреть следы нажмите E.");
            once = true;

            StartCoroutine(WaitSome());

            

        }
    }

    private IEnumerator WaitSome()
    {
        yield return new WaitForSeconds(2);
        MainManagerLast.Game.WinGame();
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
}
