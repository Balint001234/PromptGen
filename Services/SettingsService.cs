using System;
using System.IO;
using System.Text;
using System.Text.Json;
using PromptGen.Models;

namespace PromptGen.Services;

public class SettingsService
{
    private const string SettingsFileName = "settings.json";
    private AppSettings _settings;
    
    public SettingsService()
    {
        _settings = new AppSettings();
        LoadSettings();
    }

    // Simple Base64 encode to obfuscate the key in the file
    private string EncodeApiKey(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;
        
        try
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            return "B64:" + Convert.ToBase64String(bytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Encode error: {ex.Message}");
            return plainText;
        }
    }

    private string DecodeApiKey(string encoded)
    {
        if (string.IsNullOrEmpty(encoded))
            return string.Empty;
        
        try
        {
            if (encoded.StartsWith("B64:"))
            {
                var base64 = encoded.Substring(4);
                var bytes = Convert.FromBase64String(base64);
                return Encoding.UTF8.GetString(bytes);
            }
            // Compatibility with the previous ENC: format if it exists
            if (encoded.StartsWith("ENC:"))
            {
                var base64 = encoded.Substring(4);
                var bytes = Convert.FromBase64String(base64);
                Array.Reverse(bytes);
                return Encoding.UTF8.GetString(bytes);
            }
            return encoded;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Decode error: {ex.Message}");
            return encoded;
        }
    }

    public string GetSettingsFilePath()
    {
        if (!string.IsNullOrWhiteSpace(_settings.SettingsDirectory) && 
            Directory.Exists(_settings.SettingsDirectory))
        {
            return Path.Combine(_settings.SettingsDirectory, SettingsFileName);
        }

        var exeDirectory = AppContext.BaseDirectory;
        return Path.Combine(exeDirectory, SettingsFileName);
    }

    public void LoadSettings()
    {
        try
        {
            var filePath = GetSettingsFilePath();
            
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loadedSettings = JsonSerializer.Deserialize<AppSettings>(json);
                if (loadedSettings != null)
                {
                    _settings = loadedSettings;
                    // Decode the API key
                    _settings.ApiKey = DecodeApiKey(_settings.ApiKey);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings: {ex.Message}");
        }
    }

    public void SaveSettings()
    {
        try
        {
            var filePath = GetSettingsFilePath();
            var directory = Path.GetDirectoryName(filePath);
            
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var settingsToSave = new AppSettings
            {
                ApiKey = EncodeApiKey(_settings.ApiKey),
                SettingsDirectory = _settings.SettingsDirectory
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(settingsToSave, options);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    public string ApiKey
    {
        get => _settings.ApiKey;
        set
        {
            _settings.ApiKey = value;
            SaveSettings();
        }
    }

    public string SettingsDirectory
    {
        get => _settings.SettingsDirectory;
        set
        {
            var oldPath = GetSettingsFilePath();
            _settings.SettingsDirectory = value;
            
            try
            {
                if (File.Exists(oldPath))
                {
                    var newPath = GetSettingsFilePath();
                    var newDir = Path.GetDirectoryName(newPath);
                    
                    if (!string.IsNullOrEmpty(newDir) && !Directory.Exists(newDir))
                    {
                        Directory.CreateDirectory(newDir);
                    }
                    
                    File.Move(oldPath, newPath, true);
                }
                else
                {
                    SaveSettings();
                }
            }
            catch
            {
                SaveSettings();
            }
        }
    }
}
