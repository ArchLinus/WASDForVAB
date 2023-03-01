using SpaceWarp.API.Configuration;
using Newtonsoft.Json;

namespace WASDForVAB
{
    [ModConfig]
    [JsonObject(MemberSerialization.OptOut)]
    class WASDConfig
    {
        [ConfigField("CameraSensitivity")]
        [ConfigDefaultValue(1.0f)]
        public float CameraSensitivity;
    }
}
