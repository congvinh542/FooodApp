namespace ClickBuy_Api.Database.Common
{
    public static class HelperCommon
    {
        // Generator Code length
        public static string GenerateCode(int length, string prefix)
        {
            Random random = new Random();
            string characters = "0123456789";
            string result = prefix;
            for (int i = 0; i < length; i++)
            {
                result += characters[random.Next(characters.Length)];
            }
            return result;
        }
    }
}