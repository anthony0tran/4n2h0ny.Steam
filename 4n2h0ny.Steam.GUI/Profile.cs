using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using _4n2h0ny.Steam.GUI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using _4n2h0ny.Steam.GUI.EventArguments;
using System.Windows.Shell;

namespace _4n2h0ny.Steam.GUI
{
    public class Profile
    {
        // List of all profile URLs.
        public List<SteamUrlModel> ProfileUrls { get; set; } = new();

        public List<SteamUrlModel> ManualProfileUrls { get; set; } = new();

        // ProfileData Retrieve of the first steam page.
        private ProfileDataModel mainProfileData = new();

        public ProfileDataModel CurrentProfileData = new();

        private readonly ChromeDriver driver;

        public Profile(ChromeDriver driver)
        {
            this.driver = driver;

            ProfileUrls = SqliteDataAccess.GetAllUrls();
        }

        public ProfileDataModel GetCurrentProfileData(OutputDialog outputDialog)
        {
            try
            {
                var allDOMScripts = driver.FindElements(By.XPath("//script[@type = 'text/javascript']"));

                foreach (var script in allDOMScripts)
                {
                    if (script.GetAttribute("innerHTML").Contains("g_rgProfileData "))
                    {
                        string profileDataString = script.GetAttribute("innerHTML");
                        int startIndex = profileDataString.IndexOf("{");
                        int endIndex = profileDataString.IndexOf("};");

                        if (endIndex >= 0 && startIndex >= 0)
                        {
                            profileDataString = profileDataString.Substring(startIndex, endIndex - startIndex + 1);
                        }

                        CurrentProfileData = JsonConvert.DeserializeObject<ProfileDataModel>(profileDataString);
                    }
                }
            }
            catch (Exception ex)
            {
                outputDialog.AppendLogTxtBox("Could not retrieve profile data\n" + ex.Message);
                CurrentProfileData = new ProfileDataModel();
            }

            return CurrentProfileData;
        }

        public void GetMainProfileData(OutputDialog outputDialog)
        {
            try
            {
                var allDOMScripts = driver.FindElements(By.XPath("//script[@type = 'text/javascript']"));

                foreach (var script in allDOMScripts)
                {
                    if (script.GetAttribute("innerHTML").Contains("g_rgProfileData "))
                    {
                        string profileDataString = script.GetAttribute("innerHTML");
                        int startIndex = profileDataString.IndexOf("{");
                        int endIndex = profileDataString.IndexOf("};");

                        if (endIndex >= 0 && startIndex >= 0)
                        {
                            profileDataString = profileDataString.Substring(startIndex, endIndex - startIndex + 1);
                        }

                        mainProfileData = JsonConvert.DeserializeObject<ProfileDataModel>(profileDataString);
                    }
                }
            }
            catch (Exception ex)
            {
                outputDialog.AppendLogTxtBox("Could not retrieve main profile data\n" + ex.Message);
            }
        }

        private void ClickPageBtnNext(OutputDialog outputDialog)
        {
            try
            {
                string commentNextBtnId = $"commentthread_Profile_{mainProfileData.SteamId}_pagebtn_next";
                var commentNextBtn = driver.FindElement(By.Id(commentNextBtnId));
                commentNextBtn.Click();
            }
            catch (Exception ex)
            {
                outputDialog.AppendLogTxtBox("Could not find comments next button\n" + ex.Message);
            }
        }

        private int GetCurrentCommentPageIndex(OutputDialog outputDialog)
        {
            try
            {
                var activeCommentPageElement = driver.FindElements(By.XPath("//span[@class=\"commentthread_pagelink active\"]"));
                return int.Parse(activeCommentPageElement[0].Text);
            }
            catch (Exception ex)
            {
                outputDialog.AppendLogTxtBox("Can't find active comment page index\n" + ex.Message);
                return default;
            }
        }

        private void ReturnToFirstCommentPage(OutputDialog outputDialog)
        {
            if (GetCurrentCommentPageIndex(outputDialog) != 1)
            {
                try
                {
                    var commentPageElement = driver.FindElements(By.XPath("//span[@class=\"commentthread_pagelink\"]"));
                    var firstCommentPageElements = commentPageElement.Where(x => int.Parse(x.Text) == 1);
                    foreach (var firstCommentPageElement in firstCommentPageElements)
                    {
                        firstCommentPageElement.Click();
                    }
                }
                catch (Exception ex)
                {
                    outputDialog.AppendLogTxtBox("Can't find first comment page index\n" + ex.Message);
                }

            }
        }

        // This function only adds profiles to the ProfileUrlsList if the profile is in the friendsList.
        public async Task GatherProfileUrls(MainWindow mainWindow, OutputDialog outputDialog, int maxCommentPageIndex = 20)
        {
            ReturnToFirstCommentPage(outputDialog);
            await Task.Delay(1000);

            TaskBarProgressEventArgs taskBarProgressEventArgs = new() { 
                ProgressValue = 0,
                TaskbarItemProgressState = TaskbarItemProgressState.Normal
            };

            for (int i = 0; i < maxCommentPageIndex; i++)
            {
                try
                {
                    string profileUrlsXPath = $"//div[@class=\"commentthread_comment_container\"]" +
                                              $"/div[@class=\"commentthread_comments\"]" +
                                              $"/div[contains(@class,\"commentthread_comment\")]";

                    var commentElementList = driver.FindElements(By.XPath(profileUrlsXPath));

                    foreach (var commentElement in commentElementList)
                    {
                        if (commentElement.FindElements(By.XPath("div[contains(@class,\"commentthread_comment_friendindicator\")]")).Count != 0)
                        {
                            var commentLinkElement = commentElement.FindElement(By.XPath("div[contains(@class,\"commentthread_comment_avatar\")]/a"));

                            List<SteamUrlModel> foundSteamUrl = GetSteamUrlByUrl(commentLinkElement.GetAttribute("href"));

                            if (foundSteamUrl.Count == 0)
                            {
                                SteamUrlModel steamUrl = new()
                                {
                                    Url = commentLinkElement.GetAttribute("href")
                                };

                                List<SteamUrlModel> foundSteamUrls = GetSteamUrlByUrl(steamUrl.Url);
                                
                                if (foundSteamUrls.Count == 0)
                                {
                                    SqliteDataAccess.SaveUrl(steamUrl);
                                    ProfileUrls = SqliteDataAccess.GetAllUrls();
                                }                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputDialog.AppendLogTxtBox("Could not find profile url\n" + ex.Message);
                }

                if (i < maxCommentPageIndex - 1)
                {
                    ClickPageBtnNext(outputDialog);
                }                

                taskBarProgressEventArgs.ProgressValue = ((double)i + 1) / (double)maxCommentPageIndex;
                mainWindow.OnTaskbarProgressUpdated(taskBarProgressEventArgs);

                await Task.Delay(1000);
            }

            ProfileUrls = SqliteDataAccess.GetAllUrls();
            outputDialog.AppendLogTxtBox($"Found {ProfileUrls.Count} profiles");
        }

        private List<SteamUrlModel> GetSteamUrlByUrl(string url)
        {
            return ProfileUrls.Where(x => x.Url == url).ToList();
        }

        private bool IsFriend(OutputDialog outputDialog)
        {
            try
            {
                var profileActionBtnElement = driver.FindElement(By.XPath("//a[@class=\"btn_profile_action btn_medium\"]/span"));

                if (profileActionBtnElement.Text == "Message")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                outputDialog.logTxtBox.Text += "Could not find profile action button\n" + ex.Message;
            }

            return false;
        }
    }
}
