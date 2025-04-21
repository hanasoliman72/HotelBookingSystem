namespace BookingSystem.Data
{
    public static class DbInitializer
    {
        public static void Initalize(BookingContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
