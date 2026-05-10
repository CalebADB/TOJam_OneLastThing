using System.Collections.Generic;
using UnityEngine;

namespace Dishes
{
    public class RinsingSink : MonoBehaviour
    {
        public bool AcceptingDishes => !PreppingDish;

        public Dish.DishType PreppedDish;
        public bool PreppingDish => PreppedDish != Dish.DishType.None;
        public Dish PreppedPlate;
        public Dish PreppedGlass;
        public Dish PreppedFork;
        public Dish PreppedKnife;

        private bool _rinsedDish;

        protected void Start()
        {
            PreppedDish = Dish.DishType.None;
            ResolvePreppedDish();
        }

        public void TryMoveDish()
        {
            if (DishWasher.Instance.DirtySinkMgr.TryRemoveDish(out Dish.DishType dish))
            {
                PreppedDish = dish;
                ResolvePreppedDish();
            }
        }

        public void RunTap()
        {
            if (PreppingDish) _rinsedDish = true;
        }

        public void MoveDishToRack()
        {
            if (PreppingDish && _rinsedDish)
            {
                PreppedDish = Dish.DishType.None;
                ResolvePreppedDish();
                Debug.Log("play glass clink");
            }
        }

        public bool MoveDishHere(Dish.DishType type)
        {
            if (!AcceptingDishes) return false;

            _rinsedDish = false;
            PreppedDish = type;
            ResolvePreppedDish();
            return true;
        }      

        private void ResolvePreppedDish()
        {
            PreppedPlate.Hide();
            PreppedGlass.Hide();
            PreppedFork.Hide();
            PreppedKnife.Hide();

            switch (PreppedDish)
            {
                case Dish.DishType.Plate:
                    PreppedPlate.DirtySinkDish();
                    break;
                case Dish.DishType.Glass:
                    PreppedGlass.DirtySinkDish();
                    break;
                case Dish.DishType.Fork:
                    PreppedFork.DirtySinkDish();
                    break;
                case Dish.DishType.Knife:
                    PreppedKnife.DirtySinkDish();
                    break;
            }
        }
    }
}