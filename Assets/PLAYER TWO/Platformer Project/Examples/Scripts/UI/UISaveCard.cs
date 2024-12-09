using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISaveCard : MonoBehaviour
{
    public string nextScene;
    [Header("Text Formatting")]
    public string retriesFormat = "00";
    public string starsFormat = "00";
    public string coinsFormat = "000";
    public string dateFormat = "MM/dd/y hh:mm";

    [Header("Containers")]
    public GameObject dataContainer;
    public GameObject emptyContainer;

    [Header("UI Elements")]
    public Text retries;
    public Text stars;
    public Text coins;
    public Text createdAt;
    public Text updatedAt;
    public Button loadButton;
    public Button deleteButton;
    public Button newGameButton;

    protected int m_index;
    protected GameData m_data;

    public bool isFilled {  get; private set; }
    public virtual void Load()
    {
        Game.instance.LoadState(m_index, m_data);
        GameLoader.instance.Load(nextScene);

    }
    public virtual void Delete()
    {

    }

    public virtual void Create()
    {
        var data = GameData.Create();
        GameSaver.instance.Save(data, m_index);
        Fill(m_index, data);
        EventSystem.current.SetSelectedGameObject(loadButton.gameObject);
    }

    public virtual void Fill(int index, GameData data)
    {
        m_index = index;
        isFilled = (data != null);
        dataContainer.SetActive(isFilled);
        emptyContainer.SetActive(!isFilled);
        loadButton.interactable = isFilled;
        deleteButton.interactable = isFilled;
        newGameButton.interactable = !isFilled;

        if(data != null)
        {
            m_data = data;
            retries.text = data.retries.ToString(retriesFormat);
            stars.text = data.stars.ToString(starsFormat);
            coins.text = data.coins.ToString(coinsFormat);
            createdAt.text = DateTime.Parse(data.createdAt).ToString(dateFormat);
            updatedAt.text = DateTime.Parse(data.updatedAt).ToString(dateFormat);

        }

    }
    protected void Start()
    {
        loadButton.onClick.AddListener(Load);
        deleteButton.onClick.AddListener(Delete);
        newGameButton.onClick.AddListener(Create);
    }

}