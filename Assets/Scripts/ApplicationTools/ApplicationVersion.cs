namespace Society.ApplicationTools
{
    [System.Serializable]
    public class ApplicationVersion
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