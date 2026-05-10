using UnityEngine;

namespace Dishes
{
    public class DishSpawner : MonoBehaviour
    {
        public Dish.DishType SpawnerType;
        public GameObject[] SpawnPoints;

        [Range(0, 8)]
        public int Inventory;

        void Start()
        {
            Inventory = 0;
            ResolveInventory();
        }

        private void Reset()
        {
            SpawnPoints = new GameObject[transform.childCount];

            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                SpawnPoints[i] = transform.GetChild(i).gameObject;
            }
        }

        public void AddDishes (int num)
        {
            Inventory += num;
            Inventory = Mathf.Clamp(Inventory, 0, SpawnPoints.Length);
            ResolveInventory();
        }

        public void RemoveDish()
        {
            if (Inventory > 0)
            {
                Inventory--;
                ResolveInventory();
            }
        }

        private void ResolveInventory()
        {
            for (int i = 0; i < SpawnPoints.Length; i++) {
                SpawnPoints[i].SetActive(i < Inventory); // 0 index objects with 1 indexed Inventory int
            }
            Debug.Log($"Spawner has an inventory of {Inventory}");
        }
    }
}
