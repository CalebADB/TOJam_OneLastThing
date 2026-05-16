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
            //Debug.Log("Play Splash particle system");
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
            if (DishWasher.Instance.ManagingDish) return;
            if (Inventory.Count <= 0) return;
            if (SoapLevelInWater <= 0.1f) return;
            Debug.Log("Play Sound: Dish removed from water");
            // Replaceable Logic 
            int takenDish = Random.Range(0, Inventory.Count - 1);
            PreppedDishType = Inventory[takenDish];
            Inventory.RemoveAt(takenDish);
            ResolvePreppedDish();

            _nextCleaningChordIndex = 0;
        }

        public void AddSoap()
        {
            Debug.Log("Play Sound: Soap");
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
                    TestCleaningGeneric(_plateInputArray[_nextCleaningChordIndex], _plateInputArray.Length);
                    break;
                case Dish.DishType.Glass:
                    TestCleaningGeneric(_glassInputArray[_nextCleaningChordIndex], _glassInputArray.Length);
                    break;
                case Dish.DishType.Fork:
                    TestCleaningGeneric(_forkInputArray[_nextCleaningChordIndex], _forkInputArray.Length);
                    break;
                case Dish.DishType.Knife:
                    TestCleaningGeneric(_knifeInputArray[_nextCleaningChordIndex], _knifeInputArray.Length);
                    break;
            }

            DishWasher.Instance.ResolveObjective();
        }

        /// <summary>
        /// Gives you a type if valid, otherwise gives none
        /// </summary>
        public bool TryRemoveDish(out Dish.DishType removedDish)
        {
            removedDish = Dish.DishType.None;

            if (GetPreppedDish()?.DirtyPercentage <= MinAllowedDirtyPercent)
            {
                removedDish = PreppedDishType;
                ClearPreppedDish();
                return true;
            }

            return false;
        }

        public bool IsDishClean()
        {
            if (PreppedDishType == Dish.DishType.None) return false;
            return GetPreppedDish()?.DirtyPercentage <= MinAllowedDirtyPercent;
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

        private readonly KeyCode[] _plateInputArray = new KeyCode[] { KeyCode.W, KeyCode.E, KeyCode.D, KeyCode.X, KeyCode.Z, KeyCode.A };
        private readonly KeyCode[] _glassInputArray = new KeyCode[] { KeyCode.Q, KeyCode.A, KeyCode.Z, KeyCode.W, KeyCode.S, KeyCode.X, KeyCode.E, KeyCode.D, KeyCode.C };
        private readonly KeyCode[] _forkInputArray = new KeyCode[] { KeyCode.Z, KeyCode.A, KeyCode.Q, KeyCode.X, KeyCode.S, KeyCode.W };
        private readonly KeyCode[] _knifeInputArray = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T }; // , KeyCode.Y

        private void TestCleaningGeneric(KeyCode nextCleaningChordKey, int chordLength)
        {
            var keycodeClicked = GetClickedKeycode();
            // Debug.Log($"{keycodeClicked} vs {nextCleaningChordKey}");
            if (keycodeClicked == nextCleaningChordKey) // || keycodeClicked == index 0?
            {
                GetPreppedDish().CleanSlightly();

                _nextCleaningChordIndex++;
                if (_nextCleaningChordIndex >= chordLength)
                    _nextCleaningChordIndex = 0;
            }
        }
        private KeyCode GetClickedKeycode()
        {
            if (Keyboard.current.qKey.isPressed) return KeyCode.Q; // wasPressedThisFrame
            if (Keyboard.current.wKey.isPressed) return KeyCode.W;
            if (Keyboard.current.eKey.isPressed) return KeyCode.E;
            if (Keyboard.current.rKey.isPressed) return KeyCode.R;
            if (Keyboard.current.tKey.isPressed) return KeyCode.T;
            if (Keyboard.current.yKey.isPressed) return KeyCode.Y;

            if (Keyboard.current.aKey.isPressed) return KeyCode.A;
            if (Keyboard.current.sKey.isPressed) return KeyCode.S;
            if (Keyboard.current.dKey.isPressed) return KeyCode.D;
            if (Keyboard.current.fKey.isPressed) return KeyCode.F;

            if (Keyboard.current.zKey.isPressed) return KeyCode.Z;
            if (Keyboard.current.xKey.isPressed) return KeyCode.X;
            if (Keyboard.current.cKey.isPressed) return KeyCode.C;
            return KeyCode.None;
        }

        private void ResolvePreppedDish()
        {
            PreppedPlate.Hide();
            PreppedGlass.Hide();
            PreppedFork.Hide();
            PreppedKnife.Hide();

            if (PreppedDishType != Dish.DishType.None)
                GetPreppedDish()?.GetDirtySinkDish();

            DishWasher.Instance.ResolveObjective();
        }

        private void ResolveSoap()
        {
            SoapOverlay.alpha = SoapLevelInWater;
            DishWasher.Instance.ResolveObjective();
        }

        private Dish GetPreppedDish()
        {
            switch (PreppedDishType)
            {
                case Dish.DishType.Plate:
                    return PreppedPlate;
                case Dish.DishType.Glass:
                    return PreppedGlass;
                case Dish.DishType.Fork:
                    return PreppedFork;
                case Dish.DishType.Knife:
                    return PreppedKnife;
                default: return null;
            }
        }
    } 
 }
