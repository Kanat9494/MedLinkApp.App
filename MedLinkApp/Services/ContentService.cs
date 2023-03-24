namespace MedLinkApp.Services;

public class ContentService
{
    public ContentService()
    {

    }

    private static ContentService _instance;
    public static ContentService Instance()
    {
        if (_instance == null)
            _instance = new ContentService();

        return _instance;
    }

    public async Task<IEnumerable<Category>> LoadCategories()
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);

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

    //public async Task<TResponse> GetItemAsync<TResponse, TRequest>(TRequest request, string middleUrl) where TResponse : BaseResponse
    //{
    //    using (HttpClient httpClient = new HttpClient())
    //    {
    //        httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);

    //        try
    //        {
    //            var response = await httpClient.GetStringAsync(httpClient.BaseAddress + middleUrl + "/" + request);
    //            TResponse result = JsonConvert.DeserializeObject<TResponse>(response);
    //            result.StatusCode = 200;

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            var result = Activator.CreateInstance<TResponse>();
    //            result.StatusCode = 500;
    //            result.ResponseMessage = ex.Message;

    //            return result;
    //        }
    //    }
    //}

    public async Task<DoctorInfo> GetDoctorInfo(int doctorId)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);

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
