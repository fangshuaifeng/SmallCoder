using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Search;

namespace SmallCoder
{
	internal class SearchResult : TextSegment, ISearchResult, ISegment
	{
		public Match Data { get; set; }

		public string ReplaceWith(string replacement)
		{
			return Data.Result(replacement);
		}
	}
}
