using UnityEngine;

namespace Dishes
{
    public class Dish : MonoBehaviour
    {
        public enum DishType
        {
            None,
            Plate,
            Glass,
            Fork,
            Knife
        }
        public DishType Type;
    }
}