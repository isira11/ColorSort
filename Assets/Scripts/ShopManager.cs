using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EasyMobile;
using EasyButtons;
using Doozy.Engine;

public struct ThemeId
{
    public ThemeItem theme_item;
    public int id;
    public string base_id;

    public ThemeId(ThemeItem theme_item, int id, string base_id)
    {
        this.theme_item = theme_item;
        this.id = id;
        this.base_id = base_id;
    }
}

[System.Serializable]
public class Offers 
{
    public Image unlock_item_image;
    public Button unlock_btn;

    public TextMeshProUGUI coins_txt;
    public Button double_btn;

    public Button remove_ads_button;
    public Button skip_btn;

    public void ShowOffers(ShopManager shopManager)
    {

        unlock_btn.onClick.RemoveAllListeners();
        double_btn.onClick.RemoveAllListeners();
        skip_btn.onClick.RemoveAllListeners();
        remove_ads_button.onClick.RemoveAllListeners();

        List<ThemeId> list = shopManager.GetListOfLockedItems();

        if (list.Count > 0)
        {
            ThemeId random = list[Random.Range(0, list.Count)];

            string id = random.base_id + ":" + random.id;
            unlock_item_image.sprite = random.theme_item.icon;

            unlock_btn.onClick.AddListener(() =>
            {
                AdsManager.instance.ShowRewarded(()=>
                {
                    shopManager.UnlockItem(id);
                    PlayerPrefs.SetInt(random.base_id, random.id);
                    GameEventMessage.SendEvent("next");

                    switch (random.base_id)
                    {
                        case "BACKGROUND":
                            shopManager.linker.selected_background = random.theme_item;
                            shopManager.linker.BackgroundThemeChanged.Invoke(random.theme_item);
                            break;
                        case "STONE":
                            shopManager.linker.selected_stones = random.theme_item;
                            shopManager.linker.StoneThemeChanged.Invoke(random.theme_item);
                            break;
                        case "TUBE":
                            shopManager.linker.selected_tube = random.theme_item;
                            shopManager.linker.TubeThemeChanged.Invoke(random.theme_item);
                            break;
                    }

                },
                () =>
                {
                    GameEventMessage.SendEvent("next");
                }
                );
            });

        }

        int coins = PlayerPrefs.GetInt("LEVEL", 1) * 10;

        coins_txt.SetText(""+ coins);

        double_btn.onClick.AddListener(() =>
        {
            AdsManager.instance.ShowRewarded(() =>
            {
                shopManager.linker.Deposit(coins*2);
                GameEventMessage.SendEvent("next");
            },
            () =>
            {
                GameEventMessage.SendEvent("next");
            }
            );
        });

        skip_btn.onClick.AddListener(() =>
        {
            shopManager.linker.Deposit(coins);
            AdsManager.instance.ShowInterstitial();
        });

        remove_ads_button.onClick.AddListener(() => {
            shopManager.BuyNoAds();
        });

    }

}

public class ShopManager : MonoBehaviour
{
    public Offers offers;

    public Image background;

    public TextMeshProUGUI cash_txt;

    public Themes themes;

    public Transform container;

    public GameObject item_prefab_1;

    public ScriptLinkerSO linker;

    public Dictionary<string, ItemUI> selected_items_ui = new Dictionary<string, ItemUI>();

    public Dictionary<string, List<ThemeItem>> all_items = new Dictionary<string, List<ThemeItem>>();

    List<ThemeItem> ref_stones = new List<ThemeItem>();
    List<ThemeItem> ref_background = new List<ThemeItem>();
    List<ThemeItem> ref_tube = new List<ThemeItem>();

    List<ThemeItem> themeItems;

    public const string STONE_KEY = "STONE";
    public const string BACKGROUND_KEY = "BACKGROUND";
    public const string TUBE_KEY = "TUBE";

    GameObject selected_obj;

    public void Init()
    {


        all_items.Add(STONE_KEY, ref_stones);
        all_items.Add(BACKGROUND_KEY, ref_background);
        all_items.Add(TUBE_KEY, ref_tube);

        foreach (ThemeItem item in themes.stone_themes)
        {
            ref_stones.Add(item);
        }

        foreach (ThemeItem item in themes.backgrounds)
        {
            ref_background.Add(item);
        }

        foreach (ThemeItem item in themes.tubes)
        {
            ref_tube.Add(item);
        }

        PlayerPrefs.SetInt(STONE_KEY + ":" + 0, 1);
        PlayerPrefs.SetInt(BACKGROUND_KEY + ":" + 0, 1);
        PlayerPrefs.SetInt(TUBE_KEY + ":" + 0, 1);

        print(PlayerPrefs.GetInt(STONE_KEY));

        linker.BackgroundThemeChanged += (ThemeItem theme_item) =>
        {
            background.sprite = theme_item.images[0];
        };
        linker.selected_stones = ref_stones[PlayerPrefs.GetInt(STONE_KEY)];
        linker.selected_background = ref_background[PlayerPrefs.GetInt(BACKGROUND_KEY)];
        linker.selected_tube = ref_tube[PlayerPrefs.GetInt(TUBE_KEY)];

        linker.LevelCompleted += () =>
        {
            offers.ShowOffers(this);
        };

        Load(STONE_KEY);

    }

