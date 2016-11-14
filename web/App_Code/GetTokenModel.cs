using System;

public class GetTokenModel
{
    // ReSharper disable  InconsistentNaming
    public string grant_type { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public DateTime expiry { get; set; }
}