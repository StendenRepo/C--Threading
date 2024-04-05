using MainApp.Models;

namespace MainApp.Logic;

public interface IApiService
{
    Task<CarData> QueryRdwDataAsync(string licensePlate);
    Task<List<CarData>> QueryAllRdwDataInParallel();
}