using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dishes {
    public class WashingSink : MonoBehaviour
    {
        public bool AcceptingDishes => HasSpace && HasSoap;
        public bool HasSpace => Inventory.Count < 10;
        public bool HasSoap => SoapLevelInWater > 0.1f;

        [Header("Config")]
        public int MaxDishes = 10;
        [Range(0, 0.5f)]
        public float MinAllowedDirtyPercent = 0.05f;

        [Header("Vars")]
        public List<Dish.DishType> Inventory;

        public Dish.DishType PreppedDish;
        public bool PreppingDish => PreppedDish != Dish.DishType.None;
        public Dish PreppedPlate;
        public Dish PreppedGlass;
        public Dish PreppedFork;
        public Dish PreppedKnife;

        public CanvasGroup SoapOverlay;
        public float SoapLevelInWater;

        protected void Start()
        {
            Inventory = new List<Dish.DishType>();
            PreppedDish = Dish.DishType.None;
            ResolvePreppedDish();
            SoapLevelInWater = 0;
            ResolveSoap();
        }

        public bool AddDish(Dish.DishType dish)
        {
            if (!HasSpace) return false;
            if (!HasSoap) return false;

            Inventory.Add(dish);
            Debug.Log("Play Splash particle system");
            Debug.Log("Play Sound: Dish added to sink");

            // Soap
            SoapLevelInWater -= Random.Range(0.05f, 0.15f);
            SoapLevelInWater = Mathf.Max(0, SoapLevelInWater);
            ResolveSoap();
            return true;
        }

        public void PrepDishToClean()
        {
            if (DishWasher.Instance.ManagingDish) return;
            if (Inventory.Count <= 0) return;
            if (SoapLevelInWater <= 0.1f) return;

            // Replaceable Logic 
            int takenDish = Random.Range(0, Inventory.Count - 1);
            PreppedDish = Inventory[takenDish];
            Inventory.RemoveAt(takenDish);
            ResolvePreppedDish();
        }

        public void AddSoap()
        {
            SoapLevelInWater += Random.Range(0, 0.5f);
            SoapLevelInWater = Mathf.Min(SoapLevelInWater, 1);
            ResolveSoap();
        }

        public void TestCleaningAttempt()
        {
            // minigame validator should track chord index and reference input chords
            // then if it matches, tell current dish to becomeslighly cleaner

            switch (PreppedDish)
            {
                case Dish.DishType.Plate:
                    TestCleaningPlate();
                    break;
                case Dish.DishType.Glass:
                    TestCleaningGlass();
                    break;
                case Dish.DishType.Fork:
                    TestCleaningFork();
                    break;
                case Dish.DishType.Knife:
                    TestCleaningKnife();
                    break;
            }
        }

        public bool IsDishClean()
        {
            switch (PreppedDish)
            {
                case Dish.DishType.Plate:
                    return PreppedPlate.DirtyPercentage <= MinAllowedDirtyPercent;
                case Dish.DishType.Glass:
                    return PreppedGlass.DirtyPercentage <= MinAllowedDirtyPercent;
                case Dish.DishType.Fork:
                    return PreppedFork.DirtyPercentage <= MinAllowedDirtyPercent;
                case Dish.DishType.Knife:
                    return PreppedKnife.DirtyPercentage <= MinAllowedDirtyPercent;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gives you a type if valid, otherwise gives none
        /// </summary>
        public bool TryRemoveDish(out Dish.DishType removedDish)
        {
            removedDish = Dish.DishType.None;

            switch (PreppedDish)
            {
                case Dish.DishType.Plate:
                    if (PreppedPlate.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDish;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
                case Dish.DishType.Glass:
                    if (PreppedGlass.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDish;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
                case Dish.DishType.Fork:
                    if (PreppedFork.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDish;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
                case Dish.DishType.Knife:
                    if (PreppedKnife.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDish;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void ClearPreppedDish()
        {
            PreppedDish = Dish.DishType.None;
            ResolvePreppedDish();
        }

        private void TestCleaningPlate()
        {
            // Simplified for now
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                Debug.Log("how did you know!!!!");
                PreppedPlate.CleanSlightly();
            }
        }
        private void TestCleaningGlass()
        {
            // Simplified for now
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                Debug.Log("how did you know!!!!");
                PreppedGlass.CleanSlightly();
            }
        }
        private void TestCleaningFork()
        {
            // Simplified for now
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                Debug.Log("how did you know!!!!");
                PreppedFork.CleanSlightly();
            }
        }
        private void TestCleaningKnife()
        {
            // Simplified for now
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                Debug.Log("how did you know!!!!");
                PreppedKnife.CleanSlightly();
            }
        }

        private void ResolvePreppedDish()
        {
            PreppedPlate.Hide();
            PreppedGlass.Hide();
            PreppedFork.Hide();
            PreppedKnife.Hide();

            switch (PreppedDish) {
                case Dish.DishType.Plate:
                    PreppedPlate.ShowAndDirty();
                    break;
                case Dish.DishType.Glass:
                    PreppedGlass.ShowAndDirty();
                    break;
                case Dish.DishType.Fork:
                    PreppedFork.ShowAndDirty();
                    break;
                case Dish.DishType.Knife:
                    PreppedKnife.ShowAndDirty();
                    break;
            }
        }

        private void ResolveSoap()
        {
            SoapOverlay.alpha = SoapLevelInWater;
        }
    } 
 }
