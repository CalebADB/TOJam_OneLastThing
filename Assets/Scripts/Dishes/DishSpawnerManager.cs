using System.Collections.Generic;
using UnityEngine;

namespace Dishes {
    public class DishSpawnerManager : MonoBehaviour
    {
        public DishSpawner PlateSpawner;
        public DishSpawner GlassSpawner;
        public DishSpawner ForkSpawner;
        public DishSpawner KnifeSpawner;
        public WashingSink NextStage;

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

        public bool TryTakeDish(Dish.DishType type)
        {
            if (!_dishSpawnerDictionary.ContainsKey(type)) return false;

            if (_dishSpawnerDictionary[type].Inventory > 0)
            {
                _dishSpawnerDictionary[type].RemoveDish();
                return true;
            }
            return false;
        }

        public void AddLoad()
        {
            Debug.Log("sdlkfjsdlk");
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

        private void ClickedSpawn(Dish.DishType type)
        {
            if (DishWasher.Instance.ManagingDish) return;
            if (!_dishSpawnerDictionary.ContainsKey(type)) return;
            if (_dishSpawnerDictionary[type].Inventory <= 0) return;

            // Replaceable Logic
            if (!NextStage.AcceptingDishes) return;

            if (NextStage.AddDish(type))
                _dishSpawnerDictionary[type].RemoveDish();
        }
    } 
}
