using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe_Cube : MonoBehaviour
{
    /*
    Гомосексуалистам салам, остальным соболезную. Этот скрипт нужен для скроллинга предметов в
    магазине... НУ да, это все.
    */
    SceneManage sceneManage;
    //Массив кубов 
    [SerializeField] GameObject[] cubes;
    [SerializeField] GameObject[] meshes;
    [SerializeField] ScrollRect scrollRect;
    //Смещение при генерации 
    [SerializeField] Vector3 off;
    //Масштабирование. Тут хранится вектора размеров для каждого блока
    [SerializeField] Vector3[] cubesScale;
    //RectTransform объекта CUBES 
    [SerializeField] RectTransform cubesRect;
    //id куба
    [SerializeField] int id;
    //Скорость скорллинга
    [SerializeField] float snapSpeed;
    //Скорость масштабирования 
    [SerializeField] float scaleSpeed;
    //Дальность, на которой начинается масштабирование
    [SerializeField] float scaleOffset;
    bool is_scrolling;
    public bool[] bought_cube;
    int[] prices = { 0, 100, 100, 100, 100, 150, 150, 150, 150, 200, 200, 200, 200, 250, 250, 250, 250, 300, 300, 300, 300 };
    //bool touch;
    Vector2 cubesVector;
    [SerializeField] GameObject Select_Button;
    [SerializeField] GameObject Cound_txt;

    [SerializeField] Sprite[] by_Button_Sprites = new Sprite[3];
    [SerializeField] AudioSource soundSourse;
    [SerializeField] Audio audioo;
    [SerializeField] Menu menu;
    SpriteState spriteState = new SpriteState();
    string by_text;

    void OnEnable()
    {
        sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
        //Добавление в массив всех кубов и расстановка их  
        cubes = GameObject.FindGameObjectsWithTag("Cube");
        meshes = GameObject.FindGameObjectsWithTag("Mesh");
        cubesScale = new Vector3[cubes.Length];
        bought_cube = new bool[cubes.Length];
        for (int i = 0; i != cubes.Length; i++)
        {
            if (i == 0) continue;
            cubes[i].transform.localPosition = cubes[i - 1].transform.localPosition + off;
        }
        if (sceneManage.main_Mesh == null)
            MeshRe();
        if (PlayerPrefs.HasKey("by_cube"))
        {
            string tmp = PlayerPrefs.GetString("by_cube");
            for (int i = 0; i != tmp.Length; i++)
            {
                if (tmp[i] == '1' || i == 0)
                    bought_cube[i] = true;
                else
                    bought_cube[i] = false;
            }
        }
        else
        {
            string by = "";
            for (int i = 0; i != bought_cube.Length; i++)
            {
                by += '0';
            }
            PlayerPrefs.SetString("by_cube", by);
            print(by);
        }

        spriteState = Select_Button.GetComponent<Button>().spriteState;
        bought_cube[0] = true;
    }
    void Update()
    {
        float nearestPos = float.MaxValue;
        for (int i = 0; i != cubes.Length; i++)
        {
            //Это нахождение близжайшего к центру куба
            float distance = Mathf.Abs(cubesRect.anchoredPosition.x + cubes[i].transform.localPosition.x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                id = i;
            }
            //Изменение размера
            float scale = Mathf.Clamp(1 / (distance / off.x) * scaleOffset, 40, 55);
            cubesScale[i].x = Mathf.SmoothStep(cubes[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            cubesScale[i].y = Mathf.SmoothStep(cubes[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            cubesScale[i].z = Mathf.SmoothStep(cubes[i].transform.localScale.z, scale, scaleSpeed * Time.fixedDeltaTime);
            cubes[i].transform.localScale = cubesScale[i];
        }

        //Мы постепенно двигаем к центру тот блок, что находится ближе всех к центру
        if (is_scrolling) return;
        cubesVector.x = Mathf.SmoothStep(cubesRect.anchoredPosition.x, -cubes[id].transform.localPosition.x, snapSpeed * Time.fixedDeltaTime);
        cubesRect.anchoredPosition = cubesVector;

        //Нужно чтобы вовремя отключить инерцию во избежание подергиваний
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 1200 && !is_scrolling) scrollRect.inertia = false;

        if (sceneManage.selected_cube_id == id || bought_cube[id])
        {
            Select_Button.GetComponent<Image>().sprite = by_Button_Sprites[2];
            spriteState.pressedSprite = by_Button_Sprites[3];
        }
        else if (!bought_cube[id] && sceneManage.money_bal >= prices[id])
        {
            Select_Button.GetComponent<Image>().sprite = by_Button_Sprites[0];
            spriteState.pressedSprite = by_Button_Sprites[1];

        }
        else if (sceneManage.money_bal < prices[id])
        {
            Select_Button.GetComponent<Image>().sprite = by_Button_Sprites[4];
            spriteState.pressedSprite = by_Button_Sprites[4];
            cubes[id].transform.Find("Locker").gameObject.SetActive(true);
        }
        if (id <= 21 && id >= 0) by_text = prices[id].ToString();
        if (bought_cube[id]) by_text = "";
        if (PlayerPrefs.HasKey("New_Cube") && id == PlayerPrefs.GetInt("New_Cube")) by_text = "NEW";

        Select_Button.GetComponent<Button>().spriteState = spriteState;
        Cound_txt.GetComponent<Text>().text = (by_text);
        Cound_txt.transform.Find("Cond_Text_cube (1)").gameObject.GetComponent<Text>().text = (by_text);

    }
    //Вызывается тогда, когда происходит скроллинг
    public void Scrolling(bool scroll)
    {
        is_scrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
    void SoundPlay(AudioClip _clip)
    {
        soundSourse.clip = _clip;
        soundSourse.Play();
    }
    public void MeshRe()
    {
        if (PlayerPrefs.HasKey("New_Cube") && id == PlayerPrefs.GetInt("New_Cube"))
        {
            PlayerPrefs.DeleteKey("New_Cube");
        }
        SoundPlay(audioo.UI_clip[1]);
        if (bought_cube[id])
        {
            Select();
        }
        else if (sceneManage.money_bal >= prices[id])
        {
            SoundPlay(audioo.UI_clip[0]);
            sceneManage.money_bal -= prices[id];
            bought_cube[id] = true;
            //Select();
        }
        else
        {
            SoundPlay(audioo.UI_clip[2]);
        }
    }
    void Select()
    {
        sceneManage.main_Mesh = meshes[id].GetComponent<MeshFilter>().mesh;
        sceneManage.main_Material = meshes[id].GetComponent<Renderer>().material;
        sceneManage.selected_cube_id = id;
        PlayerPrefs.SetInt("selected_cube_id", id);
        PlayerPrefs.SetInt("money", sceneManage.money_bal);
        string by = "";

        for (int i = 0; i != bought_cube.Length; i++)
        {
            if (bought_cube[i]) by += '1';
            else by += '0';
        }
        PlayerPrefs.SetString("by_cube", by);
        menu.BackToMenu();
    }

    public void ButtonLeft()
    {
        scrollRect.normalizedPosition -= new Vector2(450, 0);
    }

    public void ButtonRight()
    {
        scrollRect.normalizedPosition += new Vector2(450, 0);

    }
}