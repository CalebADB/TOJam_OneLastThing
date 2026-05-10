using UnityEngine;


    public static class Utils
    {
        public static Vector3 DivideVector(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(
                vec1.x / vec2.x,
                vec1.y / vec2.y,
                vec1.z / vec2.z
            );
        }
        public static Vector3 MultiplyVector(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(
                vec1.x * vec2.x,
                vec1.y * vec2.y,
                vec1.z * vec2.z
            );
        }
    }
    