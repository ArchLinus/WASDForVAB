using SpaceWarp.API.Configuration;
using Newtonsoft.Json;

namespace WASDForVAB
{
    [ModConfig]
    [JsonObject(MemberSerialization.OptOut)]
    public class WASDConfig
    {
        [ConfigField("CameraSensitivity")]
        [ConfigDefaultValue(1.0f)]
        public float CameraSensitivity = 1.0f;
    }
}
