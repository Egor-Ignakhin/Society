namespace Society.Shoot
{
    /// <summary>
    /// значения для пуль
    /// </summary>
    public struct BulletValues
    {
        public float CoveredDistance { get; private set; }// пройденная дистанция
        public float MaxDistance { get; }// максимальная дистанция
        public float Caliber { get; }// калибр
        public float Speed { get; private set; } // скорость полёта
        public float StartSpeed { get; }// стартовая скорость
        public float Angle { get; private set; }// угол между стрелком и местом попадания
        public UnityEngine.Vector3 PossibleReflectionPoint { get; set; }// точка возможного рикошета
        public UnityEngine.LayerMask Layers { get; }// учитываемые слои
        /// <summary>
        /// просчёт энергии пули (E kin.)
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static float Energy(float mass, float speed)
        {//m*V*V/2
            return ((float)(mass * System.Math.Pow(speed, 2)) / 2) * 0.01f;
        }
        /// <summary>
        ///  Рикошет возможен?
        /// </summary>
        /// <param name="currentE"></param>
        /// <param name="startE"></param>
        /// <param name="speed"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static bool CanReflect(float mass, float kf, float currentSpeed, float startSpeed, float angle)
        {
            float currentEnergy = Energy(mass * kf, currentSpeed);
            float startEnergy = Energy(mass * kf, startSpeed);

            return currentEnergy / startEnergy * currentSpeed > 1f && angle > 10 && angle < 20 && UnityEngine.Random.Range(0, 101) > 95;            
        }
        public BulletValues(float currentDistance, float maxDistance, float caliber, float speed,
            float angle, UnityEngine.Vector3 prp, UnityEngine.LayerMask lm)
        {
            CoveredDistance = currentDistance;
            this.MaxDistance = maxDistance;
            this.Caliber = caliber;
            StartSpeed = this.Speed = speed;
            Angle = angle;
            PossibleReflectionPoint = prp;
            Layers = lm;
        }
        /// <summary>
        /// обновление значений
        /// </summary>
        /// <param name="d"></param>
        /// <param name="prp"></param>
        /// <param name="a"></param>
        public void SetValues(float d, UnityEngine.Vector3 prp, float a)
        {
            CoveredDistance += d;
            PossibleReflectionPoint = prp;
            Angle = a;
        }
    }
}