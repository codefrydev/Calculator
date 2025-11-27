namespace Calculator.Shooting.Models;

public class MotorData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Voltage { get; set; }
    public double FreeSpeed { get; set; }
    public double StallTorque { get; set; }
    public double StallCurrent { get; set; }
    public double FreeCurrent { get; set; }
}
public class ShooterInputs
{
    public List<GearStage> Stages { get; set; } = [new GearStage()];
    public int NumMotors { get; set; } = 1;
    public double GearboxEfficiency { get; set; } = 95;
    public double ShotEfficiency { get; set; } = 80;
    public int ShooterStyle { get; set; } = 1; // 1=Hood, 2=Dual
    public double WheelMass { get; set; } = 0.5;
    public double WheelDiameter { get; set; } = 4;
    public double ProjectileMass { get; set; } = 0.6;
    public double ProjectileDiameter { get; set; } = 9.5;
    public double WheelTargetSpeed { get; set; } = 5000;
    public double WheelSpeedVariation { get; set; } = 1;
    public double FlywheelMass { get; set; } = 0;
    public double FlywheelMassDiameter { get; set; } = 0;
    public double FlywheelRatio { get; set; } = 1;
    public double CustomMOI { get; set; } = 0;
    public double CustomMOIRatio { get; set; } = 1;
}

public class GearStage
{
    public double OutputGear { get; set; } = 1;
    public double InputGear { get; set; } = 1;
}
public class ShooterResults
{
    public string MotorName { get; set;}
    public double GearRatio { get; set; }
    public double ProjectileSpeed { get; set; }
    public double ShotRate { get; set; }
    public double PeakCurrentPerMotor { get; set; }
    public double RotationalSpeed { get; set; }
    public double MaxRotationalSpeed { get; set; }
    public double SpeedOverhead { get; set; }
    public double SurfaceSpeed { get; set; }
    public double SpeedTransfer { get; set; }
    public double SpeedAfterShot { get; set; }
    public double SpinUpTime { get; set; }
    public double RecoveryTime { get; set; }
    public double FlywheelEnergy { get; set; }
    public double ProjectileEnergy { get; set; }
    public double EnergyLostPerShot { get; set; }
    public double WheelMOI { get; set; }
    public double FwMOI { get; set; }
    public double CustomMOIEff { get; set; }
    public double TotalMOI { get; set; }
}