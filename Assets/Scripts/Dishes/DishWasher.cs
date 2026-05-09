using UnityEngine;

namespace Dishes
{
    public class DishWasher : Singleton<DishWasher>
    {
        public Dish.DishType HeldDish;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            HeldDish = Dish.DishType.None;
        }

        #region -- UI --

        public void TryTakePlate()
        {
            if (HeldDish != Dish.DishType.None) return;

            throw new System.NotImplementedException();
            //if (DishSpawner)
        } 

        #endregion

        // Update is called once per frame
        void Update()
        {

        }
    }

}

public class Singleton<T> : MonoBehaviour
{
    public Singleton<T> Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        Instance = null;
    }
}