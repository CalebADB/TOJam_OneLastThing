using UnityEngine;
using UnityEngine.InputSystem;

namespace Dishes
{
    public class DishWasher : Singleton<DishWasher>
    {
        public DishSpawnerManager DishSpawnMgr;
        public WashingSink DirtySinkMgr;
        public RinsingSink CleanSinkMgr;
        public TMPro.TextMeshProUGUI ObjectiveText;

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
           if(Keyboard.current.jKey.wasPressedThisFrame)
                DishSpawnMgr.AddLoad();
            if (DirtySinkMgr.PreppingDish)
                DoCleaningMinigame();
        }

        /// capture keys here, send to cleaning minigame validator
        public void DoCleaningMinigame()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Debug.Log("placeholder cleaning!!");
                DirtySinkMgr.TestCleaningAttempt();
            }
        }

        public void ResolveObjective()
        {
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
                ObjectiveText.text = "Wash Dish (C)";
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