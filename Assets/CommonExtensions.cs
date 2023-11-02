namespace Assets
{
    public static class CommonExtensions
    {
        public static int GetXOffset(this TypeDirection direction)
        {
            switch (direction)
            {
                case TypeDirection.East:
                    return 1;
                case TypeDirection.West:
                    return -1;
                default:
                    return 0;
            }
        }

        public static int GetYOffset(this TypeDirection direction)
        {
            switch (direction)
            {
                case TypeDirection.North:
                    return 1;
                case TypeDirection.South:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}
