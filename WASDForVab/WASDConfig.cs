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

        [ConfigField("BaseSpeed")]
        [ConfigDefaultValue(20.0f)]
        public float BaseSpeed = 20.0f;

        [ConfigField("FastSpeedMultiplier")]
        [ConfigDefaultValue(3.0f)]
        public float FastSpeedMultiplier = 3.0f;

        [ConfigField("SlowSpeedMultiplier")]
        [ConfigDefaultValue(0.4f)]
        public float SlowSpeedMultiplier = 0.4f;
    }
}
