using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dishes
{
    public class RinsingSink : MonoBehaviour
    {
        public bool NeedToDryDish => PreppingDish && _rinsedDish;
        public bool NeedToRinseDish => PreppingDish && !_rinsedDish;
        public bool AcceptingDishes => !PreppingDish;

        public float RinseWaterDuration = 1.5f;

        public Dish.DishType PreppedDish;
        public bool PreppingDish => PreppedDish != Dish.DishType.None;
        public Dish PreppedPlate;
        public Dish PreppedGlass;
        public Dish PreppedFork;
        public Dish PreppedKnife;

        public ParticleSystem WaterStream;
        public Canvas StationCanvas;

        private bool _rinsedDish;
        private Coroutine _stopWater;

        protected void Start()
        {
            PreppedDish = Dish.DishType.None;
            ResolvePreppedDish();
        }

        public void TryMoveDish()
        {
            if (!AcceptingDishes) return;
            if (DishWasher.Instance.DirtySinkMgr.TryRemoveDish(out Dish.DishType dish))
            {
                PreppedDish = dish;
                _rinsedDish = false;
                ResolvePreppedDish();
            }
        }

        public void RunTap()
        {
            if (PreppingDish) _rinsedDish = true; // you can keep rinsing its fine

            WaterStream.Play();
            if (_stopWater != null) StopCoroutine(_stopWater);
            _stopWater = StartCoroutine(TurnWaterOff());

            DishWasher.Instance.ResolveObjective();
        }

        IEnumerator TurnWaterOff()
        {
            yield return new WaitForSeconds(RinseWaterDuration);
            WaterStream.Stop();
        }

        public void MoveDishToRack()
        {
            if (NeedToDryDish)
            {
                PreppedDish = Dish.DishType.None;
                ResolvePreppedDish();
                Debug.Log("play glass clink");
                if (Random.Range(0, 4) == 0)
                    DishWasher.Instance.DishSpawnMgr.AddLoad();
            }
        }

        public bool MoveDishHere(Dish.DishType type)
        {
            throw new System.ApplicationException("not used");
            if (!AcceptingDishes) return false;

            _rinsedDish = false;
            PreppedDish = type;
            ResolvePreppedDish();
            return true;
        }

        public void SetCanvas(bool showing)
        {
            StationCanvas.enabled = showing;
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
                    PreppedPlate.Show();
                    break;
                case Dish.DishType.Glass:
                    PreppedGlass.Show();
                    break;
                case Dish.DishType.Fork:
                    PreppedFork.Show();
                    break;
                case Dish.DishType.Knife:
                    PreppedKnife.Show();
                    break;
            }

            DishWasher.Instance.ResolveObjective();
        }
    }
}