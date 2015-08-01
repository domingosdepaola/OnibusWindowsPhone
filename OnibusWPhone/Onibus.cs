using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnibusWPhone
{
    public class Onibus
    {
        [JsonProperty("DataHora")]
        public DateTime DataHora { get; set; }

        [JsonProperty("Ordem")]
        public string Ordem { get; set; }

        [JsonProperty("Linha")]
        public string Linha { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        [JsonProperty("Velocidade")]
        public int Velocidade { get; set; }
    }
}
