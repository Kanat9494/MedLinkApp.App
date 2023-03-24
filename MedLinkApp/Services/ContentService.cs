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

    public async Task<IEnumerable<DoctorResponse>> GetAllDoctors()
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);

            try
            {
                var response = await httpClient.GetStringAsync(httpClient.BaseAddress + "api/Doctors/GetAllDoctors");
                var doctors = JsonConvert.DeserializeObject<IEnumerable<DoctorResponse>>(response);

                return doctors;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
