using System.Runtime.Serialization;

namespace ModpacksCH.API.Model
{
    public enum ModpackFileType
    {
        [EnumMember(Value = "config")]
        Config,

        [EnumMember(Value = "mod")]
        Mod,

        [EnumMember(Value = "resource")]
        Resource,

        [EnumMember(Value = "script")]
        Script,

        [EnumMember(Value = "cf-extract")]
        CFExtract
    };
}