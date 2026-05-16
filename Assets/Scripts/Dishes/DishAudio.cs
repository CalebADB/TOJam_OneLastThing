using UnityEngine;

namespace Dishes
{
    public class DishAudio : MonoBehaviour
    {
        public enum AudioActionType
        {
            None = 0,
            PickupPlate,
            PickupGlass,
            PickupFork,
            PickupKnife,
            AddSoap,

        }

        public AudioSource ClipPlayer;

       // public 


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void PlayClip(AudioActionType clip)
        {
            switch (clip)
            {
                default:
                    break;
                case AudioActionType.PickupPlate:
                    //ClipPlayer.PlayOneShot(clip);
                    break;
            }


            
        }
    }
}
