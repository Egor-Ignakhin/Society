using UnityEngine;
namespace Society.ApplicationTools
{
    [CreateAssetMenu(fileName = "ApplicationVersion", menuName = "Tools/ApplicationVersion", order = 1)]
    internal sealed class ApplicationVersion : ScriptableObject
    {
        public int MajorVersion;
        public int MinorVersion;
        public int BuildVersion;
        public int RevisionVersion;

        public override string ToString()
        {
            return $"{MajorVersion}.{MinorVersion}.{BuildVersion}.{RevisionVersion}";
        }
    }
}