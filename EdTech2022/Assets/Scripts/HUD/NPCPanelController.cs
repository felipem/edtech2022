﻿//using Audio;
using UnityEngine;
using UnityEngine.UI;

public class NPCPanelController : MonoBehaviour
{
    public Text npcName;
    public Text npcOccupation;
    public Text npcTweet;
    private Image npcAvatar;

    private static NPCPanelController instance;

    public static NPCPanelController Instance => instance;

    private void Awake()
    {
        // find child image
        npcAvatar = GetComponentsInChildren<Image>().Length > 0
            ? GetComponentsInChildren<Image>()[1]
            : GetComponentsInChildren<Image>()[0];

        // set singleton instance
        instance = this;
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        //AudioManager.Instance.EndConversation();
    }

    public void Show(string name, string occupation, string tweet, Sprite avatar)
    {
        // show npc panel with name, occupation and tweet
        gameObject.SetActive(true);
        
        npcName.text = name;
        npcOccupation.text = occupation;
        npcTweet.text = tweet;
        npcAvatar.overrideSprite = avatar;
        
        // play conversation sounds
        //AudioManager.Instance.StartConversation();
    }
}