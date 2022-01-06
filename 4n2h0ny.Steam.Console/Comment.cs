using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using _4n2h0ny.Steam.Models;

namespace _4n2h0ny.Steam
{
    public static class Comment
    {
        public static void CommentAllPages(ChromeDriver driver, Profile profile, List<string> urlList, string commentTemplate)
        {
            foreach (var url in urlList)
            {
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(1000);

                var currentProfileData = profile.GetCurrentProfileData();

                if (CommentThreadFormAvailable(driver, currentProfileData))
                {
                    var commentString = String.Format(commentTemplate, currentProfileData.Personaname);

                    PlaceCommentOnPage(driver, currentProfileData, commentString);
                    Thread.Sleep(1000);
                }
            }
        }

        private static bool CommentThreadFormAvailable(ChromeDriver driver, ProfileDataModel profileData)
        {
            try
            {
                string commentThreadFormId = $"commentthread_Profile_{profileData.Steamid}_form";
                var commentNextBtn = driver.FindElement(By.Id(commentThreadFormId));

                if (commentNextBtn != null)
                {
                    return true;
                }
            }
            catch
            {
                ConsoleHelper.ConsoleWriteError($"Could not find comment form: {profileData.Url}");
            }

            return false;
        }

        private static void PlaceCommentOnPage(ChromeDriver driver, ProfileDataModel currentProfileData, string commentString)
        {
            try
            {
                var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));

                commentThreadTextAreaElement.SendKeys(String.Format(commentString, currentProfileData.Personaname));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unknown error: ChromeDriver only supports characters in the BMP"))
                {
                    var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));
                    commentThreadTextAreaElement.SendKeys(Globals.DefaultCommentString);
                    ConsoleHelper.ConsoleWriteError($"Default comment set for: " + currentProfileData.Url);
                }
                else
                {
                    ConsoleHelper.ConsoleWriteError($"Can't find text area for {currentProfileData.Personaname}\n" + ex.Message);
                }
            }


            try
            {
                var submitBtnElement = driver.FindElement(By.Id($"commentthread_Profile_{currentProfileData.Steamid}_submit"));
                submitBtnElement.Click();
            }
            catch (Exception ex)
            {
                ConsoleHelper.ConsoleWriteError("Can't find submit button\n" + ex.Message);
            }
        }

        public static void TestComment(ChromeDriver driver, Profile profile, string commentString)
        {
            var currentProfileData = profile.GetCurrentProfileData();

            try
            {
                var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));

                commentThreadTextAreaElement.SendKeys(String.Format(commentString, currentProfileData.Personaname));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unknown error: ChromeDriver only supports characters in the BMP"))
                {
                    var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));
                    commentThreadTextAreaElement.SendKeys(Globals.DefaultCommentString);
                    ConsoleHelper.ConsoleWriteError($"Default comment set for: " + currentProfileData.Url);
                }
                else
                {
                    ConsoleHelper.ConsoleWriteError($"Can't find text area for {currentProfileData.Personaname}\n" + ex.Message);
                }
            }
        }
    }
}
