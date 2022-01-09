using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using _4n2h0ny.Steam.GUI.Models;
using System;
using System.Threading.Tasks;
using _4n2h0ny.Steam.GUI.EventArguments;
using System.Windows.Shell;

namespace _4n2h0ny.Steam.GUI
{
    public static class Comment
    {
        public static int NoFormCounter { get; set; }
        public static async Task CommentAllPages(MainWindow mainWindow, ChromeDriver driver, Profile profile, string commentTemplate, string defaultComment, OutputDialog outputDialog)
        {
            TaskBarProgressEventArgs taskBarProgressEventArgs = new()
            {
                ProgressValue = 0,
                TaskbarItemProgressState = TaskbarItemProgressState.Normal
            };

            for(int i = 0; i < profile.ProfileUrls.Count; i++)
            {
                driver.Navigate().GoToUrl(profile.ProfileUrls[i].Url);
                await Task.Delay(1000);

                var currentProfileData = profile.GetCurrentProfileData(outputDialog);

                if (CommentThreadFormAvailable(driver, currentProfileData, outputDialog))
                {
                    var commentString = String.Format(commentTemplate, currentProfileData.PersonaName);

                    PlaceCommentOnPage(driver, currentProfileData, commentString, defaultComment, outputDialog);
                    await Task.Delay(1000);
                }

                SqliteDataAccess.DeleteUrl(profile.ProfileUrls[i]);
                profile.ProfileUrls = SqliteDataAccess.GetAllUrls();

                taskBarProgressEventArgs.ProgressValue = ((double)i + 1) / (double)profile.ProfileUrls.Count;
                mainWindow.OnTaskbarProgressUpdated(taskBarProgressEventArgs);
            }
        }

        private static bool CommentThreadFormAvailable(ChromeDriver driver, ProfileDataModel profileData, OutputDialog outputDialog)
        {
            try
            {
                string commentThreadFormId = $"commentthread_Profile_{profileData.SteamId}_form";
                var commentNextBtn = driver.FindElement(By.Id(commentThreadFormId));

                if (commentNextBtn != null)
                {
                    return true;
                }
            }
            catch
            {
                NoFormCounter++;
                outputDialog.AppendLogTxtBox($"\n{NoFormCounter}: Could not find comment form: {profileData.Url}\n");
            }

            return false;
        }

        private static void PlaceCommentOnPage(ChromeDriver driver, ProfileDataModel currentProfileData, string commentString, string defaultComment, OutputDialog outputDialog)
        {
            try
            {
                var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));

                commentThreadTextAreaElement.SendKeys(String.Format(commentString, currentProfileData.PersonaName));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unknown error: ChromeDriver only supports characters in the BMP"))
                {
                    var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));
                    commentThreadTextAreaElement.SendKeys(defaultComment);
                    outputDialog.AppendLogTxtBox($"Default comment set for: " + currentProfileData.Url);
                }
                else
                {
                    outputDialog.AppendLogTxtBox($"Can't find text area for {currentProfileData.PersonaName}\n" + ex.Message);
                }
            }

            // SUBMITTING COMMENTS  
            ClickCommentSubmitBtn(driver, currentProfileData, outputDialog);
        }

        private static void ClickCommentSubmitBtn(ChromeDriver driver, ProfileDataModel currentProfileData, OutputDialog outputDialog)
        {
            try
            {
                var submitBtnElement = driver.FindElement(By.Id($"commentthread_Profile_{currentProfileData.SteamId}_submit"));
                submitBtnElement.Click();
            }
            catch (Exception ex)
            {
                outputDialog.AppendLogTxtBox("Can't find submit button\n" + ex.Message);
            }
        }

        public static void TestComment(ChromeDriver driver, Profile profile, string commentString, string defaultString, OutputDialog outputDialog)
        {
            var currentProfileData = profile.GetCurrentProfileData(outputDialog);

            try
            {
                var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));

                commentThreadTextAreaElement.SendKeys(String.Format(commentString, currentProfileData.PersonaName));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unknown error: ChromeDriver only supports characters in the BMP"))
                {
                    var commentThreadTextAreaElement = driver.FindElement(By.ClassName("commentthread_textarea"));
                    commentThreadTextAreaElement.SendKeys(defaultString);
                    outputDialog.AppendLogTxtBox($"Default comment set for: " + currentProfileData.Url);
                }
                else
                {
                    outputDialog.AppendLogTxtBox($"Can't find text area for {currentProfileData.PersonaName}\n" + ex.Message);
                }
            }
        }
    }
}
