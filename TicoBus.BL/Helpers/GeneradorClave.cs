namespace TicoBus.BL.Helpers
{
    public static class GeneradorClave
    {
        public static string Generar()
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789@$*";
            var random = new Random();

            return new string(Enumerable.Repeat(caracteres, 10)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}