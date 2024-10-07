using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppConfiguration
{
    public static void ConfigDataSetting(string key, string value)
    {
        string ConfigData = ConfigurationManager.AppSettings[key];

        if (ConfigData == null /*|| ConfigData == "0"*/)
        {
            AddAppConfig(key, value);
        }
        else
        {
            SetAppConfig(key, value);
        }
    }

    public static string GetAppConfig(string key) //불러오기
    {
        string ConfigData = ConfigurationManager.AppSettings[key];

        if (ConfigData == null)
        {
            return string.Empty;
        }

        return ConfigData;
    }

    public static void AddAppConfig(string key, string value) //추가
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

        cfgCollection.Add(key, value);

        config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
    }

    public static void SetAppConfig(string key, string value) //수정
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);// 현재 응용 프로그램의 구성 파일을 System.Configuration.Configuration 개체로 엽니다.
        KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings; //key 값을 가지고 옵니다

        cfgCollection.Remove(key); //key 값에 대한 config 파일 삭제
        cfgCollection.Add(key, value);  //key 값에 대한 value 값 Config 파일 추가

        config.Save(ConfigurationSaveMode.Modified); //Config파일 저장
        ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);//명명된 섹션을 새로 고쳐서 다음에 검색할 때 디스크에서 다시 읽도록 합니다
    }

    public static void RemoveAppConfig(string key) //삭제
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

        try
        {
            cfgCollection.Remove(key);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }
        catch { }
    }
}
