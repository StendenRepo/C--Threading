using System.Text.RegularExpressions;

namespace MainApp.Models;

public class TuningResult
{
    private double HorsePowerAfterTuning { get; set; }
    private double TorqueAfterTuning { get; set; }
    
    public string HorsePowerDifferenceAsString { get; private set; }
    public string TorqueDifferenceAsString { get; private set; }

    public string HorsePowerAfterTuningAsString { get; private set; }

    public string TorqueAfterTuningAsString { get; private set; }
    
    public TuningResult(string horsePower, string torque)
    {
        CalculateTuningResult(horsePower, torque);
    }

    private void CalculateTuningResult(string horsePower, string torque)
    {
        var random = new Random();
        var horsePowerAsInt = Convert.ToDouble(Regex.Match(horsePower, @"\d+\.*\d*").Value);
        var torqueAsInt = Convert.ToDouble(Regex.Match(torque, @"\d+\.*\d*").Value);
        
        HorsePowerAfterTuning = horsePowerAsInt + Math.Round(horsePowerAsInt * random.Next(10, 35) / 100);
        TorqueAfterTuning = torqueAsInt + Math.Round(torqueAsInt * random.Next(10, 35) / 100);
        HorsePowerDifferenceAsString = (HorsePowerAfterTuning - horsePowerAsInt) + " PK";
        TorqueDifferenceAsString = TorqueAfterTuning - torqueAsInt + " Nm";
        HorsePowerAfterTuningAsString = HorsePowerAfterTuning + " PK";
        TorqueAfterTuningAsString = TorqueAfterTuning + " Nm";
    }
}