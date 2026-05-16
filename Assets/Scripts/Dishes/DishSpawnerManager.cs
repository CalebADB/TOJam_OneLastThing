using System.Collections.Generic;
using UnityEngine;

namespace Dishes
{
    public class DishSpawnerManager : MonoBehaviour
    {
        public DishSpawner PlateSpawner;
        public DishSpawner GlassSpawner;
        public DishSpawner ForkSpawner;
        public DishSpawner KnifeSpawner;
        public WashingSink NextStage;

        public Canvas StationCanvas;

        private Dictionary<Dish.DishType, DishSpawner> _dishSpawnerDictionary;

        protected void Awake()
        {
            _dishSpawnerDictionary = new Dictionary<Dish.DishType, DishSpawner>() {
                { Dish.DishType.Plate, PlateSpawner },
                { Dish.DishType.Glass, GlassSpawner },
                { Dish.DishType.Fork, ForkSpawner },
                {Dish.DishType.Knife, KnifeSpawner },
            };
        }

        protected void Start()
        {
            AddLoad();
            AddLoad();
            AddLoad();
        }

        public void AddLoad()
        {
            _dishSpawnerDictionary[Dish.DishType.Plate].AddDishes(Random.Range(0, 4));
            _dishSpawnerDictionary[Dish.DishType.Glass].AddDishes(Random.Range(0, 4));
            _dishSpawnerDictionary[Dish.DishType.Fork].AddDishes(Random.Range(0, 4));
            _dishSpawnerDictionary[Dish.DishType.Knife].AddDishes(Random.Range(0, 4));
        }

        public void ClickedPlateSpawn()
        {
            ClickedSpawn(Dish.DishType.Plate);
        }
        public void ClickedGlassSpawn()
        {
            ClickedSpawn(Dish.DishType.Glass);
        }
        public void ClickedForkSpawn()
        {
            ClickedSpawn(Dish.DishType.Fork);
        }
        public void ClickedKnifeSpawn()
        {
            ClickedSpawn(Dish.DishType.Knife);
        }

        public void SetCanvas(bool showing)
        {
            StationCanvas.enabled = showing;
        }

        private void ClickedSpawn(Dish.DishType type)
        {
            if (DishWasher.Instance.ManagingDish) return;
            if (!_dishSpawnerDictionary.ContainsKey(type)) return;
            if (_dishSpawnerDictionary[type].Inventory <= 0) return;

            // Replaceable Logic
            if (!NextStage.AcceptingDishes) return;

            if (NextStage.AddDish(type))
            {
                _dishSpawnerDictionary[type].RemoveDish();

                if (_dishSpawnerDictionary[Dish.DishType.Plate].Inventory <= 0 && _dishSpawnerDictionary[Dish.DishType.Glass].Inventory <= 0 && _dishSpawnerDictionary[Dish.DishType.Fork].Inventory <= 0 && _dishSpawnerDictionary[Dish.DishType.Knife].Inventory <= 0)
                {
                    AddLoad();
                    AddLoad();
                }
            }
        }
    }
}
