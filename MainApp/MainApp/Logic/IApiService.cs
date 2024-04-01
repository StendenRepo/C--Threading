namespace MainApp.Logic;

public interface IApiService
{
    Task<CarData> QueryRdwData(string licensePlate);
}