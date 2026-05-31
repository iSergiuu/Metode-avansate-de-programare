namespace DocumentGenerator.Core.Configuration;

public class ConfigurationManager
{
    private static ConfigurationManager? _instance;

    public string DefaultFormat { get; set; } = "HTML";

    private ConfigurationManager()
    {
    }

    public static ConfigurationManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ConfigurationManager();

            return _instance;
        }
    }
}