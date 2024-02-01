namespace AgileRap_Process
{
    public static class GlobalVariable
    {
        public static int UserID { get; set; }

        public static void SetUserID(int id)
        {
            UserID = id;
        }

        public static int GetUserID()
        {
            return UserID;
        }

    }
}
