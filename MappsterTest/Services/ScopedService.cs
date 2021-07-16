using System;
using System.Threading.Tasks;

namespace MappsterTest.Services
{
    public class ScopedService
    {
        private DateTime Date { get; set; }

        public ScopedService()
        {
            Date = DateTime.Now;
        }

        public async Task<string> DateAsync()
        {
            var result = await Task.Run(() =>
            {
                return $"Scoped ({Date})";
            });

            return result;
        }
    }
}
