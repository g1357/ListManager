using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.Services;

public class SettingsService :ISettingsService
{
    #region Settings Constants
    private const string DataFileNameKey = "data_file_name";
    private const string StampFileNameKey = "stamp_file_name";
    private const string AccessTokenKey = "access_token";

    private readonly string DataFileDefault = "Data.lmd";
    private readonly string StampFileDefault = "Stamp.lms";
    private readonly string AccessTokenDefault = string.Empty;

    #endregion Settings Constants

    #region Settings Properties

    public string DataFileName
    {
        get => Preferences.Get(DataFileNameKey, DataFileDefault);
        set => Preferences.Set(DataFileNameKey, value);
    }

    public string StampFileName
    {
        get => Preferences.Get(StampFileNameKey, StampFileDefault);
        set => Preferences.Set(StampFileNameKey, value);
    }

    public string DataDirectory => FileSystem.AppDataDirectory;

    public string AuthAccessToken
    {
        get => Preferences.Get(AccessTokenKey, AccessTokenDefault);
        set => Preferences.Set(AccessTokenKey, value);
    }


    #endregion Settings Properties
}
