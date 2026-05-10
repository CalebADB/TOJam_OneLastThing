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

        public enum DirtyRandomizationType
        {
            OnCounter,
            FromSoapWater
        }

        public bool IsCutlery => Type == DishType.Fork || Type == DishType.Knife;

        public DishType Type;
        public float DirtyPercentage;
        public GameObject[] DirtSmears;

        private void OnEnable()
        {
            ResolveDirt();
        }

        public void ShowAndDirty()
        {

        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void CleanSlightly()
        {
            DirtyPercentage -= IsCutlery ? Random.Range(0, 0.25f) : Random.Range(0, 0.1f);
            DirtyPercentage = Mathf.Max(0, DirtyPercentage);
            ResolveDirt();
        }

        public void DirtySinkDish()
        {
            gameObject.SetActive(true);
            RandomizeDirt(DirtyRandomizationType.FromSoapWater);
        }

        public void RandomizeDirt(DirtyRandomizationType randomization)
        {
            DirtyPercentage = randomization == DirtyRandomizationType.FromSoapWater ? Random.Range(0.2f, 0.7f) : Random.Range(0.5f, 1f);
            ResolveDirt();
        }

        private void ResolveDirt()
        {
            for (int i = 0; i < DirtSmears.Length; i++)
            {
                DirtSmears[i].gameObject.SetActive(DirtyPercentage > (float)i / DirtSmears.Length);
            }
        }
    }
}