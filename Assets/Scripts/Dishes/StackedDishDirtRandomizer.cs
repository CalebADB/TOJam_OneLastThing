using UnityEngine;

namespace Dishes
{
    public class StackedDishDirtRandomizer : MonoBehaviour
    {
        private Dish[] _dishes;

        private void Awake()
        {
            _dishes = GetComponentsInChildren<Dish>();
        }

        void Start()
        {

        }

        public void Randomize()
        {
            foreach (var dish in _dishes)
            {
                dish.RandomizeDirt(Dish.DirtyRandomizationType.OnCounter);
            }
        }
    }
}
