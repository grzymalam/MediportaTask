using System.Web;

namespace MediportaTask.Misc.Http;

public static class UriExtensions
{
    public static Uri AddQueryParameter(this Uri uri, string parameterName, string parameterValue)
    {
        var uriBuilder = new UriBuilder(uri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query[parameterName] = parameterValue;
        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }
}
