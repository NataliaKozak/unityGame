using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkService : MonoBehaviour
{
    private Material maintexture; // ссылка на текстуру экрана

    private void Start()
    {
        maintexture = GetComponent<Renderer>().material; // инициализируем ссылку на текстуру
        StartCoroutine(ShowImages()); // запускаем корутину, сменяющую изображения
    }
    //массив из 10 изображений для загрузки, замените ссылки на свои!
    private string[] webImages =
    {
      "https://sun9-9.userapi.com/impf/c849236/v849236186/1b8c5b/RDJDNR3pwrc.jpg?size=2560x1707&quality=96&sign=c5a01978f297acc7c27417a8b4870513&type=album",
       "https://sun9-1.userapi.com/impf/c851520/v851520049/152153/s8iga3qyHUM.jpg?size=2560x1707&quality=96&sign=507c34444ff3ebe7549d562daa070cb0&type=album",
       "https://sun9-9.userapi.com/impf/tAFDE_43FJ-L7M-JI52gTa7gRaQp41YD5h_Fnw/bh_goXjA2xw.jpg?size=2560x1707&quality=96&sign=6da2898cbc39abfe51360246ca65f2b0&type=album",
       "https://sun9-31.userapi.com/impg/W-oyRpye8PPG68XbjM4a1v_imQ3DnwuifI3AKQ/rU5j2znBhQE.jpg?size=2560x1713&quality=96&sign=05a97ed7537c59db52b3973b4a380f5b&type=album",
        "https://sun9-2.userapi.com/impg/QtRdm2VwFs7YTsswa7apz_RDXB4Pvkk5USk3NQ/1MlJmAPbUw4.jpg?size=2560x1713&quality=96&sign=4a871aa3edda39c1623ff0d2e4fa0e5a&type=album",
        "https://sun9-23.userapi.com/impg/hLhOtujXziZ-UG_UEmh8OeJXcd9-tyqPSklaSg/X2lH-oqpCTA.jpg?size=2560x1707&quality=96&sign=e72070292e4ee7292683e776958340a9&type=album",
        "https://sun9-86.userapi.com/impg/QU_m6YsigcZgf_SPbXQxTA10y3PYErk8dJ8izA/GH4kCOxBA08.jpg?size=2560x1697&quality=96&sign=2686b1ebb4ffd4dea85366b63344b431&type=album",
       "https://sun9-46.userapi.com/impg/kVO0fgtALiNOGrmZTAXj1om5RIjwL0pvVB6q9w/g14UkBqGjf8.jpg?size=2560x1697&quality=96&sign=509d29eabf093c32cbb9240965c1eb00&type=album",
        "https://sun9-38.userapi.com/impg/OIG4yIeht3TfsnFMY18hS5p0QkfmojwjtX7exw/sSuuVyvuCS0.jpg?size=2560x1697&quality=96&sign=e82cb879739c6793501212cc89cc9c59&type=album",
   "https://sun9-56.userapi.com/impf/c849236/v849236186/1b8be3/AJtUPJe3njU.jpg?size=2560x1707&quality=96&sign=076c36408e65e35469ae99aac80da85c&type=album"
};

    private Texture[] Images = new Texture[10]; // массив из загруженных изображений
    private int i = 0; // счетчик, чтобы знать какое изображение показывается

    private IEnumerator ShowImages() // корутина смены изображений
    {
        while (true)
        {
            if (Images[i] == null) // если требуемой текстуры нет в массиве
            {
                WWW www = new WWW(webImages[i]); // загружаем изображение по ссылке      
                yield return www; // ждем когда изображение загрузится
                Images[i] = www.texture; // записываем загруженную текстуру в массив
            }
            maintexture.mainTexture = Images[i]; // устанавливаем текстуру из массива изображений
            i++; // увеличиваем счетчик
            if (i == 10)
            {
                i = 0; // если загрузили уже 9, возвращаемся к первому
            }
            yield return new WaitForSeconds(3f); // ждем 3 секунды между сменой изображений
        }
    }

}
