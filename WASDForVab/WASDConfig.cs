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

        [ConfigField("RequireRightClickForControl")]
        [ConfigDefaultValue(true)]
        public bool RequireRightClickForControl = true;

        [ConfigField("KeyForward")]
        [ConfigDefaultValue("W")]
        public string KeyForward = "W";

        [ConfigField("KeyBack")]
        [ConfigDefaultValue("S")]
        public string KeyBack = "S";

        [ConfigField("KeyRight")]
        [ConfigDefaultValue("D")]
        public string KeyRight = "D";

        [ConfigField("KeyLeft")]
        [ConfigDefaultValue("A")]
        public string KeyLeft = "A";

        [ConfigField("KeyUp")]
        [ConfigDefaultValue("E")]
        public string KeyUp = "E";

        [ConfigField("KeyDown")]
        [ConfigDefaultValue("Q")]
        public string KeyDown = "Q";

        [ConfigField("KeyFast")]
        [ConfigDefaultValue("LEFT SHIFT")]
        public string KeyFast = "LEFT SHIFT";

        [ConfigField("KeySlow")]
        [ConfigDefaultValue("LEFT CTRL")]
        public string KeySlow = "LEFT CTRL";        
    }
}
