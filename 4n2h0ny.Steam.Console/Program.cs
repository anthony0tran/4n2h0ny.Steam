using _4n2h0ny.Steam;
#region Init
// Hook the chrome driver to a running session.
WebDriverSingleton webDriverSingleton = new ();

Profile profile = new(webDriverSingleton.Driver);
#endregion

GatherProfilesAndComment(profile, webDriverSingleton);

//TestComment(profile, webDriverSingleton);

static void GatherProfilesAndComment(Profile profile, WebDriverSingleton webDriverSingleton)
{
    // Retrieve the ProfileData of the first steam page that is opened in the browser session.
    profile.GetMainProfileData();

    // Get the urls to the profiles that commented.
    profile.GatherProfileUrls(Globals.MaxPageIndex);

    Comment.CommentAllPages(webDriverSingleton.Driver, profile, Profile.ProfileUrls, Globals.CommentTemplate);
}

static void TestComment(Profile profile, WebDriverSingleton webDriverSingleton)
{
    string testString = @"⠄⠄⣀⣤⣤⣤⣄⡀⠄⢀⣠⣤⣤⣄⡀⠄⢀⣠⣤⣤⣄⡀⢀⣠⣤⣤⣄⡀⠄⠄
⢀⣾⣿⡿⠿⠿⣿⣿⣦⢻⡿⠟⠿⣿⣿⣦⡹⣿⠿⠿⣿⣿⣦⠹⠿⠿⣿⣿⣧⡀
⠸⠿⠿⠄⠄⠄⢸⣿⣿⠄⠄⠄⠄⠈⢿⣿⣧⠄⠄⠄⠈⣿⣿⡇⠄⠄⠈⣿⣿⡇
⠄⠄⠄⠄⠄⣠⣾⣿⡟⠄⠄⠄⠄⠄⢸⣿⣿⠄⠄⢀⣼⣿⡿⠁⠄⢀⣴⣿⡿⠁
⠄⠄⢀⣠⣾⣿⠟⢋⣴⠄⠄⠄⠄⠄⢸⡿⠋⣠⣾⣿⠟⠋⠄⣠⣶⣿⠿⠋⠄⠄
⠄⣰⣿⡿⠋⠄⠈⣿⣿⣦⠄⠄⠄⢀⡾⣡⣾⣿⠋⠁⠄⢠⣾⣿⠟⠁⠄⠄⠄⠄
⢰⣿⣿⣿⣶⣶⣶⣌⠻⣿⣿⣶⣶⣿⢱⣿⣿⣿⣷⣶⠂⣿⣿⣿⣷⣶⣶⣶⣶⡆
⠈⠉⠉⠉⠉⠉⠉⠉⠁⠈⠙⠛⠛⠉⠈⠉⠉⠉⠉⠉⠄⠉⠉⠉⠉⠉⠉⠉⠉⠁
Have a great new year, {0}!";

    Comment.TestComment(webDriverSingleton.Driver, profile, testString);
}

#region Finish

webDriverSingleton.DisposeDriver();

ConsoleHelper.ConsoleWriteSuccess("Finished!");

Console.ReadKey();

#endregion