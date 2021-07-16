using System;
using System.Threading.Tasks;

namespace MappsterTest.Services
{
    public class TransientService
    {
        private DateTime Date { get; set; }

        public TransientService()
        {
            Date = DateTime.Now;
        }

        public async Task<string> DateAsync()
        {
            var result = await Task.Run(() =>
            {
                return $"Transient ({Date})";
            });

            return result;
        }
    }
}
