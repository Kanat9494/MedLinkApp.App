
namespace MedLinkApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseDevExpress()
			.UseBottomSheet()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts =>
			{
                fonts.AddFont("FiraSans-Regular", "RegularFont");
                fonts.AddFont("FiraSans-Medium.ttf", "MediumFont");
            });


		return builder.Build();
	}
}
