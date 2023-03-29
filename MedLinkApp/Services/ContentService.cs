namespace MedLinkApp.Services;

public class ContentService
{
    public ContentService(string token)
    {
        

        accessToken = token;
    }

    private static ContentService _instance;
    public static ContentService Instance(string token)
    {
        if (_instance == null)
            _instance = new ContentService(token);

        return _instance;
    }

    HttpClient httpClient;
    string accessToken;

    public async Task<IEnumerable<Category>> LoadCategories()
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            try
            {
                var response = await httpClient.GetStringAsync(httpClient.BaseAddress + "api/Categories/LoadCategories");
                var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(response);

                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctors()
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            try
            {
                var response = await httpClient.GetStringAsync(httpClient.BaseAddress + "api/Doctors/GetAllDoctors");
                var doctors = JsonConvert.DeserializeObject<IEnumerable<Doctor>>(response);

                return doctors;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public async Task<TResponse> GetItemAsync<TResponse, TRequest>(string requestUrl) where TResponse : BaseResponse
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            try
            {
                var response = await httpClient.GetStringAsync(httpClient.BaseAddress + requestUrl);
                TResponse result = JsonConvert.DeserializeObject<TResponse>(response);
                result.StatusCode = 200;

                return result;
            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.StatusCode.HasValue)
                {
                    var result = Activator.CreateInstance<TResponse>();
                    result.StatusCode = (int)httpEx.StatusCode;
                    result.ResponseMessage = httpEx.Message;

                    return result;
                }
                else
                {
                    //request failed due to an exception (connection failure, etc...)
                    var result = Activator.CreateInstance<TResponse>();
                    result.StatusCode = 500;
                    result.ResponseMessage = httpEx.Message;

                    return result;

                }
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResponse>();
                result.StatusCode = 500;
                result.ResponseMessage = ex.Message;

                return result;
            }
        }
    }

    public async Task<TResponse> GetItemDataAsync<TResponse, TRequest>(string requestUrl) 
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            try
            {
                var response = await httpClient.GetStringAsync(httpClient.BaseAddress + requestUrl);
                TResponse result = JsonConvert.DeserializeObject<TResponse>(response);

                return result;
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResponse>();

                return result;
            }
        }
    }

    public async Task<IEnumerable<TResponse>> GetItemsAsync<TResponse>(string requestUrl)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            try
            {
                var response = await httpClient.GetStringAsync(httpClient.BaseAddress + requestUrl);
                IEnumerable<TResponse> result = JsonConvert.DeserializeObject<IEnumerable<TResponse>>(response);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public async Task<DoctorInfo> GetDoctorInfo(int doctorId)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            try
            {
                var response = await httpClient.GetStringAsync(httpClient.BaseAddress + "api/Doctors/GetDoctor/" + doctorId);

                DoctorInfo result = JsonConvert.DeserializeObject<DoctorInfo>(response);
                result.StatusCode = 200;

                return result;
            }
            catch (Exception ex)
            {
                return new DoctorInfo()
                {
                    StatusCode = 500,
                    ResponseMessage = ex.Message
                };
            }
        }
    }
}
