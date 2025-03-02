using CounterStrikeSharp.API.Core;

namespace SimpleWeaponCommands;

public class Config : BasePluginConfig
{
    public Dictionary<string, string> Commands { get; set; } = new()
    {
        { "ak47", "weapon_ak47" },
        { "aug", "weapon_aug" },
        { "awp", "weapon_awp" },
        { "bizon", "weapon_bizon" },
        { "cz75a", "weapon_cz75a" },
        { "deagle", "weapon_deagle" },
        { "elite", "weapon_elite" },
        { "famas", "weapon_famas" },
        { "fiveseven", "weapon_fiveseven" },
        { "g3sg1", "weapon_g3sg1" },
        { "galilar", "weapon_galilar" },
        { "glock", "weapon_glock" },
        { "hkp2000", "weapon_hkp2000" },
        { "knife", "weapon_knife" },
        { "m249", "weapon_m249" },
        { "m4", "weapon_m4a1" },
        { "m4a1_silencer", "weapon_m4a1_silencer" },
        { "m4a1s", "weapon_m4a1_silencer" },
        { "mac10", "weapon_mac10" },
        { "mag7", "weapon_mag7" },
        { "mp5sd", "weapon_mp5sd" },
        { "mp7", "weapon_mp7" },
        { "mp9", "weapon_mp9" },
        { "negev", "weapon_negev" },
        { "nova", "weapon_nova" },
        { "p250", "weapon_p250" },
        { "p90", "weapon_p90" },
        { "revolver", "weapon_revolver" },
        { "sawedoff", "weapon_sawedoff" },
        { "scar20", "weapon_scar20" },
        { "sg556", "weapon_sg556" },
        { "ssg08", "weapon_ssg08" },
        { "scout", "weapon_ssg08" },
        { "tec9", "weapon_tec9" },
        { "ump45", "weapon_ump45" },
        { "ump", "weapon_ump45" },
        { "usp", "weapon_usp_silencer" },
        { "xm1014", "weapon_xm1014" },
        { "xm", "weapon_xm1014" }
    };
}