using UnityEngine;
namespace SMG
{
    /// <summary>
    /// вешается на модифицируемый элемент оружия на верстаке
    /// </summary>
    public class SMGGunElement : MonoBehaviour
    {
        [SerializeField] private bool isEmpty;
        public bool IsEmpty() => isEmpty;
    }
}