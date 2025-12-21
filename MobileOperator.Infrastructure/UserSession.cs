namespace MobileOperator.Infrastructure
{
    /// <summary>
    /// Текущий сессия пользоватееля
    /// </summary>
    public static class UserSession
    {
        public static int UserId { get; private set; }
        public static int StatusId { get; private set; }
        public static string UserType { get; private set; }
        public static bool IsLoggedIn { get; private set; }

        public static void StartSession(int id, int status, string type)
        {
            UserId = id;
            StatusId = status;
            UserType = type;
            IsLoggedIn = true;
        }

        public static void EndSession()
        {
            UserId = 0;
            StatusId = 0;
            UserType = null;
            IsLoggedIn = false;
        }
    }
}