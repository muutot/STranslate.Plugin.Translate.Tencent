using System.Security.Cryptography;
using System.Text;

namespace STranslate.Plugin.Translate.Tencent;

public static class TencentCloudSigner
{
    private const string Algorithm = "TC3-HMAC-SHA256";

    public static (string Signature, string Timestamp, string CredentialScope) Sign(
        string httpRequestMethod,
        string canonicalUri,
        string canonicalQueryString,
        string canonicalHeaders,
        string signedHeaders,
        string requestPayload,
        string secretKey,
        string date,
        string service)
    {
        var timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString();

        var hashedRequestPayload = HexEncode(SHA256.HashData(Encoding.UTF8.GetBytes(requestPayload)));

        var canonicalRequest = $"{httpRequestMethod}\n{canonicalUri}\n{canonicalQueryString}\n{canonicalHeaders}\n{signedHeaders}\n{hashedRequestPayload}";

        var credentialScope = $"{date}/{service}/tc3_request";

        var hashedCanonicalRequest = HexEncode(SHA256.HashData(Encoding.UTF8.GetBytes(canonicalRequest)));

        var stringToSign = $"{Algorithm}\n{timestamp}\n{credentialScope}\n{hashedCanonicalRequest}";

        var secretDate = HMACSHA256.HashData(Encoding.UTF8.GetBytes(secretKey), Encoding.UTF8.GetBytes("TC3" + date));
        var secretService = HMACSHA256.HashData(secretDate, Encoding.UTF8.GetBytes(service));
        var signKey = HMACSHA256.HashData(secretService, Encoding.UTF8.GetBytes("tc3_request"));

        var signature = HexEncode(HMACSHA256.HashData(signKey, Encoding.UTF8.GetBytes(stringToSign)));

        return (signature, timestamp, credentialScope);
    }

    private static string HexEncode(byte[] bytes) =>
        Convert.ToHexString(bytes).ToLowerInvariant();
}
