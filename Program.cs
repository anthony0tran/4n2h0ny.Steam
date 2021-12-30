using _4n2h0ny.Steam;
#region Init
// Hook the chrome driver to a running session.
WebDriverSingleton webDriverSingleton = new ();

Profile profile = new(webDriverSingleton.Driver);
#endregion

// Retrieve the ProfileData of the first steam page that is opened in the browser session.
profile.GetMainProfileData();

// Get the urls to the profiles that commented.
profile.GatherProfileUrls(30);

string commentTemplate = "Have a sparkling New Year {0}! :heart:";

Comment.CommentAllPages(webDriverSingleton.Driver, profile, Profile.ProfileUrls, commentTemplate);

#region Finish

webDriverSingleton.DisposeDriver();

ConsoleHelper.ConsoleWriteSuccess("Finished!");

Console.ReadKey();

#endregion