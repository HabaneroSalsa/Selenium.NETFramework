namespace OnlineStore.TestDataAccess
{
    class UserData
    {
        public string Key { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    class ShoppingData
    {
        public string ItemName { get; set; }
        public string ItemURL { get; set; }
        public string ItemAddToCartLocator { get; set; }
    }
}
