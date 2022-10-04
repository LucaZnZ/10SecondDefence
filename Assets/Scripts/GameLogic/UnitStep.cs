using UnityEngine;

namespace GameLogic
{
    public class UnitStep
    {
        public Vector3 target;
        public float speed;

        public UnitStep(Vector3 target, float speed)
        {
            this.target = target;
            this.speed = speed;
        }
    }
}