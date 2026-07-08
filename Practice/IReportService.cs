using System.Threading.Tasks;
using Practice.Models;

namespace Practice
{
    public interface IReportService {
        Task GenerateAllReportsAsync(AnalysisReport report, string outputDirectory);
    }
}