using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;


namespace CpLibrary.Judge.Downloader
{
	public class Downloader
	{
		public IEnumerable<(string, string)> DownloadTestcases(Uri url)
		{
			var atcoderHost = "atcoder.jp";
			if (url.Host == atcoderHost)
			{
				var req = WebRequest.Create(url);
				var html = string.Empty;

				using (var res = req.GetResponse())
				using (var resString = res.GetResponseStream())
				using (var sr = new StreamReader(resString, Encoding.Default))
				{
					html = sr.ReadToEnd();
				}
								
				var parser = new HtmlParser();
				
				var doc = parser.ParseDocument(html);

				var s = doc.GetElementById("task-statement")
					.GetElementsByClassName("part");

				var inputs = new List<string>();
				var outputs = new List<string>();
				var regexIn = new Regex("入力例 *(\\d+)");
				var regexOut = new Regex("出力例 *(\\d+)");

				foreach (var text in s)
				{
					if (regexIn.IsMatch(text.TextContent))
					{
						inputs.Add(text.GetElementsByTagName("pre")[0].TextContent);
					}
					if (regexOut.IsMatch(text.TextContent))
					{
						outputs.Add(text.GetElementsByTagName("pre")[0].TextContent);
					}
				}
				return Enumerable.Zip(inputs, outputs);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
