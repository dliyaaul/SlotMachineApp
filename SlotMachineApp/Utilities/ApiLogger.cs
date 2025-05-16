using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SlotMachineApp.Utilities
{
    public static class ApiLogger
    {
        public static void LogResult(string result, int saldo)
        {
            var data = new { result, saldo, timestamp = DateTime.Now };
            var json = JsonSerializer.Serialize(data);
            File.AppendAllText("game_log.txt", json + Environment.NewLine);
        }
    }
}
