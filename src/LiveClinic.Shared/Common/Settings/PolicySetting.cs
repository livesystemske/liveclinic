using System.Collections.Generic;

namespace LiveClinic.Shared.Common.Settings;

public class PolicySetting
{
    public const string Key = "Policy";
    public string App { get; set; }
    public List<Policy> Definitions { get; set; } = new List<Policy>();
}

public class Policy
{
    public string Name { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
}