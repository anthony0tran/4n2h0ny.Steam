using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using _4n2h0ny.Steam.GUI.Models;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace _4n2h0ny.Steam.GUI
{
    public static class Comment
    {
        public static async Task CommentAllPages(ChromeDriver driver, Profile profile, List<string> urlList, string commentTemplate, string defaultComment, OutputDialog outputDialog)
        {
            foreach (var url in urlList)
            {
                driver.Navigate().GoToUrl(url);
                await Task.Delay(1000);

                var currentProfileData = profile.GetCurrentProfileData(outputDialog);

                if (CommentThreadFormAvailable(driver, currentProfileData, outputDialog))
                {
                    var commentString = String.Format(commentTemplate, currentProfileData.Personaname);

                    PlaceCommentOnPage(driver, currentProfileData, commentString, defaultComment, outputDialog);
                    await Task.Delay(1000);
                }
            }
        }

        private static bool CommentThreadFormAvailable(ChromeDriver driver, ProfileDataModel profileData, OutputDialog outputDialog)
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
                outputDialog.AppendLogTxtBox($"Could not find comment form: {profileData.Url}");
            }

            return false;
        }

        private static void PlaceCommentOnPage(ChromeDriver driver, ProfileDataModel currentProfileData, string commentString, string defaultComment, OutputDialog outputDialog)
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
                    commentThreadTextAreaElement.SendKeys(defaultComment);
                    outputDialog.AppendLogTxtBox($"Default comment set for: " + currentProfileData.Url);
                }
                else
                {
                    outputDialog.AppendLogTxtBox($"Can't find text area for {currentProfileData.Personaname}\n" + ex.Message);
                }
            }

            // SUBMITTING COMMENTS  
            try
            {
                var submitBtnElement = driver.FindElement(By.Id($"commentthread_Profile_{currentProfileData.Steamid}_submit"));
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

                commentThreadTextAreaElement.SendKeys(String.Format(commentString, currentProfileData.Personaname));
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
                    outputDialog.AppendLogTxtBox($"Can't find text area for {currentProfileData.Personaname}\n" + ex.Message);
                }
            }
        }
    }
}
