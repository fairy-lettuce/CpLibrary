using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpLibrary.Judge.Downloader;
using Xunit;
using FluentAssertions;

namespace CpLibrary.Test.Judge.Downloader
{
	public class DownloaderTest
	{
		public static IEnumerable<object[]> GetData()
		{
			yield return new object[]
			{
				"https://atcoder.jp/contests/abc311/tasks/abc311_a",
				new List<(string, string)>
				{
					("5\nACABB\n", "4\n"),
					("4\nCABC\n", "3\n"),
					("30\nAABABBBABABBABABCABACAABCBACCA\n", "17\n")
				}
			};
			yield return new object[]
			{
				"https://atcoder.jp/contests/arc164/tasks/arc164_a",
				new List<(string, string)>
				{
					("4\n5 3\n17 2\n163 79\n1000000000000000000 1000000000000000000\n", "Yes\nNo\nYes\nYes\n")
				}
			};
		}

		[Theory(Skip = "To reduce traffic to AtCoder.jp, Downloader should not be used frequently.")]
		[MemberData(nameof(GetData))]
		public async void ABCFetchTest(string url, IEnumerable<(string, string)> testcase)
		{
			var dl = await CpLibrary.Judge.Downloader.Downloader.DownloadTestcases(new Uri(url));

			dl.Should().Equal(testcase);
		}
	}
}
