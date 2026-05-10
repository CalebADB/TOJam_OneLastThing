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
            if (Keyboard.current.jKey.wasPressedThisFrame)
                DishSpawnMgr.AddLoad();
            if (DirtySinkMgr.PreppingDish)
                DoCleaningMinigame();
        }

        /// capture keys here, send to cleaning minigame validator
        public void DoCleaningMinigame()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                int pressedCount = 0;
                foreach (var key in Keyboard.current.allKeys)
                {
                    if (key == null) continue;
                    if (key.isPressed) pressedCount++;
                }

                if (pressedCount == 1)
                {
                    Debug.Log("placeholder cleaning!!");
                    DirtySinkMgr.TestCleaningAttempt();
                }               
            }
        }

        public void SetDishCanvases(bool showing)
        {
            DishSpawnMgr.SetCanvas(showing);
            DirtySinkMgr.SetCanvas(showing);
            CleanSinkMgr.SetCanvas(showing);

            ObjectiveCanvas.enabled = showing;
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
                ObjectiveText.text = "Rinse Dish";
            }
            else if (CleanSinkMgr.AcceptingDishes && DirtySinkMgr.IsDishClean())
            {
                ObjectiveText.text = "Prep Dish To Rinse";
            }
            else if (DirtySinkMgr.PreppingDish && !DirtySinkMgr.IsDishClean())
            {
                ObjectiveText.text = "Wash Dish";
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

        //public void PickupDish(Dish.DishType dish)
        //{
        //    if (HoldingDish) return; // ideally auxillary check, shouldn't need it

        //    HeldDish = dish;
        //    ResolveHeldDish();
        //    Debug.Log("Picked up dish!");
        //}

        //private void ResolveHeldDish()
        //{

        //}


        //private void TryTakeDish(Dish.DishType type)
        //{
        //    if (HeldDish != Dish.DishType.None) return;
        //    if (DishSpawnerManager.Instance.TryTakeDish(type))
        //    {
        //        HeldDish = type;
        //    }
        //}

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