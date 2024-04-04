using MainApp.Models;

namespace MainApp.Logic;

public interface IApiService
{
    Task<CarData> QueryRdwData(string licensePlate);
    Task<List<CarData>> QueryAllRdwData();
}