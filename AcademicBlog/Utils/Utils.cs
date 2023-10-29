namespace AcademicBlog.Utils
{
    public static class Utils
    {
        public static string GetRandomPastelColor()
        {
            var pastelColors = new List<string>
    {
        "bg-gray-50  text-gray-600 ring-gray-500/10",
        "bg-red-50 text-red-700 ring-red-600/10",
        "bg-yellow-50 text-yellow-800 ring-yellow-600/20",
        "bg-green-50 text-green-700 ring-green-600/20",
        "bg-blue-50  text-blue-700 ring-blue-700/10",
        "bg-indigo-50 text-indigo-700 ring-indigo-700/10",
        "bg-purple-50 text-purple-700 ring-purple-700/10",
        "bg-pink-50 text-pink-700 ring-pink-700/10"
    };

            var randomIndex = new Random().Next(pastelColors.Count);
            return pastelColors[randomIndex];
        }
    }
}