    public void HighlightSelected(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().color = Color.red;

        if (selected_obj)
        {
            if (selected_obj != gameObject)
            {
                selected_obj.GetComponent<Image>().color = Color.black;
            }
        }
        
        selected_obj = gameObject;
    }

    public void Load(string base_id)
    {
        themeItems = new List<ThemeItem>();
        System.Action<ThemeItem,GameObject> OnClick = delegate { };
        ThemeItem selected = null;
        switch (base_id)
        {
            case STONE_KEY:
                themeItems = ref_stones;
                OnClick += (ThemeItem themeList,GameObject item_obj) =>
                {
                    linker.selected_stones = themeList;
                    linker.StoneThemeChanged.Invoke(themeList);
                    HighlightSelected(item_obj);
                };
                selected = ref_stones[PlayerPrefs.GetInt(STONE_KEY)];
                break;
            case BACKGROUND_KEY:
                themeItems = ref_background;
                OnClick += (ThemeItem themeList, GameObject item_obj) =>
                {
                    linker.selected_background = themeList;
                    linker.BackgroundThemeChanged.Invoke(themeList);
                    HighlightSelected(item_obj);
                };
                selected = ref_background[PlayerPrefs.GetInt(BACKGROUND_KEY)];
                break;
            case TUBE_KEY:
                themeItems = ref_tube;
                OnClick += (ThemeItem themeList, GameObject item_obj) =>
                {
                    linker.selected_tube = themeList;
                    linker.TubeThemeChanged.Invoke(themeList);
                    HighlightSelected(item_obj);
                };
                selected = ref_tube[PlayerPrefs.GetInt(TUBE_KEY)];
                break;
            default:
                break;
        }

        ClearContainer();
        int index = 0;

        foreach (ThemeItem themeItem in themeItems)
        {
            int temp = index;
            string id = base_id + ":" + index;
            GameObject item_obj = Instantiate(item_prefab_1, container);
            ItemUI ui = item_obj.GetComponentInChildren<ItemUI>();
            ui.icon.sprite = themeItem.icon;
            ui.id = id;

            if (IsItemUnLocked(id))
            {

                ui.locked.SetActive(false);
            }
            else
            {
                ui.locked.SetActive(true);
            }

            ui.button.onClick.AddListener(() =>
            {

                if (IsItemUnLocked(id))
                {
                    OnClick.Invoke(themeItem,item_obj);
                    PlayerPrefs.SetInt(base_id, temp);
                }
            }
            );

            if (selected == themeItem)
            {
                HighlightSelected(item_obj);
            }
            else
            {
                item_obj.GetComponent<Image>().color = Color.black;
            }

            selected_items_ui.Add(id, ui);
            index++;
        }
    }

    public void ClearContainer()
    {
        foreach (Transform item in container)
        {
            Destroy(item.gameObject);
        }

        selected_items_ui.Clear();
    }

    public bool IsItemUnLocked(string id)
    {
        return PlayerPrefs.GetInt(id, 0) == 0 ? false : true;
    }

    public void UnlockItem(string id)
    {
        PlayerPrefs.SetInt(id, 1);
    }

    public void UnlockRandom()
    {
        List<string> keys = new List<string>();
        foreach (var item in selected_items_ui)
        {
            if (!IsItemUnLocked(item.Key))
            {
                keys.Add(item.Key);
            }
        }

        if (keys.Count > 0)
        {
            string key = keys[Random.Range(0, keys.Count)];

            if (selected_items_ui.TryGetValue(key, out ItemUI value))
            {
                linker.Transact.Invoke(100, (bool success) => {
                    if (success)
                    {
                        UnlockItem(key);
                        value.Unlock();
                    }
                });
 
            }
        }

    }

    public void AddCash()
    {
        AdsManager.instance.ShowRewarded(()=> { linker.Deposit(100); });
    }

    public void BuyNoAds()
    {
        InAppPurchasing.InitializePurchasing();
        if (InAppPurchasing.IsProductOwned(EM_IAPConstants.Product_Remove_Ads))
        {
            print("product owned");
        }
        else
        {
            InAppPurchasing.Purchase(EM_IAPConstants.Product_Remove_Ads);
        }

    }

    public List<ThemeId> GetListOfLockedItems()
    {
        List<ThemeId> list = new List<ThemeId>();

        foreach (var key_value in all_items)
        {
            int i = 0;
            foreach (ThemeItem item in key_value.Value)
            {
                string id = key_value.Key + ":" + i;
                if (!IsItemUnLocked(id))
                {
                    ThemeId themeId = new ThemeId(item, i, key_value.Key);
                    list.Add(themeId);
                }
                i++;
            }
        }

        return list;
    }

    private void OnPurchaseCompleted(IAPProduct obj)
    {
        if (obj.Id == EM_IAPConstants.Product_Remove_Ads)
        {
            print("remove ads");
        }
    }

    private void OnEnable()
    {


        InAppPurchasing.PurchaseCompleted += OnPurchaseCompleted;

        linker.playing = false;
    }

    private void OnDisable()
    {
        linker.playing = true;
        InAppPurchasing.PurchaseCompleted -= OnPurchaseCompleted;
    }


}
