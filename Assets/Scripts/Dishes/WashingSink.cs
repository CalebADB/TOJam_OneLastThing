using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dishes {
    public class WashingSink : MonoBehaviour
    {
        public bool AcceptingDishes => HasSpace && HasSoap;
        public bool HasSpace => Inventory.Count < MaxDishes;
        public bool HasSoap => SoapLevelInWater > 0.1f;
        public bool NeedsDish => Inventory.Count <= 0;

        [Header("Config")]
        public int MaxDishes = 10;
        [Range(0, 1f)]
        public float StartingSoapValue = 0.25f;
        [Range(0, 0.5f)]
        public float MinAllowedDirtyPercent = 0.05f;

        [Header("Vars")]
        public List<Dish.DishType> Inventory;
        public AudioClip AddDishToWaterClip;

        public Dish.DishType PreppedDishType;
        public bool PreppingDish => PreppedDishType != Dish.DishType.None;
        public Dish PreppedPlate;
        public Dish PreppedGlass;
        public Dish PreppedFork;
        public Dish PreppedKnife;
        public AudioClip PickupPlateClip;

        public CanvasGroup SoapOverlay;
        public float SoapLevelInWater;

        public ParticleSystem SoapBubbles;
        public Canvas StationCanvas;

        private int _nextCleaningChordIndex;

        protected void Start()
        {
            Inventory = new List<Dish.DishType>();
            PreppedDishType = Dish.DishType.None;
            ResolvePreppedDish();
            SoapLevelInWater = StartingSoapValue;
            ResolveSoap();
        }

        public bool AddDish(Dish.DishType dish)
        {
            if (!HasSpace) return false;
            if (!HasSoap) return false;

            Inventory.Add(dish);
            Debug.Log("Play Splash particle system");
            Debug.Log("Play Sound: Dish added to sink");
            SoapBubbles.Play();

            // Soap
            SoapLevelInWater -= Random.Range(0.01f, 0.2f);
            SoapLevelInWater = Mathf.Max(0, SoapLevelInWater);
            ResolveSoap();
            return true;
        }

        public void PrepDishToClean()
        {
            Debug.Log("dklfjsdl");
            if (DishWasher.Instance.ManagingDish) return;
            if (Inventory.Count <= 0) return;
            if (SoapLevelInWater <= 0.1f) return;
            Debug.Log("iuiu");
            // Replaceable Logic 
            int takenDish = Random.Range(0, Inventory.Count - 1);
            PreppedDishType = Inventory[takenDish];
            Inventory.RemoveAt(takenDish);
            ResolvePreppedDish();

            _nextCleaningChordIndex = 0;
        }

        public void AddSoap()
        {
            SoapLevelInWater += Random.Range(0.3f, 0.8f);
            SoapLevelInWater = Mathf.Min(SoapLevelInWater, 1);
            ResolveSoap();
        }

        public void TestCleaningAttempt()
        {
            // minigame validator should track chord index and reference input chords
            // then if it matches, tell current dish to becomeslighly cleaner

            switch (PreppedDishType)
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

            DishWasher.Instance.ResolveObjective();
        }

        public bool IsDishClean()
        {
            switch (PreppedDishType)
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

            switch (PreppedDishType)
            {
                case Dish.DishType.Plate:
                    if (PreppedPlate.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDishType;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
                case Dish.DishType.Glass:
                    if (PreppedGlass.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDishType;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
                case Dish.DishType.Fork:
                    if (PreppedFork.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDishType;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
                case Dish.DishType.Knife:
                    if (PreppedKnife.DirtyPercentage <= MinAllowedDirtyPercent)
                    {
                        removedDish = PreppedDishType;
                        ClearPreppedDish();
                        return true;
                    }
                    break;
            }
            return false;
        }

        public void SetCanvas(bool showing)
        {
            StationCanvas.enabled = showing;
        }

        private void ClearPreppedDish()
        {
            PreppedDishType = Dish.DishType.None;
            ResolvePreppedDish();
        }

        private readonly KeyCode[] _plateInputArray = new KeyCode[]{KeyCode.W, KeyCode.E, KeyCode.D, KeyCode.X, KeyCode.Z, KeyCode.A};
        private void TestCleaningPlate()
        {
            TestCleaningGeneric(_plateInputArray[_nextCleaningChordIndex], _plateInputArray.Length);
        }
        private readonly KeyCode[] _glassInputArray = new KeyCode[] { KeyCode.Q, KeyCode.A, KeyCode.Z, KeyCode.W, KeyCode.S, KeyCode.X, KeyCode.E, KeyCode.D, KeyCode.C };
        private void TestCleaningGlass()
        {
            TestCleaningGeneric(_glassInputArray[_nextCleaningChordIndex], _glassInputArray.Length);
        }
        private readonly KeyCode[] _forkInputArray = new KeyCode[] { KeyCode.Z, KeyCode.A, KeyCode.Q, KeyCode.X, KeyCode.S, KeyCode.W };
        private void TestCleaningFork()
        {
            TestCleaningGeneric(_forkInputArray[_nextCleaningChordIndex], _forkInputArray.Length);
        }
        private readonly KeyCode[] _knifeInputArray = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };
        private void TestCleaningKnife()
        {
            TestCleaningGeneric(_knifeInputArray[_nextCleaningChordIndex], _knifeInputArray.Length);
        }
        private void TestCleaningGeneric(KeyCode nextCleaningChordKey, int chordLength)
        {
            var keycodeClicked = GetClickedKeycode();
            Debug.Log($"{keycodeClicked} vs {nextCleaningChordKey}");
            if (keycodeClicked == nextCleaningChordKey)
            {
                Debug.Log("Cleaned Slightly");
                PreppedKnife.CleanSlightly();

                _nextCleaningChordIndex++;
                if (_nextCleaningChordIndex >= chordLength)
                    _nextCleaningChordIndex = 0;
            }
        }
        private KeyCode GetClickedKeycode()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame) return KeyCode.Q;
            if (Keyboard.current.wKey.wasPressedThisFrame) return KeyCode.W;
            if (Keyboard.current.eKey.wasPressedThisFrame) return KeyCode.E;
            if (Keyboard.current.rKey.wasPressedThisFrame) return KeyCode.R;
            if (Keyboard.current.tKey.wasPressedThisFrame) return KeyCode.T;
            if (Keyboard.current.yKey.wasPressedThisFrame) return KeyCode.Y;

            if (Keyboard.current.aKey.wasPressedThisFrame) return KeyCode.A;
            if (Keyboard.current.sKey.wasPressedThisFrame) return KeyCode.S;
            if (Keyboard.current.dKey.wasPressedThisFrame) return KeyCode.D;
            if (Keyboard.current.fKey.wasPressedThisFrame) return KeyCode.F;

            if (Keyboard.current.zKey.wasPressedThisFrame) return KeyCode.Z;
            if (Keyboard.current.xKey.wasPressedThisFrame) return KeyCode.X;
            if (Keyboard.current.cKey.wasPressedThisFrame) return KeyCode.C;
            return KeyCode.None;
        }

        private void ResolvePreppedDish()
        {
            PreppedPlate.Hide();
            PreppedGlass.Hide();
            PreppedFork.Hide();
            PreppedKnife.Hide();

            switch (PreppedDishType) {
                case Dish.DishType.Plate:
                    PreppedPlate.GetDirtySinkDish();
                    break;
                case Dish.DishType.Glass:
                    PreppedGlass.GetDirtySinkDish();
                    break;
                case Dish.DishType.Fork:
                    PreppedFork.GetDirtySinkDish();
                    break;
                case Dish.DishType.Knife:
                    PreppedKnife.GetDirtySinkDish();
                    break;
            }

            DishWasher.Instance.ResolveObjective();
        }

        private void ResolveSoap()
        {
            SoapOverlay.alpha = SoapLevelInWater;
        }
    } 
 }
