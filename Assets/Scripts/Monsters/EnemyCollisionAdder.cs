using UnityEngine;

namespace Society.Editor
{
    public class EnemyCollisionAdder : MonoBehaviour
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