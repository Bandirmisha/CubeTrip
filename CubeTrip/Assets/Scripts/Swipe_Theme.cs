using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe_Theme : MonoBehaviour
{
    /*
    Гомосексуалистам салам, остальным соболезную. Этот скрипт нужен для скроллинга предметов в
    магазине... НУ да, это все.
    */
    SceneManage sceneManage;
    //Массив тем 
    GameObject[] themes;
    [SerializeField] ScrollRect scrollRect;
    //Фон стрелки вправо
    [SerializeField] GameObject rightImg;
    //Фон стрелки влево
    [SerializeField] GameObject leftImg;
    //Смещение при генерации 
    [SerializeField] Vector3 off;
    //Масштабирование. Тут хранится вектора размеров для каждой темы
    [SerializeField] Vector3[] themeScale;
    //RectTransform объекта THEME 
    [SerializeField] RectTransform themeRect;
    //id куба
    [SerializeField] int id;
    //Скорость скорллинга
    [SerializeField] float snapSpeed;
    //Скорость масштабирования 
    [SerializeField] float scaleSpeed;
    //Дальность, на которой начинается масштабирование
    [SerializeField] float scaleOffset;

    [SerializeField] Sprite open;
    [SerializeField] Sprite closed;
    bool is_scrolling;
    bool touch;
    public bool[] bought_theme;
    int[] prices = { 0, 200, 200, 250, 250, 500, 500, 500, 500, 750, 750, 750, 750, 1000 };
    Vector2 themeVector;
    [SerializeField] GameObject Select_Button;
    [SerializeField] GameObject Cound_txt;
    [SerializeField] Sprite[] by_Button_Sprites = new Sprite[3];
    [SerializeField] AudioSource soundSourse;
    [SerializeField] Audio audioo;
    [SerializeField] Menu menu;
    [SerializeField] GameObject textSELECTED;
    [SerializeField] GameObject textNEW;
    SpriteState spriteState = new SpriteState();
    string by_text;

    void OnEnable()
    {
        sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
        //Добавление в массив всех тем и расстановка их  
        themes = GameObject.FindGameObjectsWithTag("Theme");
        themeScale = new Vector3[themes.Length];
        bought_theme = new bool[themes.Length];
        for (int i = 0; i != themes.Length; i++)
        {
            if (i == 0) continue;
            themes[i].transform.localPosition = themes[i - 1].transform.localPosition + off;
        }
        if (sceneManage.main_Mesh == null)
            MeshRe();
        if (PlayerPrefs.HasKey("by_theme"))
        {
            string tmp = PlayerPrefs.GetString("by_theme");
            for (int i = 0; i != tmp.Length; i++)
            {
                if (tmp[i] == '1' || i == 0)
                    bought_theme[i] = true;
                else
                    bought_theme[i] = false;
            }
        }
        else
        {
            string by = "";
            for (int i = 0; i != bought_theme.Length; i++)
            {
                by += '0';
            }
            PlayerPrefs.SetString("by_theme", by);
            print(by);
        }
        spriteState = Select_Button.GetComponent<Button>().spriteState;
        bought_theme[0] = true;

        LockedSet();
    }
    void Update()
    {
        float nearestPos = float.MaxValue;
        for (int i = 0; i != themes.Length; i++)
        {
            //Это нахождение близжайшего к центру куба
            float distance = Mathf.Abs(themeRect.anchoredPosition.x + themes[i].transform.localPosition.x);
            if (distance < nearestPos && !touch)
            {
                nearestPos = distance;
                id = i;
            }
            //Изменение размера
            float scalex = Mathf.Clamp(1 / (distance / off.x) * scaleOffset, 15, 28);
            float scaley = Mathf.Clamp(1 / (distance / off.x) * scaleOffset, 15, 28);
            float scalez = Mathf.Clamp(1 / (distance / off.x) * scaleOffset, 0, 0);

            themeScale[i].x = Mathf.SmoothStep(themes[i].transform.localScale.x, scalex, scaleSpeed * Time.fixedDeltaTime);
            themeScale[i].y = Mathf.SmoothStep(themes[i].transform.localScale.y, scaley, scaleSpeed * Time.fixedDeltaTime);
            themeScale[i].z = Mathf.SmoothStep(themes[i].transform.localScale.z, scalez, scaleSpeed * Time.fixedDeltaTime);
            themes[i].transform.localScale = themeScale[i];
        }
        if (sceneManage.selected_theme_id == id || bought_theme[id])
        {
            Select_Button.GetComponent<Image>().sprite = by_Button_Sprites[2];
            spriteState.pressedSprite = by_Button_Sprites[3];
            Cound_txt.SetActive(false);

        }
        else if (!bought_theme[id] && sceneManage.money_bal >= prices[id])
        {
            Select_Button.GetComponent<Image>().sprite = by_Button_Sprites[0];
            spriteState.pressedSprite = by_Button_Sprites[1];
            Cound_txt.SetActive(true);

        }
        else if (sceneManage.money_bal < prices[id])
        {
            Select_Button.GetComponent<Image>().sprite = by_Button_Sprites[4];
            spriteState.pressedSprite = by_Button_Sprites[4];
            Cound_txt.SetActive(true);

        }
        //if (id <= 21 && id >= 0)

        by_text = prices[id].ToString();

        if (bought_theme[id])
            by_text = "";

        if (sceneManage.selected_theme_id == id)
        {
            Cound_txt.SetActive(false);
            textSELECTED.SetActive(true);
        }
        else
        {
            textSELECTED.SetActive(false);
        }
/*
        if (PlayerPrefs.HasKey("New_Theme") && id == PlayerPrefs.GetInt("New_Theme"))
            textNEW.SetActive(true);
        else
            textNEW.SetActive(false);
*/
        Select_Button.GetComponent<Button>().spriteState = spriteState;
        Cound_txt.GetComponent<Text>().text = (by_text);
        Cound_txt.transform.Find("Cond_Text_theme (1)").gameObject.GetComponent<Text>().text = (by_text);

        //Мы постепенно двигаем к центру тот блок, что находится ближе всех к центру
        if (is_scrolling) return;
        themeVector.x = Mathf.SmoothStep(themeRect.anchoredPosition.x, -themes[id].transform.localPosition.x, snapSpeed * Time.fixedDeltaTime);
        themeRect.anchoredPosition = themeVector;

        //Нужно чтобы вовремя отключить инерцию во избежание подергиваний
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 1200 && !is_scrolling) scrollRect.inertia = false;


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
        if (PlayerPrefs.HasKey("New_Theme") && id == PlayerPrefs.GetInt("New_Theme"))
        {
            PlayerPrefs.DeleteKey("New_Theme");
        }
        SoundPlay(audioo.UI_clip[1]);
        if (bought_theme[id])
        {
            Select();
        }
        else if (sceneManage.money_bal >= prices[id])
        {
            SoundPlay(audioo.UI_clip[0]);
            sceneManage.money_bal -= prices[id];
            bought_theme[id] = true;
            Save();
            var l = themes[id].transform.Find("Locker");
            l.gameObject.SetActive(false);
            LockedSet();
        }
        else
        {
            SoundPlay(audioo.UI_clip[2]);
        }
    }
    void Select()
    {
        sceneManage.selected_theme_id = id;
        sceneManage.select_theme = themes[id];
        Save();
        menu.BackToMenu();
    }
    void Save()
    {
        PlayerPrefs.SetInt("selected_theme_id", id);
        PlayerPrefs.SetInt("money", sceneManage.money_bal);
        string by = "";

        for (int i = 0; i != bought_theme.Length; i++)
        {
            if (bought_theme[i]) by += '1';
            else by += '0';
        }
        PlayerPrefs.SetString("by_theme", by);
    }
    public void ButtonLeft()
    {
        scrollRect.horizontalNormalizedPosition -= 400;
    }

    public void ButtonRight()
    {
        scrollRect.horizontalNormalizedPosition += 400;
    }
    void LockedSet()
    {
        for (int i = 0; i != themes.Length; i++)
        {
            var l = themes[i].transform.Find("Locker");

            if (sceneManage.money_bal < prices[i] && !bought_theme[i])
            {
                l.gameObject.SetActive(true);
                l.GetComponent<SpriteRenderer>().sprite = closed;
            }
            else
            if (sceneManage.money_bal >= prices[i] && !bought_theme[i])
            {
                l.gameObject.SetActive(true);
                l.GetComponent<SpriteRenderer>().sprite = open;
            }
            else
            {
                l.gameObject.SetActive(false);
            }

        }

    }
}
