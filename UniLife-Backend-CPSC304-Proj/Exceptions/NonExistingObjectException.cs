namespace UniLife_Backend_CPSC304_Proj.Exceptions
{
    public class NonExistingObjectException: Exception
    {
        public NonExistingObjectException(string? message) : base(message)
        {
        }
    }
}
