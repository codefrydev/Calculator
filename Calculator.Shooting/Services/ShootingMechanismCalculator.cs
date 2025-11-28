using Calculator.Shooting.Helpers;
using Calculator.Shooting.Models;

namespace Calculator.Shooting.Services;

public static class ShootingMechanismCalculator
{
    private static readonly List<MotorData> Motors = MotorsHelper.Motors;

    public static ShooterResults Calculate(ShooterInputs inputs, string motorId)
    {
        var motor = Motors.FirstOrDefault(m => m.Id == motorId) ?? Motors[0];
        return CalculateShootingPhysics(inputs, motor);
    }

    private static ShooterResults CalculateShootingPhysics(ShooterInputs inputs, MotorData motorData)
    {
        var results = new ShooterResults();

        // Gear ratio calculation
        double totalOutput = 1;
        double totalInput = 1;
        
        foreach (var stage in inputs.Stages)
        {
            totalOutput *= stage.OutputGear;
            totalInput *= stage.InputGear;
        }

        results.GearRatio = totalOutput / totalInput;
        
        // Motor characteristics
        var freeSpeed = motorData.FreeSpeed;
        var stallTorque = motorData.StallTorque;
        var stallCurrent = motorData.StallCurrent;
        var freeCurrent = motorData.FreeCurrent;

        results.MaxRotationalSpeed = freeSpeed / results.GearRatio;
        results.RotationalSpeed = Math.Min(results.MaxRotationalSpeed, inputs.WheelTargetSpeed);
        results.SpeedOverhead = (1 - results.RotationalSpeed / results.MaxRotationalSpeed) * 100;
        
        // Surface speed in ft/s
        results.SurfaceSpeed = (results.RotationalSpeed * (Math.PI / 30) * (inputs.WheelDiameter / 2)) / 12;
        
        var maxRotationalSpeedRadPerSec = results.MaxRotationalSpeed * (Math.PI / 30);
        
        // MOI Calculations (lb-in^2)
        double styleMultiplier = inputs.ShooterStyle == 2 ? 2 : 1;
        
        results.WheelMOI = styleMultiplier * 0.5 * inputs.WheelMass * Math.Pow(inputs.WheelDiameter / 2, 2);
        results.FwMOI = styleMultiplier * 0.5 * inputs.FlywheelMass * Math.Pow(inputs.FlywheelMassDiameter / 2, 2) * Math.Pow(inputs.FlywheelRatio, 2);
        results.CustomMOIEff = styleMultiplier * inputs.CustomMOI * Math.Pow(inputs.CustomMOIRatio, 2);
        
        results.TotalMOI = results.WheelMOI + results.FwMOI + results.CustomMOIEff;
        
        // Speed Transfer
        double term1 = 2;
        var term2 = (inputs.ProjectileMass * Math.Pow(inputs.WheelDiameter/2, 2)) / results.TotalMOI;
        var term3 = (0.4 * inputs.ProjectileMass * Math.Pow(inputs.ProjectileDiameter/2, 2) / results.TotalMOI) * Math.Pow(inputs.WheelDiameter/inputs.ProjectileDiameter, 2);
        results.SpeedTransfer = results.TotalMOI > 0 ? (styleMultiplier * 1 / (term1 + term2 + term3)) : 0;
        
        results.ProjectileSpeed = results.SurfaceSpeed * results.SpeedTransfer;
        var totalMoiSi = results.TotalMOI * 0.00029264; 
        
        var torqueTotal = (inputs.GearboxEfficiency / 100) * inputs.NumMotors * stallTorque * results.GearRatio; 
        
        // Spin-up time
        results.SpinUpTime = torqueTotal > 0 ? 
            -totalMoiSi * maxRotationalSpeedRadPerSec * Math.Log(Math.Max(0.0001, (results.MaxRotationalSpeed - results.RotationalSpeed)) / results.MaxRotationalSpeed) / 
            ((inputs.GearboxEfficiency/100) * inputs.NumMotors * stallTorque * results.GearRatio)
            : 0;
                
        // Energy calculations
        var rotSpeedRadS = results.RotationalSpeed * (Math.PI / 30);
        results.FlywheelEnergy = 0.5 * results.TotalMOI * Math.Pow(rotSpeedRadS, 2) * 0.00029264; 
        
        var ballRotationalEnergyTerm = 0.5 * 0.4 * inputs.ProjectileMass * Math.Pow(inputs.ProjectileDiameter/24, 2) * Math.Pow(results.ProjectileSpeed / (inputs.ProjectileDiameter/24), 2);
        results.ProjectileEnergy = (0.5 * inputs.ProjectileMass * Math.Pow(results.ProjectileSpeed, 2) + ballRotationalEnergyTerm) * 0.04214;

        results.EnergyLostPerShot = results.FlywheelEnergy > 0 ? 
            (results.ProjectileEnergy / results.FlywheelEnergy / (inputs.ShotEfficiency/100)) * 100 : 0;
        
        results.SpeedAfterShot = results.TotalMOI > 0 ? 
            Math.Sqrt(Math.Max(0, results.FlywheelEnergy * (1 - results.EnergyLostPerShot/100) * 2 / results.TotalMOI / 0.00029264)) / (Math.PI/30)
            : 0;
                
        // Recovery Time
        results.RecoveryTime = torqueTotal > 0 ? 
            -1000 * totalMoiSi * maxRotationalSpeedRadPerSec * 
            Math.Log(Math.Max(0.0001, (results.MaxRotationalSpeed - Math.Max(results.RotationalSpeed * (1 - inputs.WheelSpeedVariation/100), results.SpeedAfterShot))) / 
            (results.MaxRotationalSpeed - results.SpeedAfterShot)) / 
            ((inputs.GearboxEfficiency/100) * inputs.NumMotors * stallTorque * results.GearRatio)
            : 0;

        results.ShotRate = results.RecoveryTime > 0 ? 1 / (results.RecoveryTime / 1000) : 0;
        results.PeakCurrentPerMotor = (1 - results.SpeedAfterShot/results.MaxRotationalSpeed) * (stallCurrent - freeCurrent) + freeCurrent;

        return results;
    }

    public static List<(string MotorName, ShooterResults Results)> CalculateForAllMotors(ShooterInputs inputs)
    {
        var allResults = new List<(string MotorName, ShooterResults Results)>();
        
        foreach (var motor in Motors)
        {
            var results = CalculateShootingPhysics(inputs, motor);
            results.MotorName = motor.Name;
            allResults.Add((motor.Name, results));
        }
        
        return allResults;
    }
}