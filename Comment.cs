using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using _4n2h0ny.Steam.Models;

namespace _4n2h0ny.Steam
{
    public static class Comment
    {
        public static bool CommentThreadFormAvailable(ChromeDriver driver, ProfileDataModel profileData)
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
            catch (Exception ex)
            {
                ConsoleHelper.ConsoleWriteError("Could not find comment form\n" + ex.Message);
            }

            return false;
        }
    }
}
