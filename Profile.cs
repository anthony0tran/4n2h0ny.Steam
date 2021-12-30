using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using _4n2h0ny.Steam.Models;
using Newtonsoft.Json;

namespace _4n2h0ny.Steam
{
    public class Profile
    {
        // List of all profile URLs.
        private static List<string> ProfileUrls = new List<string>();

        // ProfileData Retrieve of the first steam page.
        private static ProfileDataModel mainProfileData = new ProfileDataModel();

        public static ProfileDataModel? CurrentProfileData { get; set; }

        private readonly ChromeDriver driver;

        public Profile(ChromeDriver driver)
        {
            this.driver = driver;
        }

        public ProfileDataModel GetCurrentProfileData()
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
                ConsoleHelper.ConsoleWriteError("Could not retrieve profile data\n" + ex.Message);
            }
            
            return CurrentProfileData;
        }

        public void GetMainProfileData()
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
                ConsoleHelper.ConsoleWriteError("Could not retrieve main profile data\n" + ex.Message);
            }
        }

        private static string GetPersonaName()
        {
            if (CurrentProfileData != null)
            {
                return CurrentProfileData.Personaname;
            }
            
            return String.Empty;
        }

        private void ClickPageBtnNext()
        {
            try
            {
                string commentNextBtnId = $"commentthread_Profile_{mainProfileData.Steamid}_pagebtn_next";
                var commentNextBtn = driver.FindElement(By.Id(commentNextBtnId));
                commentNextBtn.Click();
            }
            catch (Exception ex)
            {
                ConsoleHelper.ConsoleWriteError("Could not find comments next button\n" + ex.Message);
            }
        }

        private int GetCurrentCommentPageIndex()
        {
            try
            {
                var activeCommentPageElement = driver.FindElements(By.XPath("//span[@class=\"commentthread_pagelink active\"]"));
                return int.Parse(activeCommentPageElement[0].Text);
            } 
            catch (Exception ex)
            {
                ConsoleHelper.ConsoleWriteError("Can't find active comment page index\n" + ex.Message);
                return default;
            }            
        }

        private void ReturnToFirstCommentPage()
        {
            if (GetCurrentCommentPageIndex() != 1)
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
                    ConsoleHelper.ConsoleWriteError("Can't find first comment page index\n" + ex.Message);
                }
                
            }
        }

        public void GatherProfileUrls(int maxCommentPageIndex = 10)
        {
            ReturnToFirstCommentPage();
            Thread.Sleep(1000);

            for (int i = 0; i < maxCommentPageIndex - 1; i++)
            {
                try
                {
                    string profileUrlsXPath = $"//div[@class=\"commentthread_comment_container\"]" +
                                              $"/div[@class=\"commentthread_comments\"]" +
                                              $"/div[contains(@class,\"commentthread_comment\")]" +
                                              $"/div[contains(@class,\"commentthread_comment_avatar\")]" +
                                              $"/a";

                    var commentElementList = driver.FindElements(By.XPath(profileUrlsXPath));

                    foreach (var commentElement in commentElementList)
                    {
                        if (!ProfileUrls.Contains(commentElement.GetAttribute("href")))
                        {
                            ProfileUrls.Add(commentElement.GetAttribute("href"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.ConsoleWriteError("Could not find profile url\n" + ex.Message);
                }

                ClickPageBtnNext();
                Thread.Sleep(1000);
            }
        }

        private bool IsFriend()
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
                ConsoleHelper.ConsoleWriteError("Could not find profile action button\n" + ex.Message);
            }

            return false;
        }
    }
}
