using UnityEngine;
namespace Tools
{
    [CreateAssetMenu(fileName = "ApplicationVersion", menuName = "Tools/ApplicationVersion", order = 1)]
    sealed class ApplicationVersion : ScriptableObject
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