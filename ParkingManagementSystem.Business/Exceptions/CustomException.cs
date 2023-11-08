namespace ParkingManagementSystem.Business.Exceptions
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException() { }

        public CustomException(string name)
            : base(name)
        {

        }
    }
}
