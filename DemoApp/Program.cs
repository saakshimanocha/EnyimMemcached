using System;
using System.Collections.Generic;
using System.Text;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System.Net;
using Enyim.Caching.Configuration;
using NorthScale.Store;
using NorthScale.Store.Configuration;
using System.Threading;

namespace DemoApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var cfg = new NorthScaleClientConfiguration();
			cfg.Bucket = "default";
			//cfg.BucketPassword = null;

			cfg.Credentials = new NetworkCredential("Administrator", "11111111");
			cfg.Urls.Add(new Uri("http://192.168.2.164:8080/pools/default"));

			var nsc = new NorthScaleClient(cfg);
			//nsc.FlushAll();

			var i = 0;
			var last = true;

			var progress = @"-\|/".ToCharArray();

			while (true)
			{
				var key = "Test_Key_" + i;
				var state = nsc.Store(StoreMode.Set, key, i) & nsc.Get<int>(key) == i;
				Console.CursorVisible = false;

				if (state != last
					|| (i % (state ? 10000 : 1000) == 0))
				{
					Console.ForegroundColor = state ? ConsoleColor.White : ConsoleColor.Red;
					Console.Write(".");
				}
				else if (i % 200 == 0)
				{
					Console.Write(progress[(i / 200) % 4]);
					if (Console.CursorLeft == 0)
					{
						Console.CursorLeft = Console.WindowWidth - 1;
						Console.CursorTop -= 1;
					}
					else
					{
						Console.CursorLeft -= 1;
					}
				}

				i++;
			}
		}
	}
}
