using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKAnimation : MonoBehaviour
{
    private Animator anim; //переменная для ссылки на контроллер анимации
    private bool interact; // указывает, происходит ли взаимодействие
    private Vector3 positionForIК; // позиция объекта для взаимодействия
    private float weight = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // метод подобен Update, но используется для программных анимаций
    private void OnAnimatorIK()
    {
        if (interact)
        {
            // т.к. вес меняется от 0 до 1, а изначально мы задали 0,
            // то для плавного перехода руки от исходной позиции
            // достаточно плавно изменять вес ik анимации.
            if (weight < 1) weight += 0.001f;
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight); // заменим 1f на w
            anim.SetIKPosition(AvatarIKGoal.RightHand, positionForIК);
            //указываем позицию для направления левой руки
            anim.SetLookAtWeight(weight);
            //сначала устанавливаем вес анимации (1 – полностью перезаписывает существующую анимацию
            //и персонаж будет смотреть ровно на объект
            anim.SetLookAtPosition(positionForIК); //указываем куда нужно смотреть
        }
        else if (weight > 0) // добавим это условие для плавного изменения анимации при отдалении
        {
            weight -= 0.02f; // теперь нужно плавно убрать воздействие анимации
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, positionForIК);
            anim.SetLookAtWeight(weight);
            anim.SetLookAtPosition(positionForIК);
        }

    }


    public void StartInteraction(Vector3 pos)
    {
        positionForIК = pos;
        interact = true;
    }
    public void StopInteraction()
    {
        interact = false;
    }


}
