using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoaderToCloud
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loader = new YandexLoader()
            {
                FilePaths = new List<string>()
                {
                    @"ПутьКФайлу1",
                    @"ПутьКФайлу2",
                },
                Token = "Токен"
            };
            await loader.LoadFilesAsync();
        }
    }
}
