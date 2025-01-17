using UnityEngine;

namespace Society.Enemies
{
    internal sealed class EnemyCollisionAdder : MonoBehaviour
    {
        private void OnValidate()
        {
            foreach (var c in GetComponentsInChildren<CharacterJoint>())
            {
                c.gameObject.AddComponent<EnemyCollision>().SetEnemyParent(GetComponent<Enemy>());
            }
            DestroyImmediate(this);
        }
    }
}