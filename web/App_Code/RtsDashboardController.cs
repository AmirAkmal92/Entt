using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;


[RoutePrefix("api/rts-dashboard")]
public class RtsDashboadController : BaseApiController
{

    private readonly HttpClient m_client = new HttpClient{BaseAddress = new Uri( ConfigurationManager.ElasticSearchHost)};
   
    [Route("")]
    [HttpPost]
    public async Task<IHttpActionResult> Aggregates([RawBody]string query)
    {
        var response = await m_client.PostAsync($"{ConfigurationManager.ElasticSearchIndex}_rts/_search", new StringContent(query));
        var content = response.Content as StreamContent;
        if (null == content) throw new Exception("Cannot execute query on es ");
        var result = await content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Cannot execute query for :  {query}, Result = {result} ");
        return Json(result);
    }

    [Route("{type}")]
    [HttpPost]
    public async Task<IHttpActionResult> SearchAsync(string type, 
        [RawBody]string query)
    {
        var response = await m_client.PostAsync($"{ConfigurationManager.ElasticSearchIndex}_rts/{type.ToLowerInvariant()}/_search", new StringContent(query));
        var content = response.Content as StreamContent;
        if (null == content) throw new Exception("Cannot execute query on es ");
        var result = await content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Cannot execute query for :  {query}, Result = {result} ");
        return Json(result);
    }
}