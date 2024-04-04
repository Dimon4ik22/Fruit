
using System.Collections.Generic;
using Cinemachine;
using HyperCasualPack;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UnlockerManager : Singleton<UnlockerManager>
{
    public List<Unlocker> Unlockers;
    public int unlockIndex = 0;
    public Arrow Arrow;
    public TMP_Text unlockDescriptionText;
    public TutorialCamera TutorialCamera;
    void Start()
    {
        unlockIndex = PlayerPrefs.GetInt("UnlockerIndex");
        if (unlockIndex == Unlockers.Count)
        {
            Arrow.gameObject.SetActive(false);
            unlockDescriptionText.gameObject.SetActive(false);
        }
        if (unlockIndex > 0)
        {
            UnlockTillUnlockIndex();
        }

        if (unlockIndex < Unlockers.Count)
        {
            SubscribeToAllUnlockers();
        }
        if (unlockIndex <= 3)
        {
            Arrow.target = Unlockers[unlockIndex].gameObject.transform;
            unlockDescriptionText.text = Unlockers[unlockIndex].UnlockerTutorialDescription;
        }

        if (unlockIndex > 3)
        {
            Arrow.gameObject.SetActive(false);
            unlockDescriptionText.gameObject.SetActive(false);
        }
 
    }

    void UnlockTillUnlockIndex()
    {
        for (int i = 0; i <= unlockIndex; i++)
        {
            if (i == unlockIndex)
            {
                Unlockers[i].gameObject.SetActive(true);
                return;
            }
            Unlockers[i].gameObject.SetActive(true);
            Unlockers[i]._onUnlocked.Invoke();
       
        }
        
    }
    
    [Button]
    public void SavePlayerPref()
    {
        PlayerPrefs.SetInt("UnlockerIndex",unlockIndex);
    }
    [Button]
    public void DebugKey()
    {
        Debug.Log(PlayerPrefs.GetInt("UnlockerIndex"));
        Debug.Log(PlayerPrefs.GetString("UnlockerIndex"));

    }
    public void SubscribeToAllUnlockers()
    {
        var tempList = FindObjectsOfType<Unlocker>(true);
        foreach (var item in tempList)
        {
            item._onUnlocked.AddListener(() =>
            {
                OnOneItemUnlocked();
            });
        }
    }

    void Update()
    {
        if (unlockIndex == 3)
        {
            unlockDescriptionText.gameObject.SetActive(false);
            Arrow.gameObject.SetActive(false);
        }
    }

    public void OnOneItemUnlocked()
    {
        if (unlockIndex < Unlockers.Count)
        {
            TutorialCamera.MoveTowardsTargetAndBack(Unlockers[unlockIndex].transform);
            unlockIndex++;
            PlayerPrefs.SetInt("UnlockerIndex",unlockIndex);
            Unlockers[unlockIndex].gameObject.SetActive(true);
            unlockDescriptionText.text = Unlockers[unlockIndex].UnlockerTutorialDescription;
            Arrow.target = Unlockers[unlockIndex].gameObject.transform;
            Run.After(1.5f, () =>
            {
                TutorialCamera.MoveTowardsTargetAndBack(Unlockers[unlockIndex].transform);

            });
            
            if (unlockIndex == 3)
            {
                unlockDescriptionText.gameObject.SetActive(false);
                Arrow.gameObject.SetActive(false);
            }
        }
        else if (unlockIndex == 3)
        {
            unlockDescriptionText.gameObject.SetActive(false);
            Arrow.gameObject.SetActive(false);
        }
    }
}
