namespace Shoots
{
    public struct BulletValues
    {
        public float CurrentDistance { set; get; }
        public float MaxDistance { get; }
        public float Caliber { get; }
        public float Speed { get; }
        public BulletValues(float currentDistance, float maxDistance, float caliber, float speed)
        {
            this.CurrentDistance = currentDistance;
            this.MaxDistance = maxDistance;
            this.Caliber = caliber;
            this.Speed = speed;
        }
    }
}