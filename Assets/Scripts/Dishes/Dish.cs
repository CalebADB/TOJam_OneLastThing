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
            Debug.Log("Play Sound: Scrub");
            DirtyPercentage -= IsCutlery ? Random.Range(0.03f, 0.12f) : Random.Range(0.02f, 0.09f);
            DirtyPercentage = Mathf.Max(0, DirtyPercentage);
            ResolveDirt();
        }

        public void GetDirtySinkDish()
        {
            gameObject.SetActive(true);
            RandomizeDirt(DirtyRandomizationType.FromSoapWater);
        }

        public void RandomizeDirt(DirtyRandomizationType randomization)
        {
            DirtyPercentage = randomization == DirtyRandomizationType.FromSoapWater ? Random.Range(0.4f, 0.8f) : Random.Range(0.6f, 1f);
            ResolveDirt();
        }

        private void ResolveDirt()
        {
            for (int i = 0; i < DirtSmears.Length; i++)
            {
                // Debug.Log($"{DirtyPercentage} vs {(float)i / DirtSmears.Length} that is to say: {DirtyPercentage > (float)i / DirtSmears.Length}");
                DirtSmears[i].gameObject.SetActive(DirtyPercentage > (float)i / DirtSmears.Length);
            }
        }
    }
}