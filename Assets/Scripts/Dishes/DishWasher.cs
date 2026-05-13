using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Dishes
{
    public class DishWasher : Singleton<DishWasher>
    {
        public DishSpawnerManager DishSpawnMgr;
        public WashingSink DirtySinkMgr;
        public RinsingSink CleanSinkMgr;
        public UnityEngine.Canvas ObjectiveCanvas;
        public TMPro.TextMeshProUGUI ObjectiveText;
        public UnityEngine.UI.Image CleaningHintImage;
        public Sprite HintImage_Plate;
        public Sprite HintImage_Glass;
        public Sprite HintImage_Fork;
        public Sprite HintImage_Knife;
        public UnityEngine.Canvas ConvoCanvas;

        public bool ManagingDish => DirtySinkMgr.PreppingDish || CleanSinkMgr.PreppingDish;

        //public bool HoldingDish { get => HeldDish != Dish.DishType.None; }
        //public Dish.DishType HeldDish;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //HeldDish = Dish.DishType.None;
            ResolveObjective();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current.jKey.wasPressedThisFrame)
                DishSpawnMgr.AddLoad();
#endif
            if (DirtySinkMgr.PreppingDish)
                DoCleaningMinigame();
        }

        /// capture keys here, send to cleaning minigame validator

        private List<UnityEngine.InputSystem.Controls.KeyControl> sdfsd = new List<UnityEngine.InputSystem.Controls.KeyControl>();

        public void DoCleaningMinigame()
        {
            foreach (var key in Keyboard.current.allKeys)
            {
                if (key == null) continue;
                if (key.wasPressedThisFrame) sdfsd.Add(key);
                if (key.wasReleasedThisFrame && sdfsd.Contains(key)) sdfsd.Remove(key);
            }

            if (sdfsd.Count == 1)
            {
                DirtySinkMgr.TestCleaningAttempt();
            }

            //if (Keyboard.current.  //.anyKey.wasPressedThisFrame)
            //{
            //    Debug.Log(Keyboard.current.anyKey.shortDisplayName);
            //    int newlyPressedCount = 0;

            //    Debug.Log(newlyPressedCount);
            //    // only allow one new key press
            //    if (newlyPressedCount == 1)
            //    {
            //        DirtySinkMgr.TestCleaningAttempt();
            //    }               
            //}
        }

        public void SetDishCanvases(bool showing)
        {
            DishSpawnMgr.SetCanvas(showing);
            DirtySinkMgr.SetCanvas(showing);
            CleanSinkMgr.SetCanvas(showing);

            ObjectiveCanvas.enabled = showing;
            ConvoCanvas.enabled = showing;
        }

        public void ResolveObjective()
        {
            CleaningHintImage.gameObject.SetActive(false);
            if (CleanSinkMgr.NeedToDryDish)
            {
                ObjectiveText.text = "Move Dish To Rack";
            }
            else if (CleanSinkMgr.NeedToRinseDish)
            {
                ObjectiveText.text = "Run Faucet";
            }
            else if (CleanSinkMgr.AcceptingDishes && DirtySinkMgr.IsDishClean())
            {
                ObjectiveText.text = "Rinse Dish";
            }
            else if (DirtySinkMgr.PreppingDish && !DirtySinkMgr.IsDishClean())
            {
                ObjectiveText.text = string.Empty;
                CleaningHintImage.gameObject.SetActive(true);

                switch (DirtySinkMgr.PreppedDishType)
                {
                    case Dish.DishType.Plate:
                        CleaningHintImage.sprite = HintImage_Plate; break;
                    case Dish.DishType.Glass:
                        CleaningHintImage.sprite = HintImage_Glass; break;
                    case Dish.DishType.Fork:
                        CleaningHintImage.sprite = HintImage_Fork; break;
                    case Dish.DishType.Knife:
                        CleaningHintImage.sprite = HintImage_Knife; break;
                }
            }
            else if (!DirtySinkMgr.HasSoap)
            {
                ObjectiveText.text = "Add Soap";
            }
            else if (!DirtySinkMgr.PreppingDish && !DirtySinkMgr.NeedsDish)
            {
                ObjectiveText.text = "Grab Dish From Water";
            }
            else if (DirtySinkMgr.NeedsDish)
            {
                ObjectiveText.text = "Soak Dish";
            }
            else
            {
                ObjectiveText.text = "Wash Dishes!!";
            }
        }
    }
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null) Instance = this as T;
        else Destroy(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        Instance = null;
    }
}