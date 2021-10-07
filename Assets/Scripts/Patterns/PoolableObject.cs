using UnityEngine;
namespace Society.Patterns
{

    public class PoolableObject : MonoBehaviour
    {
        protected ObjectPool mPool;
        public void OnInit(ObjectPool op) => mPool = op;

        public void DifferenceLifeTime() => mPool.ReturnToPool(this, false);
    }
}