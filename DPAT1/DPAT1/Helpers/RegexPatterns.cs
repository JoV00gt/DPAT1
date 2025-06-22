using System.Text.RegularExpressions;

public static class RegexPatterns
{
    public static readonly Regex Trigger = new Regex(
        @"^TRIGGER\s+(?<name>\S+)\s+""(?<desc>[^""]+)"";?$",
        RegexOptions.Compiled
    );

    public static readonly Regex Transition = new Regex(
        @"^TRANSITION\s+(?<id>\S+)\s+(?<from>\S+)\s*->\s*(?<to>\S+)(?:\s+(?<trigger>\S+))?\s*(?:""(?<guard>[^""]*)"")?\s*;?\s*$",
        RegexOptions.Compiled
    );

    public static readonly Regex Action = new Regex(
        @"^ACTION\s+(?<name>\S+)\s+""(?<desc>[^""]+)""\s*:\s*(?<type>\w+);?$",
        RegexOptions.Compiled
    );

    public static readonly Regex State = new Regex(
        @"^STATE\s+(?<name>\S+)\s+(?<parent>\S+)\s+""(?<desc>[^""]*)""\s*:\s*(?<type>\w+)\s*;?\s*$",
        RegexOptions.Compiled
    );
}