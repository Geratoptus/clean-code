using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class Tests
{
    [Test]
    public void Markdown_Render_ShouldWorkFast()
    {
        const int scale = 2;
        var sw = new Stopwatch();
        var results = new List<TimeSpan>();
        for (var len = 640; len <= 655360; len *= scale)
        {
            var markdown = GenerateMarkdown(len);
            GC.Collect();
            sw.Start();
            Md.Render(markdown);
            sw.Stop();
            
            results.Add(sw.Elapsed);
            sw.Reset();
        }
        
        Enumerable.Range(1, results.Count - 1)
            .Select(i => (double)results[i].Ticks / results[i - 1].Ticks)
            .Should().OnlyContain(timeRatio => timeRatio < scale * scale);
    }
    private static string GenerateMarkdown(int len)
    {
        var rand = new Random();
        List<string> specElements = [" ", "_", "__", "#", "\\", Environment.NewLine];
        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXY".Select(char.ToString).ToList();
        
        var allElements = specElements.Concat(alphabet).ToList();
        return Enumerable.Range(0, len).Aggregate(new StringBuilder(), 
            (sb, _) => sb.Append(allElements[rand.Next(allElements.Count)])).ToString();
    }
}