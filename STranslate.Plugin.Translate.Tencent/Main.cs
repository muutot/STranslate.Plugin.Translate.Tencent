using STranslate.Plugin.Translate.Tencent.View;
using STranslate.Plugin.Translate.Tencent.ViewModel;
using System.Text.Json.Nodes;
using System.Windows.Controls;

namespace STranslate.Plugin.Translate.Tencent;

public class Main : TranslatePluginBase
{
    private const string Service = "tmt";
    private const string Version = "2018-03-21";
    private const string Action = "TextTranslate";
    private const string Host = "tmt.tencentcloudapi.com";
    private const string Endpoint = $"https://{Host}";
    private Control? _settingUi;
    private SettingsViewModel? _viewModel;
    private Settings Settings { get; set; } = null!;
    private IPluginContext Context { get; set; } = null!;

    public override Control GetSettingUI()
    {
        _viewModel ??= new SettingsViewModel(Context, Settings);
        _settingUi ??= new SettingsView { DataContext = _viewModel };
        return _settingUi;
    }

    /// <summary>
    ///     https://www.tencentcloud.com/document/product/1264/66057
    /// </summary>
    public override string? GetSourceLanguage(LangEnum langEnum) => langEnum switch
    {
        LangEnum.Auto => "auto",
        LangEnum.ChineseSimplified => "zh",
        LangEnum.ChineseTraditional => "zh-TW",
        LangEnum.English => "en",
        LangEnum.Japanese => "ja",
        LangEnum.Korean => "ko",
        LangEnum.French => "fr",
        LangEnum.Spanish => "es",
        LangEnum.Russian => "ru",
        LangEnum.German => "de",
        LangEnum.Italian => "it",
        LangEnum.Turkish => "tr",
        LangEnum.PortuguesePortugal => "pt",
        LangEnum.PortugueseBrazil => "pt",
        LangEnum.Vietnamese => "vi",
        LangEnum.Indonesian => "id",
        LangEnum.Thai => "th",
        LangEnum.Malay => "ms",
        LangEnum.Arabic => "ar",
        LangEnum.Hindi => "hi",
        LangEnum.Cantonese => "zh",
        _ => "auto"
    };

    /// <summary>
    ///     https://www.tencentcloud.com/document/product/1264/66057
    /// </summary>
    public override string? GetTargetLanguage(LangEnum langEnum) => langEnum switch
    {
        LangEnum.Auto => "auto",
        LangEnum.ChineseSimplified => "zh",
        LangEnum.ChineseTraditional => "zh-TW",
        LangEnum.English => "en",
        LangEnum.Japanese => "ja",
        LangEnum.Korean => "ko",
        LangEnum.French => "fr",
        LangEnum.Spanish => "es",
        LangEnum.Russian => "ru",
        LangEnum.German => "de",
        LangEnum.Italian => "it",
        LangEnum.Turkish => "tr",
        LangEnum.PortuguesePortugal => "pt",
        LangEnum.PortugueseBrazil => "pt",
        LangEnum.Vietnamese => "vi",
        LangEnum.Indonesian => "id",
        LangEnum.Thai => "th",
        LangEnum.Malay => "ms",
        LangEnum.Arabic => "ar",
        LangEnum.Hindi => "hi",
        LangEnum.Cantonese => "zh",
        _ => "auto"
    };

    public override void Init(IPluginContext context)
    {
        Context = context;
        Settings = context.LoadSettingStorage<Settings>();
    }

    public override void Dispose() => _viewModel?.Dispose();

    public override async Task TranslateAsync(TranslateRequest request, TranslateResult result, CancellationToken cancellationToken = default)
    {
        if (GetSourceLanguage(request.SourceLang) is not string sourceStr)
        {
            result.Fail(Context.GetTranslation("UnsupportedSourceLang"));
            return;
        }
        if (GetTargetLanguage(request.TargetLang) is not string targetStr)
        {
            result.Fail(Context.GetTranslation("UnsupportedTargetLang"));
            return;
        }

        var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var payloadObj = new JsonObject
        {
            ["SourceText"] = request.Text,
            ["Source"] = sourceStr,
            ["Target"] = targetStr,
            ["ProjectId"] = 0
        };
        var requestPayload = payloadObj.ToJsonString();

        var canonicalHeaders = $"content-type:application/json\nhost:{Host}\nx-tc-action:{Action.ToLowerInvariant()}\n";
        var signedHeaders = "content-type;host;x-tc-action";

        var (signature, timestamp, credentialScope) = TencentCloudSigner.Sign(
            "POST",
            "/",
            "",
            canonicalHeaders,
            signedHeaders,
            requestPayload,
            Settings.SecretKey,
            date,
            Service
        );

        var authorization = $"TC3-HMAC-SHA256 Credential={Settings.SecretId}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}";

        var options = new Options
        {
            Headers = new Dictionary<string, string>
            {
                ["Authorization"] = authorization,
                ["Host"] = Host,
                ["X-TC-Action"] = Action,
                ["X-TC-Timestamp"] = timestamp,
                ["X-TC-Version"] = Version
            }
        };

        var response = await Context.HttpService.PostAsync(Endpoint, payloadObj, options, cancellationToken: cancellationToken);
        var parsedData = JsonNode.Parse(response);

        var error = parsedData?["Response"]?["Error"];
        if (error != null)
        {
            var msg = $"{error["Code"]}: {error["Message"]}";
            throw new Exception($"Tencent TMT error: {msg}");
        }

        var data = parsedData?["Response"]?["TargetText"]?.ToString();
        if (string.IsNullOrEmpty(data)) throw new Exception($"No result.\nRaw: {response}");

        result.Success(data);
    }
}
