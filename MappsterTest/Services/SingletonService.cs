using System;
using System.Threading.Tasks;

namespace MappsterTest.Services
{
    public class SingletonService
    {
        private DateTime Date { get; set; }

        public SingletonService()
        {
            Date = DateTime.Now;
        }

        public async Task<string> DateAsync()
        {
            var result = await Task.Run(() =>
            {
                return $"Singleton ({Date})";
            });

            return result;
        }
    }
}
