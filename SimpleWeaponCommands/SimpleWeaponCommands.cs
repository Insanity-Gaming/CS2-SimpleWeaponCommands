using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace SimpleWeaponCommands;

public class SimpleWeaponCommands : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName { get; } = "Simple Weapon Commands";
    public override string ModuleVersion { get; } = "1.0";
    public override string ModuleAuthor { get; } = "Retro";
    public override string ModuleDescription { get; } = "A simple plugin that allows you to bind weapons to commands.";
    public Config Config { get; set; } = new();

    private Dictionary<string, bool> _isPrimary = new()
    {
        {"weapon_ak47", true},
        {"weapon_awp", true},
        {"weapon_deagle", true},
        {"weapon_m4a1", true},
        {"weapon_m4a1_silencer", true},
        {"weapon_sg553", true},
        {"weapon_aug", true},
        {"weapon_ssg08", true},
        {"weapon_negev", true},
        {"weapon_bizon", true},
        {"weapon_m249", true},
        {"weapon_famas", true},
        {"weapon_galilar", true},
        {"weapon_glock", false},
        {"weapon_hkp2000", false},
        {"weapon_usp_silencer", false},
        {"weapon_tec9", false},
        {"weapon_p250", false},
        {"weapon_cz75a", false},
        {"weapon_fiveseven", false},
        {"weapon_elite", false},
        {"weapon_revolver", false},
        {"weapon_mp5sd", true},
        {"weapon_ump45", true},
        {"weapon_mp9", true},
        {"weapon_p90", true},
        {"weapon_mp7", true},
        {"weapon_mac10", true},
        {"weapon_sg556", true},
        {"weapon_g3sg1", true},
        {"weapon_scar20", true},
        {"weapon_xm1014", true},
        {"weapon_mag7", true},
        {"weapon_sawedoff", true},
        {"weapon_nova", true},
    };
    
    private Dictionary<ulong, float> _lastCommand = new();
    
    public void OnConfigParsed(Config config)
    {
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        foreach (var key in Config.Commands.Keys)
        {
            AddCommand($"css_{key}", $"Gives the player a {key}", OnWeaponCommand);
        }
    }
    
    private bool IsWeaponPrimary(string weapon) => _isPrimary.TryGetValue(weapon, out var isPrimary) && isPrimary;

    [CommandHelper(whoCanExecute:CommandUsage.CLIENT_ONLY)]
    private void OnWeaponCommand(CCSPlayerController? player, CommandInfo cmd)
    {
        if (player is null) return;
        if (player.TeamNum < 2 || player.LifeState != (byte) LifeState_t.LIFE_ALIVE) return;
        
        if (_lastCommand.TryGetValue(player.SteamID, out var lastCommand) && Server.CurrentTime - lastCommand < 5f)
        {
            cmd.ReplyToCommand("You are sending commands too quickly.");
            return;
        }
        
        var shortHand = cmd.GetCommandString.Replace("css_", "");
        if (!Config.Commands.TryGetValue(shortHand, out var weapon)) return;

        var currentWeapon = player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon;
        var isNewWeaponPrimary = IsWeaponPrimary(weapon);

        if (currentWeapon is {IsValid: true, Value: not null})
        {
            var name = currentWeapon.Value.DesignerName;
            DropWeaponByDesignName(player, name);
            Server.NextFrame(() =>
            {
                player.GiveNamedItem(weapon);
                player.ExecuteClientCommand($"slot{(isNewWeaponPrimary ? "1" : "2")}");
            });
            _lastCommand[player.SteamID] = Server.CurrentTime;
        }
        else
        {
            Server.NextFrame(() =>
            {
                player.GiveNamedItem(weapon);
            });
        }
    }
    
    public bool DropWeaponByDesignName(CCSPlayerController? controller, string weaponName)
    {
        if(controller is null) return false;

        if (!controller.PlayerPawn.IsValid || controller.PlayerPawn.Value is null) return false;

        var pawn = controller.PlayerPawn.Value;
        
        if(pawn.WeaponServices is null) return false;

        var weaponServices = pawn.WeaponServices;
        
        var weapon = weaponServices.MyWeapons.FirstOrDefault(x => x.Value?.DesignerName == weaponName);

        if (weapon is null || !weapon.IsValid) return false;
        
        weaponServices.ActiveWeapon.Raw = weapon.Raw;
        controller.DropActiveWeapon();
        weapon.Value!.AddEntityIOEvent("Kill", delay:0.1f);
        return true;
    }
}