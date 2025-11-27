using Calculator.Shooting.Models;

namespace Calculator.Shooting.Helpers;

public static class MotorsHelper
{
    public static readonly List<MotorData> Motors =
    [
        new()
        {
            Id = "falcon500", Name = "Falcon 500", Voltage = 12, FreeSpeed = 6380, StallTorque = 4.69,
            StallCurrent = 257, FreeCurrent = 1.5
        },
        new()
        {
            Id = "krakenx60", Name = "Kraken X60", Voltage = 12, FreeSpeed = 6000, StallTorque = 7.09,
            StallCurrent = 366, FreeCurrent = 2.0
        },
        new()
        {
            Id = "neo", Name = "NEO", Voltage = 12, FreeSpeed = 5676, StallTorque = 2.6, StallCurrent = 105,
            FreeCurrent = 1.8
        },
        new()
        {
            Id = "neovortex", Name = "NEO Vortex", Voltage = 12, FreeSpeed = 6784, StallTorque = 3.6,
            StallCurrent = 211, FreeCurrent = 3.6
        },
        new()
        {
            Id = "cim", Name = "CIM", Voltage = 12, FreeSpeed = 5330, StallTorque = 2.41, StallCurrent = 131,
            FreeCurrent = 2.7
        },
        new()
        {
            Id = "minicim", Name = "Mini CIM", Voltage = 12, FreeSpeed = 5840, StallTorque = 1.41, StallCurrent = 89,
            FreeCurrent = 3.0
        },
        new()
        {
            Id = "775pro", Name = "775pro", Voltage = 12, FreeSpeed = 18730, StallTorque = 0.71, StallCurrent = 134,
            FreeCurrent = 0.7
        },
        new()
        {
            Id = "bag", Name = "BAG", Voltage = 12, FreeSpeed = 13180, StallTorque = 0.43, StallCurrent = 53,
            FreeCurrent = 1.8
        }
    ];
}