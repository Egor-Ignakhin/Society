namespace Shoots
{
    public struct BulletValues
    {
        public float CurrentDistance { set; get; }
        public float MaxDistance { get; }
        public float Caliber { get; }
        public float Speed { get; set; }
        public float StartSpeed { get; }
        public float Angle { get; set; }// угол между стрелком и местом попадания
        public UnityEngine.Vector3 PossibleReflectionPoint { get; set; }
        public UnityEngine.LayerMask Layers { get; set; }
        public static float Energy(float mass, float speed)
        {//m*V*V/2
            return ((float)(mass * System.Math.Pow(speed, 2)) / 2) * 0.01f;
        }
        public static bool CanReflect(float currentE, float startE)
        {            
            return currentE / startE >= 0.5f;
        }
        public BulletValues(float currentDistance, float maxDistance, float caliber, float speed,
            float angle, UnityEngine.Vector3 prp, UnityEngine.LayerMask lm)
        {
            this.CurrentDistance = currentDistance;
            this.MaxDistance = maxDistance;
            this.Caliber = caliber;
            StartSpeed = this.Speed = speed;
            Angle = angle;
            PossibleReflectionPoint = prp;
            Layers = lm;
        }
    }
}