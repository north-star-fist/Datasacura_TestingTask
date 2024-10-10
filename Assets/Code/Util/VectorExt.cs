using UnityEngine;

namespace Datasacura.TestTask.ZooWorld.Util
{

    public static class VectorExt
    {

        public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);

        public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);

        public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);

        public static Vector3 WithXIf(this Vector3 v, float x, bool condit) => condit ? new Vector3(x, v.y, v.z) : v;

        public static Vector3 WithYIf(this Vector3 v, float y, bool condit) => condit ? new Vector3(v.x, y, v.z) : v;

        public static Vector3 WithZIf(this Vector3 v, float z, bool condit) => condit ? new Vector3(v.x, v.y, z) : v;

        /// <summary>
        /// Returns vector each component of whech is multiple of <paramref name="precision"/>
        /// that is approximatelly equal to corresponding component of source <paramref name="v"/> vector.
        /// </summary>
        /// <param name="v"> vector to quantize </param>
        /// <param name="precision"> precision </param>
        /// <returns> quantized vector </returns>
        public static Vector3 Quantize(this Vector3 v, float precision = 0.01f)
        {
            return new Vector3(Quantize(v.x, precision), Quantize(v.y, precision), Quantize(v.z, precision));
        }

        static float Quantize(float number, float precision) => Mathf.Round(number / precision) * precision;
    }
}
