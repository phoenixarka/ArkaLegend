using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LootWindow : MonoBehaviour {
    private static LootWindow instance;

    public static LootWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LootWindow>();
            }
            return instance;
        }
    }

    private int lootSlotNum = 4;

    [SerializeField]
    private LootButton[] lootButtons;

    [SerializeField]
    private Text pageNumText;

    [SerializeField]
    private GameObject prevBtn, nextBtn,closeBtn;

    private CanvasGroup canvasGroup;

    private List<Item> lootList;

    private int totalPageNum;

    private int currPageIdx;

    // For debug
    [SerializeField]
    private List<Item> items;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        CloseLootWindow();
    }

    //initialize loot window
    public void CreateLootList(List<Item> allLoots) {
        if (allLoots.Count > 0) {
            ClearButtons();
            lootList = allLoots;

            calculatePage();
            currPageIdx = 0;
            AddLoot();
            OpenLootWindow();
        } else
        {
            Debug.Log("Warning Text:No lootable items");

        }
    }

    private void calculatePage() {
        totalPageNum = (lootList.Count / 4) + (lootList.Count % 4 > 0 ? 1 : 0);
    }

    private void AddLoot() {
        int queueSize = lootList.Count;
        if (queueSize > 0) {
            if (totalPageNum > 1)
            {
                pageNumText.text = (currPageIdx + 1) + "/" + totalPageNum;
            }
            else {
                pageNumText.text = "";
            }
            prevBtn.SetActive(currPageIdx > 0);
            nextBtn.SetActive(totalPageNum > 1 && currPageIdx < totalPageNum-1);

            int startIdx = lootSlotNum * currPageIdx;

            for (int i = 0; i < 4; i++) {
                // add loot if there is an item belongs to
                if (i < queueSize - startIdx)
                {
                    //Debug.Log(i + "+" + startIdx);
                    Item currItem = lootList[i + startIdx];
                    lootButtons[i].ThisLoot = currItem;
                    lootButtons[i].Icon.sprite = currItem.Icon;

                    string title = string.Format("<color={0}>{1}</color>",
                                             QualityColor.Quality2colorMap[currItem.ThisQuality], currItem.Title);
                    lootButtons[i].Title.text = title;
                    lootButtons[i].gameObject.SetActive(true);
                }
                // clear previous/next pages item
                else {
                    lootButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void NextPage()
    {
        currPageIdx = currPageIdx + 1;
        AddLoot();
    }

    public void PrevPage() {
        currPageIdx = currPageIdx - 1;
        AddLoot();
    }

    public void TakeLoot(int btnIdx) {
        lootList.RemoveAt(btnIdx + currPageIdx * 4);

        calculatePage();
        if (currPageIdx >= totalPageNum - 1) {
            currPageIdx = totalPageNum - 1;
        }
        AddLoot();
    }

    public void OpenLootWindow() {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void CloseLootWindow()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ClearButtons();
    }

    private void ClearButtons() {
        foreach (LootButton lootbtn in lootButtons) {
            lootbtn.gameObject.SetActive(false);
        }
    }
}
