using System.Collections.Generic;
using UnityEngine;

namespace Dishes {
    public class DishSpawnerManager : Singleton<DishSpawnerManager> 
    {
        public DishSpawner PlateSpawner;
        public DishSpawner GlassSpawner;
        public DishSpawner ForkSpawner;
        public DishSpawner KnifeSpawner;

        private Dictionary<Dish.DishType, DishSpawner> _dishSpawnerDictionary;

        protected override void Awake()
        {
            base.Awake();
            _dishSpawnerDictionary = new Dictionary<Dish.DishType, DishSpawner>() {
                { Dish.DishType.Plate, PlateSpawner },
                { Dish.DishType.Glass, GlassSpawner },
                { Dish.DishType.Fork, ForkSpawner },
                {Dish.DishType.Knife, KnifeSpawner },
            };
        }

        public bool RetrieveDish(Dish.DishType type)
        {
            if (!_dishSpawnerDictionary.ContainsKey(type)) return false;

            if (_dishSpawnerDictionary[type].Inventory > 0)
            {
                _dishSpawnerDictionary[type].RemoveDish();
                return true;
            }
            return false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    } }
